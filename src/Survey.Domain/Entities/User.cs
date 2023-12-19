using Survey.Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int IncorrectEntry { get; set; } // Hatalı giriş Sayısı
        public DateTime? NextTime { get; set; }  
        public string PasswordHash { get; set; }
        public LoginType LoginType { get; set; }
    
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

    }
}