using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SurveyAPI.Constants;
using SurveyAPI.Data;
using SurveyAPI.Extensions;
using SurveyAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();

builder.Services.AddAuthorization();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

string secretKey = Environment.GetEnvironmentVariable("JWT__Key")!;
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters() {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT__Issuer"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT__Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        )
    };
});

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                      ?? builder.Configuration.GetConnectionString("WebApiDatabase");

builder.Services.AddDbContext<DataContext>(options => {
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<QuestionService>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { RoleConstants.USER, RoleConstants.ADMIN };
    foreach (var role in roles) {
        if(!await roleManager.RoleExistsAsync(role)) {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var seeder = new DatabaseSeeder(dbContext, userManager);
    await seeder.SeedAsync();
}

app.Run();
