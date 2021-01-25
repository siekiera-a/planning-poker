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
using Server.Services.Meeting;

namespace Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IJwtTokenManager _tokenManager;
        private readonly IMeetingService _meetingService;

        public UserController(IUserService userService, IJwtTokenManager tokenManager, IMeetingService meetingService)
        {
            _userService = userService;
            _tokenManager = tokenManager;
            _meetingService = meetingService;
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
            var user = await _userService.Register(credentials.Username, credentials.Email, credentials.Password);

            if (user.IsEmpty)
            {
                return Conflict(new ErrorResponse { Message = $"User with email: '{credentials.Email}' already exists!" });
            }

            return Ok(MapUserToResponse(user.Value));
        }

        [Authorize]
        [HttpGet("results")]
        public async Task<IActionResult> GetResults(DateTimeRequest request)
        {
            var response = await _meetingService.GetResults(request.DateTime);
            return Ok(response); // List<UserResultResponse>
        }


        //public async Task<IActionResult> Logout()
        //{
        //    throw new NotImplementedException("logout");
        //}

    }
}
