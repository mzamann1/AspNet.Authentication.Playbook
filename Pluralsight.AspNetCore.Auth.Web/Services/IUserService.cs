﻿using System.Threading.Tasks;

namespace Pluralsight.AspNetCore.Auth.Web.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string username, string password, out User user);
    }

    public class User
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
