namespace CRUD.Services.FileValidation
{
    public class FileValidationServices : IFileValidationServices
    {
       public async Task<(bool Success, string Message)> 
            FileValidation(IFormFile file, List<string> AllowedExtension, string FolderName)
        {


            if (file == null)
                return (false, "file is not");
            if(file.Length == 0) return (false, "file is empty");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtension.Contains(fileExtension))
                return (false, "The filepath extension doesnt allow to upload, pls choose a file extension base from the options");

            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            else
            {
                // Log or output a message if the directory already exists
                Console.WriteLine($"Directory {uploadFolder} already exists.");
            }


            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return (true, uniqueFileName);


        }
        
    }
}
