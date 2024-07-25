using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using VTTCommonParameters.Dal.Entities.AccountEntities;
using VTTCommonParameters.Repository;
using VTTCommonParameters.Repository.Dto;
using VTTCommonParameters.Repository.Repositories;


namespace VTTCommonParamaters.Api.Controllers
{

    

    [Route("[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        //public static User user = new User();
        public AuthController(IConfiguration configuration)
        {
            _configuration= configuration;
        }

        [HttpPost("Register")]
        public ActionResult<User> Register(User user)
        {
            UserRepository repository=new UserRepository();
            string response=repository.UserRegister(user);

            return Ok(new { message= response });
        }

        [HttpPost("Login")]
        public ActionResult<string> Login(string email, string password)
        {

            //string token = CreateToken(user);
            UserRepository repository = new UserRepository();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));
            ResponseModel response = repository.UserLogin(email, password, key);

            if(response.Result)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

    }
}