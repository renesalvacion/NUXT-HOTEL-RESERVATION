using CRUD.Data;
using Microsoft.EntityFrameworkCore;
using CRUD.Model;
using CRUD.DTO;
namespace CRUD.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        
        private readonly CrudDbContext _user;
       
        public AppointmentServices(CrudDbContext user) 
        {
            _user = user;
           
        }   

        public async Task<(bool Success, string Message)> SetAppointment(AppointmentDto dto, int userId)
        {
            if (dto == null)
                return (false, "Need to Fillup the Form");
            var customer = await _user.Cruds.FirstOrDefaultAsync(u => u.Id == userId);

            if (customer == null)
                return (false, "You Dont have Access on reservation");

            var appointment = new Appointment
            {
                RoomType = dto.RoomType,
                NumberBed = dto.NumberBed,  
                Price = dto.Price,
                HeadPerson  = dto.HeadPerson,
                CrudId = userId,
                Created = DateTime.UtcNow,
            };

            _user.Add(appointment);
            await _user.SaveChangesAsync();

            return (true, "Set Appointment Successfuly");

        }
        public async Task<(bool Success, List<Appointment> Appointments)> GetAppointment(int userId)
        {
   

            var appointments = await _user.Appointments
                .Where(a => a.CrudId == userId)
                .ToListAsync();

            return (true, appointments);
        }

        public async Task<(List<ViewAppointmentDtoAdmin> Data, int TotalCount)> ViewAppointmentAdmin(
            int pageNumber, int pageSize)
        {
            // Validate pageNumber and pageSize
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10; // Limit page size to 100

            // Get total count BEFORE pagination
            var totalCount = await _user.Appointments.CountAsync();

            var grouped = await _user.Appointments
                .Include(a => a.Cruds)
                .GroupBy(a => new { a.CrudId, a.Cruds.Name })
                .Select(g => new
                {
                    g.Key.CrudId,   
                    g.Key.Name,
                    ReservationCount = g.Count(),
                    Latest = g.OrderByDescending(x => x.Created).FirstOrDefault()
                })
                    .OrderBy(x => x.CrudId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            // second projection (in-memory)
            var appointments = grouped.Select(x => new ViewAppointmentDtoAdmin
            {
                ReservationCount = x.ReservationCount,
                AppointmentId = x.Latest.AppointmentId,
                RoomType = x.Latest.RoomType,
                NumberBed = x.Latest.NumberBed,
                HeadPerson = x.Latest.HeadPerson,
                Price = x.Latest.Price,
                Status = x.Latest.Status,
                Created = x.Latest.Created,
                CrudsName = x.Name,
                CrudId = x.CrudId
            }).ToList();

            return (appointments, totalCount);
        }

        public async Task<(List<ViewAppointmentDtoAdmin> Data, bool success)> AdminViewUserAppointment(int userId)
        {
            var count = await _user.Appointments.CountAsync(a => a.CrudId == userId);
            Console.WriteLine($"Found {count} appointments for userId {userId}");
            // Get all appointments for the user
            var appointments = await _user.Appointments
                .Where(a => a.CrudId == userId)
                .Include(a => a.Cruds)
                .OrderByDescending(a => a.Created)
                .Select(a => new ViewAppointmentDtoAdmin
                {
                    AppointmentId = a.AppointmentId,
                    RoomType = a.RoomType,
                    NumberBed = a.NumberBed,
                    HeadPerson = a.HeadPerson,
                    Price = a.Price,
                    Status = a.Status,
                    Created = a.Created,
                    CrudsName = a.Cruds.Name,
                    CrudId = a.CrudId
                })
                .ToListAsync();

            if (appointments.Count == 0)
                return (new List<ViewAppointmentDtoAdmin>(), false);

            return (appointments, true);
        }

        public async Task<(bool Success, string Message)> AdminCancelAppointment(int appointmentId, UpdateStatusDto dto)
        {
            var appointment = await _user.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null) return (false, "Cant Find The Appointment Id");

            appointment.Status = dto.Status;
            await _user.SaveChangesAsync();

            return (true, "Update Status Successfuly");
        }


        public async Task<(bool Success, string Message)> EditUserAppointment(UpdateUserAppointmentDto dto, int userId)
        {
            var appointment = await _user.Appointments.FirstOrDefaultAsync(a => a.CrudId == userId && a.AppointmentId == dto.AppointmentId);

            if (appointment == null) return (false, "You dont have Existing Appointment");

            // Update the appointment details with the provided DTO values
            appointment.ReservationDate = dto.ReservationDate; // Example of updating appointment date
            appointment.AppointmentId = dto.AppointmentId;
            appointment.RoomType = dto.RoomType;
            appointment.NumberBed = dto.NumberBed;
            appointment.HeadPerson = dto.HeadPerson;

            //appointment.Description = dto.Description; // Update the description or other fields

            // Save the changes to the database
            try
            {
                await _user.SaveChangesAsync();
                return (true, "Appointment updated successfully.");
            }
            catch (Exception ex)
            {
                // If any errors occur while saving, return failure
                return (false, $"An error occurred while updating the appointment: {ex.Message}");
            }

        }

        public async Task<(bool Success, string Message)> CancelReservation(CancelAppointmentDto dto, int userId)
        {
            var appoiment = await _user.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId  && a.CrudId == userId);

            if (appoiment == null) return (false, "You dont have existing appointment");

            

            appoiment.Status = "Cancel";

            try
            {
                await _user.SaveChangesAsync();
                return (true, "Appoint Succ{" +
                    "essfuly Cancel");
            }
            catch (Exception ex)
            {
                // If any errors occur while saving, return failure
                return (false, $"An error occurred while updating the appointment: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> AdminEditAppointment(AdminEditReservation dto, int adminId)
        {
            var user = await _user.Cruds.FirstOrDefaultAsync(u => u.Id == adminId && u.Role == "Admin");

            if (user == null) return (false, "Your Authorized To Edit the Customer Reservation");

            var appId = await _user.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == dto.ReservationId);
            if (appId == null) return (false, "The Reservation Id That You Edit Is Not Existing");

            appId.AppointmentId = dto.ReservationId;
            appId.RoomType = dto.RoomType;
            appId.HeadPerson = dto.HeadPerPerson;

            try
            {
                await _user.SaveChangesAsync();
                return (true, "Appointment Update By Admin Successfully");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while updating the appointment: {ex.Message}");
            }

        }

    }
}
