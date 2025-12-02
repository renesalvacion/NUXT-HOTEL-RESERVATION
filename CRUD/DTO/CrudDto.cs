namespace CRUD.DTO
{
    public class CrudDto
    {
       
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public IFormFile? Profile { get; set; }
    }
}

