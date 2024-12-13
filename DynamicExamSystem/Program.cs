using Application.Helpers;
using DynamicExamSystem.infrastructure.Data;
using DynamicExamSystem.infrastructure.repository.Implementations;
using DynamicExamSystem.infrastructure.repository.Interfaces;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(options =>
{
    options.AddConsole();
    options.SetMinimumLevel(LogLevel.Debug);
});

builder.Services.AddCors((options) => {
    options.AddPolicy("DevCors", (corsBuilder) => {
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });

    options.AddPolicy("ProdCors", (corsBuilder) => {
        corsBuilder.WithOrigins("https://myProductionSite.com")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services
    .AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IStudentExamRepository, StudentExamRepository>();

//builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(Jwt =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
        Jwt.SaveToken = true;
        Jwt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Student", policy => policy.RequireRole("Student"));
});

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await RoleSeeder.SeedRolesAsync(serviceProvider);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
