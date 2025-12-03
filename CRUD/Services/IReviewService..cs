using CRUD.DTO;
using CRUD.Model;

namespace CRUD.Services
{
    public interface IReviewService
    {
        Task<(bool Success, string Message)> PostReview(ReviewDto dto,int userId);
        Task<List<Review>> ViewReview(int roomId);
        Task<AdminReviewDto> AdminViewReview();
        Task<List<Review>> ViewSpecificReview(int roomId);
    }
}
