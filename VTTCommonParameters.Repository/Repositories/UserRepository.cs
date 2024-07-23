using Azure.Core;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VTTCommonParameters.Dal;
using VTTCommonParameters.Dal.Entities.AccountEntities;

namespace VTTCommonParameters.Repository.Repositories
{
    public class UserRepository
    {
        private readonly VTTCommonParametersContext _context;
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _context = new VTTCommonParametersContext();
            _configuration = configuration;
        }
        public UserRepository()
        {
            _context = new VTTCommonParametersContext();
        }


        public string UserRegister(User registerUser/*,string email, string password*/)
        {
            var userMail = _context.Users
                .FirstOrDefault(x => x.Email == registerUser.Email);

            if (userMail == null)
            {
             
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
                registerUser.Password = passwordHash;

                _context.AddRange(registerUser);
                _context.SaveChanges();

                return "ok";

            }
            return "Email Exists! Please register with a valid Email.";

        }
        public string UserLogin(User loginUser) {

            var userMail = _context.Users
                .FirstOrDefault(x => x.Email == loginUser.Email);

            var userHashedPassword=userMail?.Password;

            if (userMail == null) { 

                return "No such user."; 
            }
            if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, userHashedPassword))
            {
                return "Wrong Password!";
            }

            return "Ok";
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
