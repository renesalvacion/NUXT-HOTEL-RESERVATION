using CRUD.DTO;
using CRUD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentServices _services;

        public AppointmentController(IAppointmentServices services)
        {
            _services = services;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> SetAppointment(AppointmentDto dto, int userId)
        {
            var appointment = await _services.SetAppointment(dto, userId);

            if (!appointment.Success)
                return BadRequest(new { message = appointment.Message });

            return Ok(new { message = appointment.Message });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAppointment(int userId)
        {
            var result = await _services.GetAppointment(userId);

            if (!result.Success)
                return NotFound(new { message = "No appointments found" });

            // Return a plain object instead of the tuple
            return Ok(new
            {
                success = result.Success,
                appointments = result.Appointments
            });
        }

        [HttpGet("view-appointment-admin")]
        public async Task<IActionResult> AdminViewAppointment(int pageNumber = 1, int pageSize = 8)
        {
            // Call the service to get both the data and the total count (this returns a tuple)
            var (appointments, totalCount) = await _services.ViewAppointmentAdmin(pageNumber, pageSize);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Ensure the pageNumber is not greater than totalPages
            if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
                // Re-fetch data with the corrected page number
                var (newAppointments, _) = await _services.ViewAppointmentAdmin(pageNumber, pageSize);
                appointments = newAppointments;
            }

            // Return the response with the paginated data
            return Ok(new
            {
                success = true,
                totalCount,
                pageNumber,
                pageSize,
                totalPages,
                appointments // This contains the List of appointments
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("view-appointment-admin/{userId}")]
        public async Task<IActionResult> AdminViewUserAppointment(int userId)
        {
            // Extract the token from Authorization header
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(new { message = "Authorization token is missing or malformed." });

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();

            try
            {
                // Read and validate JWT token
                var jwtToken = handler.ReadJwtToken(token);

                // Optionally, validate issuer, audience, and expiration here
                // If needed, perform additional token validation such as:
                // var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;
                // var expirationClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

                var role = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                )?.Value;

                if (role != "Admin")
                    return Unauthorized(new { message = "You do not have permission to view other users' appointments." });

                var result = await _services.AdminViewUserAppointment(userId);

                if (!result.success)
                    return NotFound(new { message = $"No appointments found for user {userId}." });

                return Ok(new { results = result.Data });
            }
            catch (Exception ex)
            {
                // Log the error here
                return BadRequest(new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }


        [HttpPost("Admin-Update-Status-Appointment/{appointmentId}")]
        public async Task<IActionResult> UpdateStatus(int appointmentId, UpdateStatusDto dto)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin") return Unauthorized(new { success = false, message = "Your Not Authorize To Update The Status" });

            var result = await _services.AdminCancelAppointment(appointmentId, dto);

            if (!result.Success) return BadRequest(new { success = result.Success, message = result.Message });

            return Ok(new { success = result.Success, message = result.Message});
        }

        [HttpPost("Admin-Update-Reservation/{adminId}")]
        public async Task<IActionResult> UpdateReservation(AdminEditReservation dto, int adminId)
        {
            var result = await _services.AdminEditAppointment(dto, adminId);

            if (!result.Success) return BadRequest (new { success = result.Success, message = result.Message });

            return Ok(new { success = result.Success, message = result.Message });
        }


        [HttpPost("UserUpdateReservation/{userId}")]
        public async Task<IActionResult> UpdateReservationDate(UpdateUserAppointmentDto dto, int userId)
        {
            var result = await _services.EditUserAppointment(dto, userId);

            if (!result.Success) return BadRequest(new { success = result.Success, message = result.Message });

            return Ok(new { success = result.Success, message = result.Message });
        }

        [HttpPost("UserCancelReservation/{userId}")]
        public async Task<IActionResult> CancelReservation(CancelAppointmentDto dto,int userId)
        {
            var result = await _services.CancelReservation(dto, userId);

            if(!result.Success) return BadRequest(new { success = result.Success, message = result.Message });

            return Ok(new { success = result.Success, message = result.Message });
        }

    }
}
