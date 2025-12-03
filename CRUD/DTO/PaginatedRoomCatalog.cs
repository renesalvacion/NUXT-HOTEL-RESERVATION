using CRUD.Model;

namespace CRUD.DTO
{
    public class PaginatedRoomCatalog
    {
        public List<RoomCatalogItem>? Rooms { get; set; }
        public int TotalCount { get; set; }
    }
}
