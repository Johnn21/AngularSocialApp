using System.Text;
using System.Web;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, IMapper mapper,
             ITokenService tokenService, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _signInManager = signInManager;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> RegisterUser([FromBody] RegisterDto registerDto)
        {
            if (_userManager.Users.Any(x => x.UserName == registerDto.UserName)) return BadRequest("Username is taken");

            if (_userManager.Users.Any(x => x.Email == registerDto.Email)) return BadRequest("Email is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            if (result.Succeeded) 
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                user.AccountConfirmationToken = code;

                await _userManager.UpdateAsync(user);

                var encodedConfirmationToken = Base64UrlEncoder.Encode(code);

                var msg = @$"Hello, <b>{user.UserName}</b>!</br>
                    By clicking on this <a href='http://localhost:4200/activate-account/{encodedConfirmationToken}/{user.UserName}'>link</a> you will activate your account</br></br>
                    Regards from AngularSocialApp!";

                await _emailService.SendAsync(user.Email, "Account confirmation", msg, true);

                return Ok(true);
            }

            return BadRequest("Failed to create new account");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LoginUser([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .Include(fr => fr.FriendshipRequestsReceived)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("Invalid credentials");

            if (!user.EmailConfirmed) return Unauthorized("Email not confirmed");

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

        [HttpPost("activate-account")]
        public async Task<ActionResult<bool>> ActivateAccount([FromBody]ActivateAccountDto activateAccountDto)
        {
            var user = await _userManager.Users
                .Where(x => x.UserName == activateAccountDto.Username)
                .SingleOrDefaultAsync();

            if (user == null) return BadRequest("This user does not exist");

            if (user.EmailConfirmed) return BadRequest("This user have already a confirmed email");

            if (user.AccountConfirmationToken.Equals(user.AccountConfirmationToken))
            {
                user.EmailConfirmed = true;
                user.AccountConfirmationToken = null;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return Ok(true);
            }

            return BadRequest("Failed to activate user");
        }

        [HttpPost("reset-password-email-confirmation/{email}")]
        public async Task<ActionResult> ResetPasswordEmailConfirmation(string email)
        {
            var user = await _userManager.Users
                .Where(x => x.Email == email)
                .SingleOrDefaultAsync();
            
            if (user == null) return BadRequest("This user does not exist");

            if (!user.EmailConfirmed) return BadRequest("This user does not have a confirmed email");

            Random _rdm = new Random();
            user.ResetPasswordCode = _rdm.Next(1000, 9999).ToString();

            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded) 
            {
                try
                {
                    var msg = @$"Hello, <b>{user.UserName}</b>!</br>
                        Your reset password code: {user.ResetPasswordCode}</br></br>
                        Regards from AngularSocialApp!";

                    await _emailService.SendAsync(user.Email, "Reset password", msg, true);

                    return NoContent();
                }
                catch
                {
                    return BadRequest("Failed to send reset password to this email");
                }
            }

            return BadRequest("Failed to send reset password code to user email");
        }

        [HttpPost("reset-password-check-code/{code}/{email}")]
        public async Task<ActionResult> ResetPasswordCheckCode(string code, string email)
        {
            var user = await _userManager.Users
                .Where(x => x.Email == email)
                .SingleOrDefaultAsync();

            if (user == null) return BadRequest("This user does not exist");

            if (!user.EmailConfirmed) return BadRequest("This user does not have a confirmed email");

            if (string.IsNullOrEmpty(user.ResetPasswordCode) || user.ResetPasswordCode != code) 
                return BadRequest("Invalid reset code");

            if (user.ResetPasswordCode == code) 
            {
                user.ResetPasswordCode = null;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) 
                {
                    return NoContent();
                }
            }

            return BadRequest("Failed to check reset password code");
        }

        [HttpPost("reset-password-new")]
        public async Task<ActionResult> ResetPasswordNew([FromBody]ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.Users
                .Where(x => x.Email == resetPasswordDto.Email)
                .SingleOrDefaultAsync();

            if (user == null) return BadRequest("This user does not exist");

            if (!user.EmailConfirmed) return BadRequest("This user does not have a confirmed email");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            if (result.Succeeded) return NoContent();

            return BadRequest("Failed to change password");
        }
    }
}