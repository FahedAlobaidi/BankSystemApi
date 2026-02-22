using BankSystemApi.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BankSystemApi.TokenService
{
    /// <summary>
    /// JWT Token Service - Creates and manages JWT authentication tokens
    /// Last Updated: 2025-01-26
    /// 
    /// PURPOSE:
    /// Generates JWT (JSON Web Token) access tokens for authenticated users
    /// Tokens are used for stateless authentication in REST APIs
    /// 
    /// WHAT IS A JWT?
    /// A JWT is a compact, URL-safe token consisting of three parts:
    /// 1. HEADER: Algorithm and token type {"alg":"HS256","typ":"JWT"}
    /// 2. PAYLOAD: Claims (user data) {"sub":"123","email":"user@example.com",...}
    /// 3. SIGNATURE: Cryptographic signature ensuring integrity
    /// 
    /// Format: xxxxx.yyyyy.zzzzz (header.payload.signature)
    /// 
    /// WHY JWT FOR BANKING API?
    /// - Stateless: Server doesn't store session data (scalable)
    /// - Self-contained: Token carries all user info needed for authorization
    /// - Secure: Cryptographically signed (can't be tampered with)
    /// - Standard: Works across different platforms/languages
    /// - API-friendly: Easy to send in HTTP headers
    /// 
    /// AUTHENTICATION FLOW:
    /// 1. User logs in with credentials (POST /api/auth/login)
    /// 2. Server validates credentials
    /// 3. TokenService.CreateToken(user) generates JWT
    /// 4. JWT sent to client in response
    /// 5. Client stores JWT (localStorage, cookie, etc.)
    /// 6. Client sends JWT in Authorization header for subsequent requests
    /// 7. Server validates JWT signature and extracts claims
    /// 8. Server authorizes request based on claims (role, user id, etc.)
    /// 
    /// SECURITY CONSIDERATIONS:
    /// ⚠️ Token is NOT encrypted - payload is visible (base64 encoded)
    /// ⚠️ Never put sensitive data in claims (passwords, credit cards, etc.)
    /// ⚠️ Secret key must be strong (256+ bits) and kept secure
    /// ⚠️ Tokens can't be revoked until they expire (1 hour in this case)
    /// ⚠️ HTTPS required in production (prevents token interception)
    /// 
    /// CURRENT LIMITATIONS:
    /// - No refresh token mechanism (user must re-login after 1 hour)
    /// - Hardcoded 1 hour expiration (should be configurable)
    /// - Single role per user (can't handle multiple roles)
    /// - No token revocation before expiration
    /// - No "remember me" functionality
    /// 
    /// DEPENDENCIES:
    /// - System.IdentityModel.Tokens.Jwt (NuGet package)
    /// - Microsoft.IdentityModel.Tokens (NuGet package)
    /// - Configuration values in appsettings.json:
    ///   * Authentication:SecretForKey (base64 encoded, 256+ bits)
    ///   * Authentication:Issuer (your API URL)
    ///   * Authentication:Audience (your API URL or client app URL)
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();

            claimsForToken.Add(new Claim("sub", user.Id.ToString()));
            claimsForToken.Add(new Claim("sub_client", user.Client.Id.ToString()));
            claimsForToken.Add(new Claim("given_name", user.Client.FirstName));
            claimsForToken.Add(new Claim("family-name", user.Client.LastName));
            claimsForToken.Add(new Claim("role", user.Role));
            claimsForToken.Add(new Claim("email", user.Email));
            claimsForToken.Add(new Claim("phone", user.Client.Phone));

            var jwtSecurityToken = new JwtSecurityToken(_configuration["Authentication:Issuer"], _configuration["Authentication:Audience"], claimsForToken, DateTime.UtcNow, DateTime.UtcNow.AddHours(1), signingCredentials);

            var returnedToken= new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return returnedToken;
        }
    }
}
