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
using VTTCommonParameters.Repository.Dto;

namespace VTTCommonParameters.Repository.Repositories
{
    public class UserRepository
    {
        private readonly VTTCommonParametersContext _context;
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
        public ResponseModel UserLogin(string email, string password,SymmetricSecurityKey key)
        {
            ResponseModel response = new ResponseModel();

            var userMail = _context.Users
                .FirstOrDefault(x => x.Email == email);

            var userHashedPassword = userMail?.Password;

            if (userMail == null)
            {
                response.Result = false;
                response.ErrorMessge =  "No user.";
                return response;
            }
            if (!BCrypt.Net.BCrypt.Verify(password, userHashedPassword))
            {
                response.Result = false;
                response.ErrorMessge = "No pwd match.";
                return response;
            }
            string token = RefreshToken(email,key);
            response.Data = token;
            return response;
        }

        private string RefreshToken(string email, SymmetricSecurityKey key)
        {


            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                //new Claim(ClaimTypes.Role, "Admin")
            };

            //You cant generate key here. IConfiguration can only be used in main API.

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            //    _configuration.GetSection("AppSettings:Token").Value!));

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
