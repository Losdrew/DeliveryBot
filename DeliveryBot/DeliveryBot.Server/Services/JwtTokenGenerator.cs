using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryBot.Server.Services;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;

    public JwtTokenGenerator(IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> GenerateAsync(IdentityUser user)
    {
        var claims = GetClaimsForUser(user);
        var roleNames = await _userManager.GetRolesAsync(user);
        AddRoleClaims(claims, roleNames);
        var token = CreateJwtToken(claims);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> GetClaimsForUser(IdentityUser user)
    {
        var currentTimeOffset = DateTimeOffset.Now;
        var validFrom = currentTimeOffset.ToUnixTimeSeconds();
        var validTo = currentTimeOffset.AddDays(30).ToUnixTimeSeconds();

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Nbf, validFrom.ToString()),
            new(JwtRegisteredClaimNames.Exp, validTo.ToString())
        };
        return claims;
    }

    private void AddRoleClaims(ICollection<Claim> claims, ICollection<string> roleNames)
    {
        foreach (var roleName in roleNames)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
        }
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims)
    {
        var securityKey = _configuration.GetRequiredSection("SecurityKey").Value;
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var header = new JwtHeader(signingCredentials);
        return new JwtSecurityToken(header, new JwtPayload(claims));
    }
}
