﻿namespace JwtAuth
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}