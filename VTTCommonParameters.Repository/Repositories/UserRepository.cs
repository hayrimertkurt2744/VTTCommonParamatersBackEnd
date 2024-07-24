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
        public ResponseModel UserLogin(string email, string password, SymmetricSecurityKey key)
        {
            ResponseModel response = new ResponseModel();

            var userMail = _context.Users.FirstOrDefault(x => x.Email == email);

            if (userMail == null)
            {
                response.Result = false;
                response.ErrorMessge = "No user.";
                response.Data = "";
                return response;
            }

            var userHashedPassword = userMail.Password;

            if (!BCrypt.Net.BCrypt.Verify(password, userHashedPassword))
            {
                response.Result = false;
                response.ErrorMessge = "No pwd match.";
                response.Data = "";
                return response;
            }

            var existingToken = _context.RefreshTokens.FirstOrDefault(x => x.UserId == userMail.Id);

            if (existingToken == null || existingToken.Expires < DateTime.Now)
            {
                // Token doesn't exist or is expired, generate a new token
                string token = GenerateToken(email, key);
                var refreshToken = new RefreshToken
                {
                    Token = token,
                    UserId = userMail.Id,
                    Expires = DateTime.Now.AddMinutes(30), // Set your token expiration time here
                    Created = DateTime.Now
                };

                _context.RefreshTokens.Add(refreshToken);
                _context.SaveChanges();

                response.Data = refreshToken.Token;
            }
            else if (existingToken.Expires > DateTime.Now)
            {
                // Token exists and is not expired
                response.Data = existingToken.Token;

                var repeatToken = new RefreshToken
                {
                    Token = existingToken.Token,
                    UserId = userMail.Id,
                    Expires = existingToken.Expires, // Set your token expiration time here
                    Created = DateTime.Now
                };
                _context.RefreshTokens.Add(repeatToken);
                _context.SaveChanges();
            }

            response.Result = true;
            return response;
        }

        private string GenerateToken(string email, SymmetricSecurityKey key)
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
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
