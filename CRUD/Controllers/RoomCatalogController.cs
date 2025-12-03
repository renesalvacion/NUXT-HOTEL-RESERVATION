using CRUD.DTO;
using CRUD.Services;
using CRUD.Services.FileValidation;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRUD.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class RoomCatalogController : ControllerBase
    {
        private readonly IRoomCatalogServices _services;
        private readonly IFileValidationServices _fileValidationServices;

        public RoomCatalogController(IRoomCatalogServices services, IFileValidationServices fileValidationServices)
        {
            _services = services;
            _fileValidationServices = fileValidationServices;
        }

        [Authorize]
        [HttpPost("room-catalog")]
        public async Task<IActionResult>AddRoomCatalog(RoomCatalogDto dto)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized();

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var role = jwtToken.Claims.FirstOrDefault(c =>
               c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
           )?.Value;

            if (role != "Admin")
                return Unauthorized(new { message = "Only admin can access this page" });

            var valid = await _fileValidationServices.FileValidation(
                dto.RoomImage,
                new List<string> { ".jpg", ".jpeg", ".png", ".jfif" },
                "RoomImage"
            );

            if(!valid.Success)
                return BadRequest(new {message =  valid.Message});

            string uniqueFilename = valid.Message;
            var result = await _services.CreateRoomCatalog(dto, uniqueFilename);

            if(!result.Success)
                return BadRequest(new {message =  result.Message});

            return Ok(new {message = result.Message});
        }


        [HttpGet]
        public async Task<IActionResult> GetCatalog(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _services.GetCatalog(pageNumber, pageSize);

            if (result == null || result.Rooms == null || !result.Rooms.Any())
                return NotFound(new { message = "result not found" });

            return Ok(new
            {
                success = true,
                rooms = result.Rooms,
                totalCount = result.TotalCount
            });
        }


        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetDetailsRoom(int roomId)
        {
            var details = await _services.GetDetailsRoom(roomId);

            if (details == null) return NotFound(new { message = "Room Not Found" });

            return Ok(new { success = true, room = details });
        }

        [HttpPut("Update-Room/{roomId}")]
        public async Task<IActionResult> UpdateRoomDetails(int roomId, RoomCatalogDto dto)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized();

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var role = jwtToken.Claims.FirstOrDefault(c =>
               c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
           )?.Value;

            if (role != "Admin")
                return Unauthorized(new { message = "Only admin can access this page" });

            var valid = await _fileValidationServices.FileValidation(
                dto.RoomImage,
                new List<string> { ".jpg", ".jpeg", ".png", ".jfif" },
                "RoomImage"
            );

            if (!valid.Success)
                return BadRequest(new { message = valid.Message });

            string uniqueFilename = valid.Message;

            var result = await _services.AdminEditRoom(roomId,dto, uniqueFilename);
            if(!result.Success) return BadRequest(new { message = result.Message, success = result.Success } );

            return Ok(new { message = result.Message, success = result.Success } );

        }
    }
}
