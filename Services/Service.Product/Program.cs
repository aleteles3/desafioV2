using Domain.MassTransit.Queues;
using Service.Product.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Infra.IoC.Core.DependencyInjector.AddServices(builder.Services, builder.Configuration);
Infra.IoC.Product.DependencyInjector.AddServices(builder.Services, builder.Configuration);
Infra.IoC.Authentication.DependencyInjector.AddServices(builder.Services, builder.Configuration);
Infra.IoC.Swagger.DependencyInjector.AddServices(builder.Services, builder.Configuration);

var queueConsumer = new Dictionary<string, IEnumerable<Type>>()
{
    { MessageQueueProduct.AcceptOrder.Name, new[] { typeof(AcceptOrderConsumer) } }
};

Infra.IoC.MassTransit.DependencyInjector.AddServices(builder.Services, queueConsumer);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

var url = app.Configuration.GetSection("ServicesUrls").GetSection("Product").Value;
app.Run(url);
