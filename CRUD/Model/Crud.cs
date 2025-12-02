using System.ComponentModel.DataAnnotations;
namespace CRUD.Model
{
    public class Crud
    {
        [Key]
        public int Id { get; set; }

        [Required]  
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }  
        
        public string Role { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string? Profile { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Review> Reviews { get; set; }    

    }
}
