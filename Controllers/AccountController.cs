using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.ClassLibrary1;
public class AccountController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext dataContext, ITokenService tokenService)
    {
        _dataContext = dataContext;
        _tokenService = tokenService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDto registerDto)
    {
        if (await isUserExists(registerDto.Username!))
            return BadRequest("username is already exists");

        using var hmacSHA256 = new HMACSHA256();
        var user = new AppUser
        {
            UserName = registerDto.Username!.Trim().ToLower(),
            PasswordHash = hmacSHA256.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!.Trim())),
            PasswordSalt = hmacSHA256.Key
        };

        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        return new UserDTO
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    private async Task<bool> isUserExists(string username)
    {
        return await _dataContext.Users.AnyAsync(user => user.UserName == username.ToLower());
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(user =>
                            user.UserName == loginDto.UserName);

        if (user is null) return Unauthorized("invalid username");

        using var hmacSHA256 = new HMACSHA256(user.PasswordSalt!);

        var computedHash = hmacSHA256.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password!.Trim()));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash?[i]) return Unauthorized("invalid password");
        }
        return new UserDTO
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };;
    }
}