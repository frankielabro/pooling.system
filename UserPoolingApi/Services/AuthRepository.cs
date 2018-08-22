using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserPoolingApi.EntityFramework;
using UserPoolingApi.Models;

namespace UserPoolingApi.Services
{
    /*This AuthRepository is for the authentication of Admins*/
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Admin> Register(Admin admin, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            admin.PasswordHash = passwordHash;
            admin.PasswordSalt = passwordSalt;
            admin.Created_at = DateTime.Now;

            await _context.Admins.AddAsync(admin); //Add a row in the "Admins" table with the value "admin"
            await _context.SaveChangesAsync(); //Save changes in the database

            return admin;
        }

        public async Task<Admin> Login(string username, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
            if(admin == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, admin.PasswordHash, admin.PasswordSalt))
            {
                return null;
            }

            return admin;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Admins.AnyAsync(a => a.Username == username))
            {
                return true;
            }
            return false;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /*=========NEW METHODS HERE=========*/
        /*Authentication methods made for Client Login/Registration here*/

       

        
    }
}
