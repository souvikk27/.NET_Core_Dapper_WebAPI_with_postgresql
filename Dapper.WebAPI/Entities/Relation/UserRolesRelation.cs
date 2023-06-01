namespace Dapper.WebAPI.Entities.Relation
{
    public class UserRolesRelation
    {
        public string? UserName { get; set; }
        public List<string>? RoleNames { get; set; }
    }
}
