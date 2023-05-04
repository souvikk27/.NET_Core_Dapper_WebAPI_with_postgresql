using Dapper.WebAPI.Helpers;
using Dapper.WebAPI.Interfaces;
using Dapper.WebAPI.Repositories;

namespace Dapper.WebAPI.InfraStructure
{
    public static class ServiceRegistration
    {
        public static void AddInfraStructure(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ExcelOperations>();
        }
    }
}
