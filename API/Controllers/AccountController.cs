using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, IMapper mapper,
             ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] RegisterDto registerDto)
        {
            if (_userManager.Users.Any(x => x.UserName == registerDto.UserName)) return BadRequest("Username is taken");

            if (_userManager.Users.Any(x => x.Email == registerDto.Email)) return BadRequest("Email is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return new UserDto 
            {
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
                UserName = user.UserName
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LoginUser([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .Include(fr => fr.FriendshipRequestsReceived)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid credentials");

            return new UserDto
            {
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                FriendRequestsCount = user.FriendshipRequestsReceived.Count()
            };
        }
    }
}