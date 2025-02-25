using NonogramApp.Services;

namespace NonogramApp.Models
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public bool IsAdmin { get; set; }
        public string ProfilePicture { get; set; } = "";
        public string FullUrl
        {
            get
            {
                return NonogramService.ImageBaseAddress + this.ProfilePicture;
            }
        }
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
