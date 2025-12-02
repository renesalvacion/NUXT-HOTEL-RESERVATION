using CRUD.DTO;
using CRUD.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [Route("review/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult>PostReview(ReviewDto dto,int userId)
        {
            var review = await _reviewService.PostReview(dto, userId);

            if (!review.Success) return BadRequest(new { message = review.Message });

            return Ok(new { message = review.Message });
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult>ViewRoomReview(int roomId)
        {
            var result = await _reviewService.ViewReview(roomId);

            return Ok(result);
        }
    }

}
