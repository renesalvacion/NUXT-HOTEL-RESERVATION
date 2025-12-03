using CRUD.Data;
using CRUD.DTO;
using CRUD.Model;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class RoomCatalogServices : IRoomCatalogServices
    {
        private readonly CrudDbContext _context;

        public RoomCatalogServices(CrudDbContext context)
        {
            _context = context;
        }
        public async Task<(bool Success, string Message)> CreateRoomCatalog(RoomCatalogDto dto,string filename)
        {

            if (dto == null)
                return (false, "Dto is null");

            var user = await _context.Cruds.FirstOrDefaultAsync(u => u.Role == "Admin");

            // Check if user exists
            if (user == null)
                return (false, "User not found");

            // Check if role is Admin
            if (user.Role != "Admin")
                return (false, "Access denied. Only Admin can add rooms.");

            var room = new RoomCatalog
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                MaxOccupancy = dto.MaxOccupancy,
                RoomNumber  = dto.RoomNumber,
                RoomStatus = dto.RoomStatus,  
                RoomDateCreated = DateTime.UtcNow,
                RoomType = dto.RoomType,
                RoomImage = filename
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return (true, "Room Added Successfully");

        }

        public async Task<PaginatedRoomCatalog> GetCatalog(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var catalog = await _context.Rooms
                .OrderBy(r => r.RoomId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RoomCatalogItem
                {
                    Room = r,
                    AverageStars = _context.Reviews
                        .Where(review => review.RoomId == r.RoomId)
                        .Select(review => (double?)review.stars)
                        .Average() ?? 0
                })
                .ToListAsync();

            Console.WriteLine("Catalog Review: ", catalog);



            var totalCount = await _context.Rooms.CountAsync();

            return new PaginatedRoomCatalog
            {
                Rooms = catalog,
                TotalCount = totalCount
            };
        }



        public async Task<List<RoomCatalog>> GetDetailsRoom(int roomId)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == roomId);
            if (room == null) return new List<RoomCatalog>();
            return new List<RoomCatalog> { room };
        }

        //Admin Edit room
        public async Task<(bool Success, string Message)> AdminEditRoom(int roomId, RoomCatalogDto dto, string filename)
        {
            if (dto == null) return (false, "Need To Fillout the Whole Form");

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == roomId);

            if (room == null) return (false, "Room Id Is not Existing");

            room.Name = dto.Name;
            room.Description = dto.Description;
            room.Price = dto.Price;
            room.MaxOccupancy = dto.MaxOccupancy;
            room.RoomNumber = dto.RoomNumber;
            room.RoomStatus = dto.RoomStatus;
            room.RoomType = dto.RoomType;
            if (!string.IsNullOrWhiteSpace(filename))
            {
                room.RoomImage = filename;
            }

            await _context.SaveChangesAsync();

            return (true, "Update Successfully");
    }

    }
}
