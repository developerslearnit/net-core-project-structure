namespace ShephardTech.Financials.Entities
{
    public partial class LoginToken : BasicBaseEntity
    {
        public int LoginTokenId { get; set; }

        public string Username { get; set; }

        public string AuthToken { get; set; }

        public DateTime ExpiryDate { get; set; }

    }
}
