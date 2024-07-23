using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VTTCommonParamaters.Api.Models;
using VTTCommonParameters.Dal.Entities.AccountEntities;
using VTTCommonParameters.Repository;
using VTTCommonParameters.Repository.Repositories;


namespace VTTCommonParamaters.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //public static User user = new User();
        

        [HttpPost("register")]
        public ActionResult<User> Register(User user)
        {
            UserRepository repository=new UserRepository();
            string response=repository.UserRegister(user);

            return Ok(new { message= response });
        }

        [HttpPost("login")]
        public ActionResult<string> Login(User user)
        {

            //string token = CreateToken(user);
            UserRepository repository = new UserRepository();
            string response = repository.UserLogin(user);

            return Ok(new {message=response} );
        }

        ////Make CreateToken method private as it is not an action method
        //    private string CreateToken(User user)
        //{
        //    List<Claim> claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Email, user.Email),
        //            new Claim(ClaimTypes.Role, "Admin")
        //        };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        //        _configuration.GetSection("AppSettings:Token").Value!));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds
        //    );

        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //    return jwt;
        //}
    }
}