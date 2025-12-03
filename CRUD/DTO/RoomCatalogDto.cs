namespace CRUD.DTO
{
    public class RoomCatalogDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public int MaxOccupancy { get; set; }
        public int RoomNumber { get; set; }
        public string RoomStatus { get; set; } = string.Empty;
        public string? RoomType { get; set; }
        public IFormFile? RoomImage { get; set; }
    }
}
