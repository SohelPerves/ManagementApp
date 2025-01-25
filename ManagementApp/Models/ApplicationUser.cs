namespace ManagementApp.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime RegistrationTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }
}
