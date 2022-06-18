using Application.Product.AutoMapper;
using Application.Product.Interfaces;
using Application.Product.Services;
using Domain.Product.Cqrs.Category.Handlers;
using Domain.Product.Cqrs.Order.Handlers;
using Domain.Product.Cqrs.Product.Handlers;
using Domain.Product.Interfaces;
using Infra.Data.Product.Context;
using Infra.Data.Product.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IoC.Product
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
        {
            //Adding Database Connection
            services.AddDbContext<ProductContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ?? string.Empty, 
                    x => x.MigrationsAssembly(typeof(ProductContext).Assembly.FullName)));
            
            //Adding AutoMapper
            services.AddAutoMapper(typeof(AutoMapperConfiguration));
            
            //Adding Mediator
            services.AddMediatR(new[]
            {
                typeof(ProductCommandHandler),
                typeof(CategoryCommandHandler),
                typeof(OrderCommandHandler)
            });

            //Adding Services
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductAppService, ProductAppService>();
            services.AddScoped<ICategoryAppService, CategoryAppService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}