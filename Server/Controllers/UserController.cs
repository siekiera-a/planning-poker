using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;
using Server.Models.Dapper;
using Server.Services.Authentication;

namespace Server.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IJwtTokenManager _tokenManager;

        public UserController(IUserService userService, IJwtTokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        private LoginResponse MapUserToResponse(User user)
        {
            var response = new LoginResponse()
            {
                Email = user.Email,
                Username = user.Name,
                Token = _tokenManager.CreateToken(user.Id),
                RefreshToken = ""
            };

            return response;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest credentials)
        {
            var user = await _userService.Login(credentials.Email, credentials.Password);

            if (user.IsPresent)
            {
                return Ok(MapUserToResponse(user.Value));
            }

            return Unauthorized(new { Message = "Authentication Failed!" });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest credentials)
        {
            var user = await _userService.Register(credentials.Username, credentials.Email, credentials.Password);

            if (user.IsEmpty)
            {
                return Conflict(new { Message = $"User with email: '{credentials.Email}' already exists!" });
            }

            return Ok(MapUserToResponse(user.Value));
        }


        public async Task<IActionResult> Logout()
        {
            throw new NotImplementedException("logout");
        }

    }
}
