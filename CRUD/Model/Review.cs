using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CRUD.Model
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public int stars { get; set; }

        public string ReviewMessage { get; set; }
        public string ReviewTitle { get; set; }
        public DateTime ReviewCreated { get; set; } = DateTime.UtcNow;

        [ForeignKey("RoomId")]
        // Foreign key to Room\
        public int RoomId { get; set; }

        [JsonIgnore]
        public RoomCatalog Room { get; set; }

        // Foreign key to User
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Crud Users { get; set; } // Navigation property
    }
}
