﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserPoolingApi.Services;
using UserPoolingApi.Models;
using UserPoolingApi.ViewModels;

namespace UserPoolingApi.Controllers
{
    [Produces("application/json")]
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;

        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register([FromBody] AdminRegistrationViewModel registerVM)
        {
            try
            {
                if (await _repo.UserExists(registerVM.Username))
                {
                    ModelState.AddModelError("Username", "Username is already exists.");
                }

                if (!ModelState.IsValid)
                {
                    return Json(BadRequest(ModelState));
                }
                registerVM.Username = registerVM.Username.ToLower();
                registerVM.Email = registerVM.Email.ToLower();

                var adminToCreate = new Admin
                {
                    Username = registerVM.Username,
                    Email = registerVM.Email
                };

                await _repo.Register(adminToCreate, registerVM.Password);

                return Json(StatusCode(201));
            }
            catch (Exception ex)
            {
                return Json(BadRequest(ex));
            }           
        } 

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginViewModel loginVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(StatusCode(400));
                    //return StatusCode(400, Json("Bad Request"));
                    //return BadRequest(ModelState);
                }
                var adminFromRepo = await _repo.Login(loginVM.Username, loginVM.Password);
                if(adminFromRepo == null)
                {
                    return Json(StatusCode(401));
                    //return StatusCode(401, Json("Username or password is incorrect."));
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                // _config is IConfiguration object from Microsoft Extension
                var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value); 
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, adminFromRepo.AdminId.ToString()),
                        new Claim(ClaimTypes.Name, adminFromRepo.Username)
                    }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)   
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                
                return Json(StatusCode(201, tokenString));
                //return Ok(new { tokenString });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}