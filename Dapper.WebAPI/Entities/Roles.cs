namespace Dapper.WebAPI.Entities
{
    public class Roles
    {
        public string? Role_Id { get; set; }
        public string? Name { get; set;}
        public string? NormalizedName { get; set;}
        public string? ConcurrencyStamp { get; set; }
    }
}
