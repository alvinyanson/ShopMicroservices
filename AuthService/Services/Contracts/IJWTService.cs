using System.Security.Claims;

namespace AuthService.Services.Contracts
{
    public interface IJWTService
    {
        string GenerateJwtToken(IEnumerable<Claim> claims);
    }
}
