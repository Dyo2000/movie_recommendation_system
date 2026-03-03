using Microsoft.EntityFrameworkCore;
using MovieRecommendation.API.Data;
using MovieRecommendation.API.Models.Settings;
using MovieRecommendation.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Avoid object reference cycles during JSON serialization
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Optional: Include XML comments if you want inline descriptions in Swagger UI
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddAuthorization();

// DbContext
builder.Services.AddDbContext<MovieRecommendationContext>(options =>
    options.UseSqlite("Data Source=movies.db"));

// TMDb settings & services
builder.Services.Configure<TMDbSettings>(
    builder.Configuration.GetSection("TMDb"));

builder.Services.AddHttpClient();
builder.Services.AddScoped<TmdbService>();

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MovieRecommendationContext>();
    context.Database.Migrate();
    DataSeeder.Seed(context);
}

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();