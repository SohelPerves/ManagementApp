using ManagementApp.Models;

namespace ManagementApp.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password && !u.IsBlocked);
            if (user != null)
            {
                user.LastLoginTime = DateTime.Now;
                _context.SaveChanges();
            }
            return user;
        }

        public void Register(ApplicationUser user)
        {
            user.RegistrationTime = DateTime.UtcNow;
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }

}
