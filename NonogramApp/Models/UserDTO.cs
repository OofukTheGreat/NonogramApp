﻿namespace AppServer.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public UserDTO() { }
        public UserDTO(string email, string password, string displayname)
        {
            this.Email = email;
            this.Password = password;
            this.DisplayName = displayname;
        }
    }
}
