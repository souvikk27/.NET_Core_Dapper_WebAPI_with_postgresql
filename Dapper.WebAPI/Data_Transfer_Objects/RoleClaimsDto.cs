namespace Dapper.WebAPI.Data_Transfer_Objects
{
    public class RoleClaimsDto
    {
        public string? RoleId { get; set; }

        public List<ClaimList>? claims { get; set; }
    }

    public class ClaimList
    {
        public string? ClaimType { get; set; }

        public string? ClaimValue { get; set; }

        public bool Selected { get; set; }
    }
}
