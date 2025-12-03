namespace CRUD.Services.FileValidation
{
    public interface IFileValidationServices
    {
        Task<(bool Success, string Message)> FileValidation(IFormFile file, List<string> AllowedExtension, string FolderName);
        
    }
}
