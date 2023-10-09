
namespace Meal_Planner_Api.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly DataContext _authContext;
        private readonly IConfiguration _configuration;

        public JwtTokenService(DataContext authContext, IConfiguration configuration)
        {
            _authContext = authContext;
            _configuration = configuration;
        }

        //create jwt token on login
        public string CreateJwtToken(User user)
        {
            //get secret key from appsettings.json
            var secretkey = _configuration.GetSection("JwtSettings:SecretKey").Value;
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials

            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        //create refresh token
        public string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);


            var tokenInUser = _authContext.Users.Any(x => x.RefreshToken == refreshToken);

            if (tokenInUser)
            {
                return CreateRefreshToken();
            }

            return refreshToken;
        }


        //get principal from expired token
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var secretkey = _configuration.GetSection("JwtSettings:SecretKey").Value;
            var key = Encoding.ASCII.GetBytes(secretkey);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))

            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
