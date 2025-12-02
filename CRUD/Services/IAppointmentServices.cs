using CRUD.DTO;
using CRUD.Model;

namespace CRUD.Services
{
    public interface IAppointmentServices
    {
        Task<(bool Success, string Message)> SetAppointment(AppointmentDto dto, int userId);
        Task<(bool Success, List<Appointment> Appointments)> GetAppointment(int userId);

        Task<(List<ViewAppointmentDtoAdmin> Data, int TotalCount)> ViewAppointmentAdmin(int pageNumber, int bvpageSize);
        Task<(List<ViewAppointmentDtoAdmin> Data, bool success)> AdminViewUserAppointment(int userId);
        Task<(bool Success, string Message)> AdminCancelAppointment(int appointmentId, UpdateStatusDto dto);
        Task<(bool Success, string Message)> AdminEditAppointment(AdminEditReservation dto, int adminId);


        Task<(bool Success, string Message)> EditUserAppointment(UpdateUserAppointmentDto dto, int userId);

        Task<(bool Success, string Message)> CancelReservation(CancelAppointmentDto dto, int userId);
    }
}
