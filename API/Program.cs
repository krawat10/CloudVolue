using API;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

if(builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseInMemoryDatabase("API"));
}
else
{
    builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddHttpClient("Calculation", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["CalculationURL"]);
});

var app = builder.Build();
app.UseDeveloperExceptionPage();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/health");
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if(app.Environment.IsProduction())
{
    using (var serviceScope = app.Services
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope())
    {
        using var context = serviceScope.ServiceProvider.GetService<ApplicationContext>();
        context?.Database.Migrate();
    }
}

app.Run();
