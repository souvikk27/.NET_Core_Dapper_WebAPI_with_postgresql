namespace Dapper.WebAPI.Data_Transfer_Objects
{
    public class UserRolesDto
    {
        public Guid userid { get; set; }

        public List<Role>? roles { get; set; } 
    }

    public class Role
    {
        public Guid roleid { get; set; }
    }
}
