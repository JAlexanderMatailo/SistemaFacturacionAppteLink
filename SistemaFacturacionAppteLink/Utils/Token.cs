using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SisCazhapugroBack.Utilidades
{
    public static class Token
    {
        public static string key = "1uCpfkVEM7F7PMJ1ZQ5S1duRbf8osyTNQxIkt1T5KI=";
        public static string GenerarToken(string nameUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var TokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, nameUser),
                }),
                Expires = ObtenerTiempoExpiración(),
                SigningCredentials = ObtenerCredencialesFirma()
            };
            var token = tokenHandler.CreateToken(TokenDes);
            return tokenHandler.WriteToken(token);
        }
        private static DateTime ObtenerTiempoExpiración()
        {
            return DateTime.UtcNow.AddMonths(1);
        }
        private static SigningCredentials ObtenerCredencialesFirma()
        {
            return new SigningCredentials(ObtenerClaveFirma(), ObtenerAlgoritmoDeSeguridad());
        }
        public static void AddJwtAuthentication(IServiceCollection services)
        {
            SymmetricSecurityKey signingKey = ObtenerClaveFirma();

            services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = signingKey,
                };
            });
        }
        private static SymmetricSecurityKey ObtenerClaveFirma()
        {
            return new SymmetricSecurityKey(ConvertirCadenaABytes());
        }

        private static string ObtenerAlgoritmoDeSeguridad()
        {
            return SecurityAlgorithms.HmacSha256Signature;
        }

        private static byte[] ConvertirCadenaABytes()
        {
            return Encoding.UTF8.GetBytes(key);
        }
    }
}
