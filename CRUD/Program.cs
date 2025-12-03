using CRUD.Data;
using CRUD.Model;
using CRUD.Services;
using CRUD.Services.FileValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
});

// =====================
//  JWT Configuration
// =====================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// =====================
//  Add Services
// =====================
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<CrudDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// =====================
//  JWT Authentication
// =====================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = ClaimTypes.Role,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Authorization should come AFTER authentication
builder.Services.AddAuthorization();

// Password hasher
builder.Services.AddScoped<PasswordHasher<Crud>>();

// Application Services
builder.Services.AddScoped<ICrudServices, CrudServices>();
builder.Services.AddScoped<IFileValidationServices, FileValidationServices>();
builder.Services.AddScoped<IAppointmentServices, AppointmentServices>();
builder.Services.AddScoped<IRoomCatalogServices, RoomCatalogServices>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddSingleton<JwtService>();


var app = builder.Build();

// =====================
//  Middleware Pipeline
// =====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins");

app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();   // MUST come before UseAuthorization()
app.UseAuthorization();

app.MapControllers();
app.Run();
