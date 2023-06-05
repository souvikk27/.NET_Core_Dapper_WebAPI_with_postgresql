namespace Dapper.WebAPI.Entities
{
    public class RoleClaims
    {
        //public int Id { get; set; }

        public string? RoleId { get; set; }

        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }
    }
}
