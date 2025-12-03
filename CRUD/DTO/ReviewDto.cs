namespace CRUD.DTO
{
    public class ReviewDto
    {
        public int stars { get; set; }

        public string? ReviewMessage { get; set; }
        public string? ReviewTitle { get; set; }

        // Foreign key to Room
        public int RoomId { get; set; }
    }
}
