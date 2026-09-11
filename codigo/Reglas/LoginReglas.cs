using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public class LoginReglas : ILoginReglas
    {
        private readonly ILoginServicio _loginServicio;

        public LoginReglas(ILoginServicio loginServicio)
        {
            _loginServicio = loginServicio;
        }

        public  Task<LoginResponse> IniciarSesion(LoginRequest credenciales) => _loginServicio.IniciarSesion(credenciales);
        
    }
}
