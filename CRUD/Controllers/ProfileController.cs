using CRUD.DTO;
using CRUD.Model;
using CRUD.Services;
using CRUD.Services.FileValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController(ICrudServices crud, IFileValidationServices fileValid) : ControllerBase
    {
        private readonly ICrudServices _crudServices = crud;
        private readonly IFileValidationServices _fileValidationServices = fileValid;


        [Authorize]
        [HttpGet("get-session")]
        public IActionResult GetSession()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized();

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            )?.Value;
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (email == null || userId == null)
                return Unauthorized();

            return Ok(new
            {
                email,
                role,  // now your frontend can use session.value.role
                userId
            });
        }


        [HttpPost("User-SignIn-Google")]
        public async Task<IActionResult> SignIn([FromBody] GoogleAuthRequest googleInfo)
        {
            var result = await _crudServices.GoogleSignIn(googleInfo);
            if(result.api == null) return BadRequest(new {message  = "null", result = result.api });

            return Ok(new { message = result.message, result = result.api });
        }

        [HttpPost("Google-Login-Google")]
        public async Task<IActionResult> LogIn([FromBody] GoogleAuthRequest googleInfo)
        {
            // Call the GoogleLogin service method and await the result
            var result = await _crudServices.GoogleLogin(googleInfo);

            // Check if login was successful based on the Success flag
            if (!result.Success)
            {
                // If login failed, return a BadRequest response with the message from the service
                return BadRequest(new { message = result.message, result = result.api });
            }

            // If login was successful, return a success response along with the user data, token
            return Ok(new { message = result.message, result = result.api, token = result.token });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Need to Fill Login Form" });

            var logs = await _crudServices.LoginServ(dto);

            if (!logs.Success)
                return BadRequest(new { message = logs.Message });

            

            return Ok(new
            {
                message = logs.Message, 
                token = logs.Token
            });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            HttpContext.Session.Remove("UserId");
            return Ok(new {message = "logout successfully"});
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ViewProfile(int userId)
        {
            var prof = await _crudServices.ViewProfile(userId);

            if (!prof.Success) return NotFound(new { message = "Profile Not Found" });

            return Ok(new { Success = true, profile = prof.User });
        }

        [HttpPost]
        public async Task<IActionResult> Crud([FromForm] CrudDto dto)
        {
           
            var valid = await _fileValidationServices.FileValidation(
                dto.Profile,
                new List<string> { ".jpg", ".jpeg", ".png" },
                "profile"
            );

            if(!valid.Success)
                return BadRequest(new {message = valid.Message});
            Console.WriteLine("VALID SUCCESS: " + valid.Success);
            Console.WriteLine("VALID MESSAGE: " + valid.Message);


            string uniqueFilename = valid.Message;
            var result = await _crudServices.PostCrud(dto, uniqueFilename);

            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);

            
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var result = await _crudServices.GetData();
            return Ok(result);
        }

    }
}
