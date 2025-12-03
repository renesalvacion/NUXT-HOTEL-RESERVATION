using CRUD.Model;

namespace CRUD.DTO
{
    public class AdminReviewDto
    {
        public List<Review>? Results { get; set; }
        public List<RoomAverageStarsDto>? RoomAverages { get; set; }  // average stars per room
    }
}
