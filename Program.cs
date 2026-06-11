using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// SERVICES (DI SETUP)
// ==========================

// Controllers
builder.Services.AddControllers();

// ==========================
// EXERCISE 2 FIX (DI LIFETIMES)
// ==========================

// Scoped service
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// Singleton worker (FIXED via IServiceScopeFactory inside class)
builder.Services.AddSingleton<EnrollmentWorker>();

// Validate DI lifetimes early (IMPORTANT for lab)
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

// ==========================
// EXERCISE 3 FIX (OPTIONS PATTERN)
// ==========================

builder.Services
    .AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();

// ==========================
// AUTH (your lab setup)
// ==========================

builder.Services.AddAuthentication("TestScheme")
    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
        "TestScheme",
        options => { }
    );

builder.Services.AddAuthorization();

var app = builder.Build();

// ==========================
// PIPELINE ORDER (IMPORTANT)
// ==========================

// 1. Logging middleware FIRST
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ==========================
// ENDPOINTS
// ==========================

app.MapGet("/api/assessments/results", () =>
{
    return Results.Ok(new
    {
        courseCode = "CS-101",
        studentId = "S-001",
        letterGrade = "A"
    });
}).RequireAuthorization();

// Worker test endpoint (optional for Exercise 2 testing)
app.MapGet("/api/enrollments/worker-smoke", (EnrollmentWorker worker) =>
{
    worker.ProcessBatch();
    return Results.Ok("processed");
});

app.MapControllers();

app.Run();