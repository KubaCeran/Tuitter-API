using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterRepositories();
builder.Services.RegisterApplicationServices();

var app = builder.CommonApiDiSetup();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
