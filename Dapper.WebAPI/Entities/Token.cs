namespace Dapper.WebAPI.Entities
{
    public class Token
    {
        public string accessToken { get; set; }

        public DateTime expiration { get; set; }
    }
}
