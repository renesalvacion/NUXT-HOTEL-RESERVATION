using CRUD.DTO;
using CRUD.Model;

namespace CRUD.Services
{
    public interface IRoomCatalogServices
    {
        Task<(bool Success, string Message)> CreateRoomCatalog(RoomCatalogDto dtoz, string filename);
        Task<PaginatedRoomCatalog> GetCatalog(int pageNumber = 1, int pageSize = 10);

        Task<List<RoomCatalog>>GetDetailsRoom(int roomId);

        //Admin Rights
        Task<(bool Success, string Message)> AdminEditRoom(int roomId, RoomCatalogDto dto, string filename);
    }
}
