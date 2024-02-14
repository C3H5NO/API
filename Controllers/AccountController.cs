using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.ClassLibrary1;
public class AccountController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AccountController(IMapper mapper, UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDto registerDto)
    {
        if (await isUserExists(registerDto.Username!))
            return BadRequest("username is already exists");

        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.Username!.Trim().ToLower();

        var appUser = await _userManager.CreateAsync(user, registerDto.Password!);
        if (!appUser.Succeeded) return BadRequest(appUser.Errors);

        var role = await _userManager.AddToRoleAsync(user, "Member");
        if (!role.Succeeded) return BadRequest(role.Errors);
        
        return new UserDTO
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            Aka = user.Aka,
            Gender = user.Gender,
        };
    }

    private async Task<bool> isUserExists(string username)
    {
        return await _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        var user = await _userManager.Users
                        .Include(photo => photo.Photos)
                        .SingleOrDefaultAsync(user =>
                        user.UserName == loginDto.UserName);

        if (user is null) return Unauthorized("invalid username");
        var appUser = await _userManager.CheckPasswordAsync(user, loginDto.Password!);
        if (!appUser) return BadRequest("invalid password");
        return new UserDTO
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
            Aka = user.Aka,
            Gender = user.Gender,
        };
    }
}