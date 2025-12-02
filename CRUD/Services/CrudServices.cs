using CRUD.Data;
using CRUD.DTO;
using CRUD.Model;
using CRUD.Services.FileValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CRUD.Services
{
    public class CrudServices : ICrudServices
    {
        private readonly CrudDbContext _context;
        private readonly IFileValidationServices _fileValidation;
        private readonly PasswordHasher<Crud> _passwordHasher = new();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CrudServices> _logger;
        private readonly JwtService _jwt;

        public CrudServices(CrudDbContext context, IFileValidationServices fileValidation, PasswordHasher<Crud> passwordHasher, 
            IHttpContextAccessor httpContextAccessor, ILogger<CrudServices> logger, JwtService jwt)
        {
            _context = context;
            _fileValidation = fileValidation;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger;
            _jwt = jwt;
        }

        public async Task<(bool Success, Crud? User)> ViewProfile(int userId)
        {
            var user = await _context.Cruds.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return (false, null);
            return (true, user);
        }



        public async Task<(bool Success, string Message, string Token)> LoginServ(LoginDto dto)
        {
            
            if (dto == null)
                return (false, "Need to fill the form", null);

            var user = await _context.Cruds.FirstOrDefaultAsync( u => u.Email.ToLower() == dto.Email.ToLower());

            if (user == null)
                return (false, "Invalid Emaill", null);

            if (user.Password == null)
                return (false, "", null);
            var verifyPass = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if (verifyPass == PasswordVerificationResult.Failed)
                return (false, "Incorrect password", null);

            if (_httpContextAccessor.HttpContext == null)
            {
                return (false, "Unable to access session.", null);
            }

            // 2. Generate JWT token
            string token = _jwt.GenerateToken(user.Id.ToString(), user.Email, user.Role);

                _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id.ToString());
                _httpContextAccessor.HttpContext.Session.SetString("Username", user.Name ?? ""); // store name safely
                _httpContextAccessor.HttpContext.Session.SetString("Role", user.Role ?? "");


            return (true, "Login Successfully", token);
        }

        public async Task<(bool Success, string Message)> PostCrud(CrudDto dto, string fileName)
        {
           
            
         

            // Ensure password is not null or empty
            if (string.IsNullOrEmpty(dto.Password))
            {
                return (false, "Password cannot be null or empty.");
            }



            var crud = new Crud
            {
                Name = dto.Name,
                Email = dto.Email,
                Created = DateTime.UtcNow,
                // Set the Profile field to the unique file name (not the IFormFile object)
                Profile = fileName
            };


            var hashPass = _passwordHasher.HashPassword(crud, dto.Password);
            crud.Password = hashPass;

            _context.Cruds.Add(crud);
            
            await _context.SaveChangesAsync();

            return (true, "Success");
        }

       
        public async Task<IEnumerable<Crud>> GetData()
        {
            try
            {
                var result =  await _context.Cruds.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception (using your preferred logging mechanism)
                _logger.LogError(ex, "Error occurred while fetching data from the database.");

                // Return an empty list to indicate failure
                return new List<Crud>();
            }


        }


        public async Task<(string message, List<GoogleUserInfo> api)> GoogleSignIn([FromBody] GoogleAuthRequest googleAuth)
        {
            var googleIdToken = googleAuth.IdToken;

            // Step 1: Verify the ID Token with Google
            var client = new HttpClient();
            var requestUri = $"https://oauth2.googleapis.com/tokeninfo?id_token={googleIdToken}";
            var response = await client.GetStringAsync(requestUri);

            // Step 2: Handle possible errors from Google (if response contains error)
            if (response.Contains("error"))
            {
                // Return an empty list if the token verification failed
                return ("Google token verification failed.", new List<GoogleUserInfo>());
            }

            // Step 3: Parse the response to get user info (email, name, etc.)
            var googleUser = JsonConvert.DeserializeObject<GoogleUserInfo>(response);

            // Ensure the email is returned from Google and is not null
            if (string.IsNullOrEmpty(googleUser?.Email))
            {
                // Return an empty list if the email retrieval failed
                return ("Failed to retrieve email from Google.", new List<GoogleUserInfo>());
            }

            // Step 4: Check if user already exists in the database
            var user = await _context.Cruds
                .FirstOrDefaultAsync(u => u.Email == googleUser.Email);

            // Generate a random password
            string randomPassword = GenerateRandomPassword();

            // Hash the generated password
            var hashedPassword = _passwordHasher.HashPassword(new Crud(), randomPassword);

            if (user == null)
            {
                // Step 5: If new user, create a new record in the database
                user = new Crud
                {
                    Email = googleUser.Email,
                    Name = googleUser.Name,
                    Profile = googleUser.Picture,
                    Password = hashedPassword // Store the hashed password
                };

               
                _context.Cruds.Add(user);
                await _context.SaveChangesAsync();
            }

            // Step 6: Return a success response (or user info, token, etc.)
            // If you want to return a list, you can return the user as a single-item list
            var api = new List<GoogleUserInfo> { googleUser };
            return ("Successfully signed in.", api);
        }


        public async Task<(string message, bool Success, List<GoogleUserInfo> api, string token)> GoogleLogin([FromBody] GoogleAuthRequest googleAuth)
        {
            var googleIdToken = googleAuth.IdToken;

            // Step 1: Verify the ID Token with Google
            var client = new HttpClient();
            var requestUri = $"https://oauth2.googleapis.com/tokeninfo?id_token={googleIdToken}";

            try
            {
                // Get the response from Google's token verification endpoint
                var response = await client.GetStringAsync(requestUri);

                // Step 2: Parse the response to get user info (email, name, etc.)
                var googleUser = JsonConvert.DeserializeObject<GoogleUserInfo>(response);

                // Ensure the email is returned from Google and is not null
                if (string.IsNullOrEmpty(googleUser?.Email))
                {
                    return ("Failed to retrieve email from Google.", false, new List<GoogleUserInfo>(), null);
                }

                // Step 3: Check if the user already exists in the database
                var user = await _context.Cruds
                    .FirstOrDefaultAsync(u => u.Email == googleUser.Email);

                if (user == null)
                {
                    // If user doesn't exist, return an error message
                    return ("User not found, please sign up first.", false, new List<GoogleUserInfo>(), null);
                }

                // Step 4: If user exists, generate JWT token
                var token = _jwt.GenerateToken(user.Id.ToString(), user.Email, user.Role); // Using _jwt for consistency

                // Step 5: Store session data (optional)
                _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id.ToString());
                _httpContextAccessor.HttpContext.Session.SetString("Username", user.Name ?? "");
                _httpContextAccessor.HttpContext.Session.SetString("Role", user.Role ?? "");

                // Return success message with user data and the generated JWT token
                var api = new List<GoogleUserInfo> { googleUser };
                return ("Successfully logged in.", true, api, token);
            }
            catch (Exception ex)
            {
                // Handle errors from Google response or other issues
                return ($"Google token verification failed. Error: {ex.Message}", false, new List<GoogleUserInfo>(), null);
            }
        }



        private string GenerateJwtToken(Crud user)
        {
            var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourapp",
                audience: "yourapp",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public string GenerateRandomPassword(int length = 12)
        {
            // Define the characters allowed in the password
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+[{]}|;:'\",<.>/?";

            // Create a random number generator
            var random = new Random();

            // Use StringBuilder to construct the password
            var password = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                // Choose a random character from the validChars string
                password.Append(validChars[random.Next(validChars.Length)]);
            }

            

            return password.ToString();
        }


    }
}

