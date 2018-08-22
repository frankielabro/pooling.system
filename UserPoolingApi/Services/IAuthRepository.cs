using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserPoolingApi.Models;

namespace UserPoolingApi.Services
{
    public interface IAuthRepository
    {
        Task<Admin> Register(Admin admin, string password);
        Task<Admin> Login(string username, string password);
        Task<bool> UserExists(string username);


    }
}
