using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;
using Server.Models.Dapper;
using Server.Services;
using Server.Services.Authentication;
using Server.Services.Meeting;

namespace Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IJwtTokenManager _tokenManager;
        private readonly EmailValidator _emailValidator;

        public UserController(IUserService userService, IJwtTokenManager tokenManager, EmailValidator emailValidator)
        {
            _userService = userService;
            _tokenManager = tokenManager;
            _emailValidator = emailValidator;
        }

        private LoginResponse MapUserToResponse(User user)
        {
            var response = new LoginResponse
            {
                Email = user.Email,
                Username = user.Name,
                Token = _tokenManager.CreateToken(user.Id),
                RefreshToken = ""
            };

            return response;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest credentials)
        {
            var user = await _userService.Login(credentials.Email, credentials.Password);

            if (user.IsPresent)
            {
                return Ok(MapUserToResponse(user.Value));
            }

            return Unauthorized(new ErrorResponse { Message = "Authentication Failed!" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest credentials)
        {
            if (!_emailValidator.IsValidEmail(credentials.Email))
            {
                return BadRequest(new ErrorResponse { Message = "Invalid user email!" });
            }

            var user = await _userService.Register(credentials.Username, credentials.Email, credentials.Password);

            if (user.IsEmpty)
            {
                return Conflict(new ErrorResponse { Message = $"User with email: '{credentials.Email}' already exists!" });
            }

            return Ok(MapUserToResponse(user.Value));
        }

    }
}
