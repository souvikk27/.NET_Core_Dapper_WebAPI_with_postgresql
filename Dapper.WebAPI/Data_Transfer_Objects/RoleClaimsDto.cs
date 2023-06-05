namespace Dapper.WebAPI.Data_Transfer_Objects
{
    public class RoleClaimsDto
    {
        public string? RoleId { get; set; }

        public string? ClaimType { get; set; }

        public List<ClaimList> claims { get; set; }
    }

    public class ClaimList
    {
        public string? ClaimValue { get; set; }
    }
}
