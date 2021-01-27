using chat_server.Entity;
using chat_server.Entity.interfaces;
using chat_server.Manager.IManagers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Manager
{
	public class ManagerUser:IManagerUser
    {
        //private IGenericService<UserVM> _genericService;
        private IAuth<UserVM> _auth;
        public ManagerUser(IAuth<UserVM> auth)
		{
            _auth = auth;
    }
        public async Task<Result> loginDetails(UserVM model, string tokenValue)
        {
            Result result;
            UserVM userData;
            try
            {
                string email = "", userId = "", firstName = "", lastName = "";
                result = await _auth.Login(model);
                if (result != null)
                {
                    if (result.IsSuccess && result.Data != null)
                    {
                        userData = (UserVM)result.Data;
                        email = ((UserVM)result.Data).Email;
                        userId = ((UserVM)result.Data).UserId.ToString();
                        firstName = ((UserVM)result.Data).FirstName;
                        lastName = ((UserVM)result.Data).LastName;
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, userId),
                            new Claim("USERID", !string.IsNullOrEmpty(userId) ? userId : ""),
                            new Claim("EMAIL", !string.IsNullOrEmpty(email) ? email : "")
                        };
                        
                        byte[] tokenByte = Encoding.UTF8.GetBytes(tokenValue);
                        var key = new SymmetricSecurityKey(tokenByte);

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                        var JWToken = new JwtSecurityToken(
                             issuer: "https://localhost:44319/",
                             audience: "https://localhost:44319/",
                             claims: GetUserClaims(userData),
                             notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                             expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                             signingCredentials: creds
                         );
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                        result.Message = token;
                        result.Data = userData;
                    }
                    else
                    {
                        result.Message = "Invalid username or password";
                        result.IsSuccess = false;
                        return result;
                    }
                }
                
                else
                {
                    result.Message = "Invalid username or password";
                    result.IsSuccess = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = new Result();
                result.Message = ex.Message.ToString();
                result.IsSuccess = false;
                return result;
            }
            return result;
            
        }
    private IEnumerable<Claim> GetUserClaims(UserVM user)
    {
        List<Claim> claims = new List<Claim>();
        Claim _claim;
        _claim = new Claim(ClaimTypes.Name, user.UserId.ToString());
        claims.Add(_claim);
        _claim = new Claim("USERID", user.UserId.ToString());
        claims.Add(_claim);
        _claim = new Claim("EMAIL", user.Email);
        claims.Add(_claim);
        //if (user.WRITE_ACCESS != "")
        //{
        //    _claim = new Claim(user.WRITE_ACCESS, user.WRITE_ACCESS);
        //    claims.Add(_claim);
        //}
        return claims.AsEnumerable<Claim>();
    }

		
	}
}
