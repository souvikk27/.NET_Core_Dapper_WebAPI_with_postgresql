namespace Dapper.WebAPI.Interfaces
{
    public interface IUnitOfWork
    {
       IProductRepository Products { get; }
       IUserRepository Users { get; }
       IRoleRepository Roles { get; }
       IRoleClaimRepository RoleClaims { get; }
    }
}
