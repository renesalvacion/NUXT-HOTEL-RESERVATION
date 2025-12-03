namespace CRUD.DTO
{
    public class ViewAppointmentDtoAdmin
    {
        public int CrudId { get; set; }
        public int ReservationCount { get; set; }
        public int AppointmentId { get; set; }
        public string? RoomType { get; set; }
        public int NumberBed { get; set; }
        public int HeadPerson { get; set; }
        public int Price { get; set; }
        public string? Status { get; set; }
        public DateTime Created { get; set; }
        public string? CrudsName { get; set; }
    }
}
