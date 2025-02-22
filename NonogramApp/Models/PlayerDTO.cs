﻿namespace NonogramApp.Models
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public bool IsAdmin { get; set; }
        public string ProfileImagePath { get; set; } = "";
        public PlayerDTO() { }
        public PlayerDTO(int id, string email, string password, string displayname, bool isAdmin)
        {
            this.Id = id;
            this.Email = email;
            this.Password = password;
            this.DisplayName = displayname;
            IsAdmin = isAdmin;
        }
    }
}
