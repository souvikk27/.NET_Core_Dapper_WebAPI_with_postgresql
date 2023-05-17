namespace Dapper.WebAPI.Data_Transfer_Objects
{
    public class RolesDto
    {
        public string? Role_Id { get; set; }
        public string? Name { get; set; }
        public string? Normalized_Name { get; set; }
        public string? ConcurrencyStamp { get; set; }
    }
}
