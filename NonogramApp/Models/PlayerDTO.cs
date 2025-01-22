namespace NonogramApp.Models
{
    public class PlayerDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public bool IsAdmin { get; set; }
        public string ProfileImagePath { get; set; } = "";
        public virtual List<LevelDTO> Levels { get; set; } = new List<LevelDTO>();
        public PlayerDTO() { }
        public PlayerDTO(string email, string password, string displayname, bool isAdmin)
        {
            this.Email = email;
            this.Password = password;
            this.DisplayName = displayname;
            IsAdmin = isAdmin;
        }
    }
}
