namespace Dapper.WebAPI.Entities.Relation
{
    public class RoleClaimRelation
    {
        public string? RoleName { get; set; }

        public string? ClaimType { get; set; }

        public List<string>? ClaimValue { get; set; } 
    }
}
