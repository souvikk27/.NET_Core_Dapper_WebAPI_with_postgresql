namespace Dapper.WebAPI.Entities.Relation
{
    public class RoleClaimRelation
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public List<ClaimList>? ClaimList { get; set; } 
    }

    public class ClaimList
    {
        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }

        public bool Selected { get; set; }
    }
}
