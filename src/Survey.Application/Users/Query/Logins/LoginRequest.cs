using System.ComponentModel.DataAnnotations;

namespace Survey.Application;

public class LoginRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    [StringLength(8, ErrorMessage = "Identifier too long (8 character limit).")]
    public string Password { get; set; }
}
