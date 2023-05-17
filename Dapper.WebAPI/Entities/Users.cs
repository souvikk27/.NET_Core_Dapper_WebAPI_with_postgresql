namespace Dapper.WebAPI.Entities
{
    public class Users
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? SecurityStamp { get; set; }
    }
}
