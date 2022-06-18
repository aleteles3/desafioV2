var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Infra.IoC.Core.DependencyInjector.AddServices(builder.Services, builder.Configuration);
Infra.IoC.Users.DependencyInjector.AddServices(builder.Services, builder.Configuration);
Infra.IoC.Authentication.DependencyInjector.AddServices(builder.Services, builder.Configuration);
Infra.IoC.Swagger.DependencyInjector.AddServices(builder.Services, builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

var value = app.Configuration.GetSection("IsDevelopment").Value;
if (value != null && bool.Parse(value))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var url = app.Configuration.GetSection("ServicesUrls").GetSection("Users").Value;
app.Run(url);
