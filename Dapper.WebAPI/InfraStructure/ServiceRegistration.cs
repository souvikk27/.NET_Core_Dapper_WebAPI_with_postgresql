using Dapper.WebAPI.Helpers;
using Dapper.WebAPI.Interfaces;
using Dapper.WebAPI.Repositories;

namespace Dapper.WebAPI.InfraStructure
{
    public static class ServiceRegistration
    {
        //Specifically for Repositories
        public static void AddInfraStructure(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUserRepository,  UserRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        //For Helper Classes
        public static void AddHelpers(this IServiceCollection services)
        {
            services.AddTransient<ExcelOperations>();
            services.AddTransient<PasswordEncrypt>();
        }
    }
}
