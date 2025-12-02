using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Model
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public string? RoomType { get; set; }
        [Required]
        public int NumberBed { get; set; }
        public int HeadPerson { get; set; }
        public int CrudId { get; set; }
        public int Price { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime ReservationDate { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        
        public Crud Cruds { get; set; } = null;

    }
}
