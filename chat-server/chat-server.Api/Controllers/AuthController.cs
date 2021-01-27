using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using chat_server.Entity.interfaces;
using chat_server.Entity;
using chat_server.Manager.IManagers;

namespace chat_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
        private IAuth<UserVM> _auth;
        private readonly IConfiguration _config;
        IManagerUser _managerUser;
        public AuthController(IConfiguration config, IAuth<UserVM> auth, IManagerUser managerUser)
		{
            _config = config;
            _auth = auth;
            _managerUser = managerUser;
        }
        [HttpPost("register")]
        public async Task<Result> Register([FromBody] UserVM value)
        {
            return await _auth.Register(value);
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] UserVM model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                model.ForLogin = true;
                string tokenValue = _config.GetSection("AppSettings:Token").Value;
                Result result = await _managerUser.loginDetails(model, tokenValue);
                if (result.IsSuccess)
                {
                    string token = result.Message;
                    UserVM userData = (UserVM)result.Data;

                    HttpContext.Session.SetString("JWToken", token);
                    return Ok(new
                    {
                        Message = "Login Successful",
                        token = token,
                        statusCode = StatusCode(201),
                        userId = userData.UserId,
                        firstName = userData.FirstName,
                        lastName = userData.LastName,
                        email = model.Email
                    });
                }
                return Unauthorized("Email Not Found!");
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BadRequest(error: ex.Message.ToString());
            }

        }
        [HttpPost("logout")]
        public async Task<bool> Logoff()
        {
            try
            {
                await Task.Run(() =>
                {
                    HttpContext.Session.Clear();
                });
                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
    }
}
