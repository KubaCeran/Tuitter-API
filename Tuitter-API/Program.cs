using Microsoft.EntityFrameworkCore;
using Tuitter_API.Data.DataContext;
using Tuitter_API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDataContext(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.RegisterRepositories();
builder.Services.RegisterApplicationServices();

const string AllowAll = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowAll, policy =>
    {
        policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

app.UseCors(AllowAll);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
