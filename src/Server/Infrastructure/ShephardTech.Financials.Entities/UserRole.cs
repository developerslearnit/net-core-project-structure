namespace ShephardTech.Financials.Entities
{
    public partial class UserRole : BasicBaseEntity
    {
        public int UserRoleId { get; set; }

        public int RoleId { get; set; }

        public int UserId { get; set; }

    }
}
