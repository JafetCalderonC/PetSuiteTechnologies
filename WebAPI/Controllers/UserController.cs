using CoreApp;
using CoreApp.Others;
using DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly PasswordOptions _passwordOptions;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _passwordOptions = new PasswordOptions
            {
                LowerCase = "abcdefghijklmnopqrstuvwxyz",
                UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                Numbers = "0123456789",
                SpecialCharacters = "!@#$%^&*()_-+=[{]};:<>|./?",
                MinLowerCase = 1,
                MinUpperCase = 1,
                MinNumbers = 1,
                MinSpecialCharacters = 1,
                MinPasswordLength = 8
            };
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login(Login login)
        {
            try
            {
                var manager = new UserManager(_passwordOptions);
                var userId = manager.Login(login);

                var jwt = _configuration.GetSection("Jwt");
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"])), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(tokenConfig);

                return Ok(new { token, userId });
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpPut]
        [Route("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(ForgotPassword forgot)
        {
            try
            {
                var manager = new UserManager(_passwordOptions);
                manager.ForgotPassword(forgot);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpPost]
        [Route("SignUp")]
        [AllowAnonymous]
        public IActionResult SignUp(User signUp)
        {
            try
            {
                var manager = new UserManager(_passwordOptions);
                manager.Create(signUp);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpPut]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePassword change)
        {
            try
            {
                // Get the user id from the token
                var userAuthId = int.Parse((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var manager = new UserManager(userAuthId, _passwordOptions);
                manager.ChangePassword(change);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpGet]
        [Route("IsLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            try
            {
                // Get the user id from the token
                var userAuthId = int.Parse((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // get the user from the database
                var manager = new UserManager(userAuthId, _passwordOptions);
                var user = manager.RetrieveById(userAuthId);
                if (user == null)
                {
                    return Ok(false);
                }

                return Ok(true);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(User create)
        {
            try
            {
                // Get the user id from the token
                var userAuthId = int.Parse((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var manager = new UserManager(userAuthId, _passwordOptions);
                manager.Create(create);

                return Ok(create);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(User update)
        {
            try
            {
                // Get the user id from the token
                var userAuthId = int.Parse((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var manager = new UserManager(userAuthId, _passwordOptions);
                manager.Update(update);

                return Ok(update);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                // Get the user id from the token
                var userAuthId = int.Parse((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var manager = new UserManager(userAuthId, _passwordOptions);
                manager.Delete(id);

                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpGet]
        [Route("RetrieveById")]
        public IActionResult RetrieveById(int id)
        {
            try
            {
                // Get the user id from the token
                var userAuthId = int.Parse((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var manager = new UserManager(userAuthId, _passwordOptions);
                var user = manager.RetrieveById(id);

                return Ok(user);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpGet]
        [Route("RetrieveAll")]
        public IActionResult RetrieveAll()
        {
            try
            {
                // Get the user id from the token
                var userAuthId = int.Parse((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var manager = new UserManager(userAuthId, _passwordOptions);
                var users = manager.RetrieveAll();

                return Ok(users);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }
    }
}