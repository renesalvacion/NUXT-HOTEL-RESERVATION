using System.ComponentModel.DataAnnotations;

namespace CRUD.DTO
{
    public class UpdateUserAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string? RoomType { get; set; }

        public int NumberBed { get; set; }
        public int HeadPerson { get; set; }
        // Reservation Date - Make sure it's a valid future date
        [Required(ErrorMessage = "Reservation date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime ReservationDate { get; set; }
    }
}
