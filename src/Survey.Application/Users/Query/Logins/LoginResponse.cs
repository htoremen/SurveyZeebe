using System.Runtime.Serialization;

namespace Survey.Application;

//[DataContract(Name = "data")]
public class LoginResponse
{
    public LoginResponse(User user, string jwtToken, string refreshToken)
    {
        if (user != null)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
        }
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }

    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }

    public string RefreshToken { get; set; }

}
