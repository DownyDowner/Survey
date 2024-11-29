using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SurveyAPI.Data;
using SurveyAPI.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddDbContext<DataContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"));
});

builder.Services.AddScoped<QuestionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<IdentityUser>();

app.MapGet("users/me", async (ClaimsPrincipal claims, DataContext context) => {
    string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
    return await context.Users.FindAsync(userId);
}).RequireAuthorization();

app.MapPost("logout", async (SignInManager<IdentityUser> signInManager) => {
    await signInManager.SignOutAsync();
    return Results.Ok(new { message = "Logged out successfully" });
}).RequireAuthorization();

app.Run();
