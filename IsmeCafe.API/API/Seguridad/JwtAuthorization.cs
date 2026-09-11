using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Abstracciones.Interfaces.Seguridad;
using Microsoft.IdentityModel.Tokens;

namespace API.Seguridad
{
    public class JwtAuthorization : IJwtAuthorization
    {
        private readonly IConfiguration _configuration;

        public JwtAuthorization(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Guid idUsuario,string nombreCompleto,string correo,string nombreRol)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var duracionMinutos = _configuration.GetValue<int>("Jwt:DuracionMinutos");

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("La llave JWT no se encuentra configurada.");

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,idUsuario.ToString()),
                new(ClaimTypes.NameIdentifier,idUsuario.ToString()),
                new(ClaimTypes.Name,nombreCompleto),
                new(ClaimTypes.Email,correo),
                new(ClaimTypes.Role,nombreRol),
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credenciales = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(duracionMinutos),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}