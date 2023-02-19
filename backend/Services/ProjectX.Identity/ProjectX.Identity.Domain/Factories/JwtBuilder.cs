using ProjectX.Core;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace ProjectX.Identity.Domain;

public partial class JwtToken
{
    public sealed class JwtBuilder
    {
        #region Private Fields
        
        private Guid? _jwtId;

        private string? _secret;

        private TimeSpan? _expiryTimeFrame;

        private DateTime? _issuedAt;

        private string? _issuer;

        private IEnumerable<string>? _audience;

        private AccountEntity? _account;

        #endregion

        #region Setup Methods

        public JwtBuilder AddJwtId(Guid jwtId)
        {
            _jwtId = jwtId;
            return this;
        }

        public JwtBuilder AddSecret(string secret)
        {
            _secret = secret;
            return this;
        }

        public JwtBuilder AddExpiryTimeFrame(TimeSpan expiryTimeFrame)
        {
            _expiryTimeFrame = expiryTimeFrame;
            return this;
        }

        public JwtBuilder AddIssuedAt(DateTime issuedAt)
        {
            _issuedAt = issuedAt;
            return this;
        }

        public JwtBuilder AddAccount(AccountEntity account)
        {
            _account = account;
            return this;
        }

        public JwtBuilder AddIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        public JwtBuilder AddAudiance(params string[] audience)
        {
            _audience = audience;
            return this;
        }

        #endregion

        public JwtToken Build()
        {
            _expiryTimeFrame.ThrowIfNull();
            _secret.ThrowIfNull();
            _issuer.ThrowIfNull();
            _account.ThrowIfNull();
            _audience.ThrowIfNull();

            var issuedAt = _issuedAt ?? DateTime.UtcNow;
            var credentials = GetCredentials();
            var expiresAt = issuedAt.Add(_expiryTimeFrame.Value);
            var jwtId = (_jwtId ?? Guid.NewGuid()).ToString();

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Sub, _account.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, _account.Email!),
                new(JwtRegisteredClaimNames.Iat, issuedAt.ToUniversalTime().ToString()),
                new(JwtRegisteredClaimNames.Jti, jwtId) // used by the refresh token
            };

            claims.AddRange(_audience.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _issuer!,
                //Audience = _jwtConfig.Audience,
                Expires = expiresAt,
                SigningCredentials = credentials,
                Subject = new ClaimsIdentity(claims
                //,JwtBearerDefaults.AuthenticationScheme //TODO: test it
                )
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtHandler.WriteToken(token);
            var refreshToken = RefreshToken.Create(_account!, jwtId, issuedAt);
            
            Clear();
            
            return new JwtToken(jwtId, jwtToken, issuedAt, refreshToken); ;
        }

        private SigningCredentials GetCredentials()
        {
            /*
             * At Auth0 we allow signing of tokens using either a symmetric algorithm (HS256), 
             * or an asymmetric algorithm (RS256).RS256: 
             * <see href="https://www.jerriepelser.com/blog/manually-validating-rs256-jwt-dotnet/"/>
             * <see href="https://developer.okta.com/code/dotnet/jwt-validation/"/>
             * HS256 tokens are signed and verified using a simple secret, 
             * where as RS256 use a private and public key for signing and verifying the token signatures.
             * SHA-256 it's Hashing function. This means that if we take our Header and Payload and run it through this function,
             * no one will be able to get the data back again just by looking at the output.
             * Hashing is not encryption: encryption by definition is a reversible action - we do need to get back the original input from the encrypted output.
            */
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret!)),
                SecurityAlgorithms.HmacSha256);

            return credentials;
        }

        private void Clear()
        {
            _jwtId = null;
            _secret = null;
            _expiryTimeFrame = null;
            _issuedAt = null;
            _issuer = null;
            _audience = null;
            _account = null;
        }
    }
}