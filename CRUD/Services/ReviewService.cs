using CRUD.Data;
using CRUD.DTO;
using CRUD.Model;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class ReviewService : IReviewService
    {
        private readonly CrudDbContext _dbContext;

        public ReviewService(CrudDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<(bool Success, string Message)> PostReview(ReviewDto dto, int userId)
        {
            if (dto == null) return (false, "Dto is null");

            var user = await _dbContext.Cruds.FirstOrDefaultAsync(u => u.Id  == userId);

            if (user == null) return (false, "Cant Find Your User Id");

            var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.RoomId == dto.RoomId);
            if (room == null) return (false, "Cant Find Room Id");

            var review = new Review
            {
                stars = dto.stars,
                ReviewMessage = dto.ReviewMessage,
                ReviewTitle = dto.ReviewTitle,
                ReviewCreated = DateTime.UtcNow,
                RoomId = dto.RoomId,
                UserId = userId,
            };

            try
            {
                await _dbContext.AddAsync(review);

                await _dbContext.SaveChangesAsync();

                return (true, "Review Successfully Added");
            }
            catch (Exception ex) 
            {
                return (false, $"Error: {ex.Message}");
            }
        }

            public async Task<List<Review>> ViewReview(int roomId)
            {
                // Find the room by its ID (Make sure it exists)
                var room = await _dbContext.Rooms
                    .Include(r => r.Reviews) // Include the related reviews (assuming you have a navigation property)
                    .FirstOrDefaultAsync(r => r.RoomId == roomId);

                Console.WriteLine("room reviews: ", room);

                if (room == null)
                {
                    // If the room doesn't exist, return an empty list
                    return new List<Review>();
                }

            
                // Return the reviews for the room
                return room.Reviews.ToList();
            }
            public async Task<List<Review>> ViewSpecificReview(int roomId)
            {
                // Find the room by its ID (Make sure it exists)
                var review = await _dbContext.Reviews.Where(r => r.RoomId == roomId).ToListAsync();

                return review;
            }

            public async Task<AdminReviewDto> AdminViewReview()
            {
                // Group by roomId and pick first review for each group
                var results = await _dbContext.Reviews
                    .GroupBy(r => r.RoomId)
                    .Select(g => new Review
                    {
                        ReviewId = g.First().ReviewId,
                        ReviewTitle = g.First().ReviewTitle,
                        ReviewMessage = g.First().ReviewMessage,
                        stars = g.First().stars,
                        ReviewCreated = g.First().ReviewCreated,
                        RoomId = g.Key,
                        Room = g.First().Room,

                    })
                    .ToListAsync();

                // Average stars per room
                var roomAverages = await _dbContext.Reviews
                    .GroupBy(r => r.RoomId)
                    .Select(g => new RoomAverageStarsDto
                    {
                        RoomId = g.Key,
                        AverageStars = g.Average(r => r.stars)
                    })
                    .ToListAsync();

                return new AdminReviewDto
                {
                    Results = results,
                    RoomAverages = roomAverages
                };
            }
        }

    }
