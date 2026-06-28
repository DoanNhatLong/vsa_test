using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using bank.Entity;
using Microsoft.EntityFrameworkCore;
using bank.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Transient);
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
app.UseGlobalExceptionHandler();

app.UseFastEndpoints(c =>
{
    c.Errors.ResponseBuilder = (failures, statusCode, _) =>
    {
        var errors = failures.Select(f => new ApiError(f.PropertyName, f.ErrorMessage)).ToList();
        return ApiResponse<object>.ErrorResponse(
            400, "Validation failed", errors);
    };
});

app.UseSwaggerGen();

app.Run();
