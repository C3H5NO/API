using API.Controllers;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [AllowAnonymous]

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        // var users = await _userRepository.GetUsersAsync();
        // return Ok(_mapper.Map<IEnumerable<MemberDto>>(users));
        return Ok(await _userRepository.GetMembersAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto?>> GetUser(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return _mapper.Map<MemberDto>(user);
    }

    [HttpGet("username/{username}")]

    public async Task<MemberDto?> GetMemberByUsername(string username)
    {
        return await _userRepository.GetMemberByUserNameAsync(username);
    }

}