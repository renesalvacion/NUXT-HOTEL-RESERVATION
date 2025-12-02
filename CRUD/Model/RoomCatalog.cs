using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CRUD.Model
{
    public class RoomCatalog
    {
        [Key]
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int MaxOccupancy { get; set; }
        public int RoomNumber {get; set;}
        public string RoomStatus { get; set; } = string.Empty;
        public string RoomType { get; set; }

        public string RoomImage { get; set; }
        public DateTime RoomDateCreated { get; set; } = DateTime.UtcNow;
        
        public ICollection<Review> Reviews { get; set; }

    }
}
