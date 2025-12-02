using System.ComponentModel.DataAnnotations;

namespace CRUD.DTO
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }  
        public string RoomType { get; set; }

        public int NumberBed { get; set; }
        public int HeadPerson { get; set; }
        public int Price { get; set; }
        public string status { get; set; }
        // Reservation Date - Make sure it's a valid future date
        [Required(ErrorMessage = "Reservation date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime ReservationDate { get; set; }

    }
}
