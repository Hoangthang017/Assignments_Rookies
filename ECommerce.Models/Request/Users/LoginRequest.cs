﻿namespace ECommerce.Models.Request.Users
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string Scope { get; set; }
    }
}