using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TiendaProducto.Models;

namespace TiendaProducto.Controllers
{

    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<mUsuario> gestionUsuarios;
        private readonly SignInManager<mUsuario> gestionLogin;
        private IConfiguration _configuration;

        public LoginController(UserManager<mUsuario> gestionUsuarios, SignInManager<mUsuario> gestionLogin, IConfiguration configuration)
        {
            _configuration = configuration;
            this.gestionUsuarios = gestionUsuarios;
            this.gestionLogin = gestionLogin;
        }

        [HttpPost]
        [Route("Login/Registro")]
        public async Task<ActionResult> Registro(mRegistro registro)
        {
            if (ModelState.IsValid)
            {
                var usuario = new mUsuario
                {
                    Nombre = registro.Nombre,
                    Apellido = registro.Apellido,
                    Email = registro.Email,
                    UserName = registro.Username
                    
                };

                var resultado = await gestionUsuarios.CreateAsync(usuario, registro.Password);
                if (resultado.Succeeded)
                {
                    await gestionLogin.SignInAsync(usuario, isPersistent: false);
                    var claims = new[]
                  {
                      new Claim(ClaimTypes.Email,registro.Email),
                      new Claim(ClaimTypes.NameIdentifier,usuario.Id)
                  };

                    var llave = _configuration["AuthSettings:Key"];
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                        );
                    string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(tokenAsString);
                }
                return BadRequest(resultado.Errors);
                
            }
            return BadRequest(registro);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(mLogin login)
        {
            if (ModelState.IsValid)
            {
                var user = await gestionUsuarios.FindByNameAsync(login.UserName);
                if (user == null)
                {
                    return BadRequest("El usuario no existe");
                }
                var pass = await gestionLogin.PasswordSignInAsync(user.UserName, login.Password,false,false);
                if (!pass.Succeeded)
                {
                    return BadRequest("Contraseña incorrecta");
                }
                  var claims = new[]
                  {
                      new Claim(ClaimTypes.Email,user.Email),
                      new Claim(ClaimTypes.NameIdentifier,user.Id)
                  };

                 var llave = _configuration["AuthSettings:Key"];
                 var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
                 var token = new JwtSecurityToken(                     
                     claims:claims,
                     expires:DateTime.Now.AddDays(30),
                     signingCredentials:new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
                     );
                 string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
                
                return Ok(tokenAsString);

            }
            return BadRequest("Datos invalidos");
        }
    }
}
