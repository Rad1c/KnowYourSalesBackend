using BLL.Enums;
using BLL.IServices;
using DAL.IRepositories;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MODEL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services;

public sealed class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IAuthRepository _authRepository;
    public AuthService(IConfiguration configuration, IAuthRepository authRepository)
    {
        _configuration = configuration;
        _authRepository = authRepository;
    }

    public string CreateToken(Guid userId, Guid accountId, TokenTypeEnum tokenType, RoleEnum role)
    {
        List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("type", tokenType.ToString()),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim("accountId", accountId.ToString()),
            };

        SymmetricSecurityKey key = new(
            Encoding.UTF8.GetBytes(_configuration.GetSection("Token:SECRET_KEY").Value!)
        );

        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = creds
        };

        double days = int.Parse(
            tokenType == TokenTypeEnum.AccessToken ? _configuration.GetSection("Token:AccessTokenValidity").Value! :
            _configuration.GetSection("Token:RefreshTokenValidity").Value!);
        tokenDescriptor.Expires = DateTime.Now.AddDays(days);


        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.ASCII.GetBytes(password));
    }
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        var hmac = new HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
    public Dictionary<string, string> ValidateToken(string token)
    {
        Dictionary<string, string> claimsList = new();
        if (token == null)
            return claimsList;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Token:SECRET_KEY").Value!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            claimsList = jwtToken.Claims.ToDictionary(x => x.Type, x => x.Value);

            return claimsList;
        }
        catch
        {
            return claimsList;
        }
    }
    public async Task<ErrorOr<Account?>> GetAccountByEmail(string email)
    {
        Account? account = await _authRepository.GetAccountByEmail(email);

        if (account == null) return Errors.Errors.Auth.InvalidCredentials;

        return account;
    }
}
