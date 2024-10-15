namespace GymMembershipAPI.Domain
{
    public class Role
    {
        public int Id {  get; set; }
        public string Name { get; set; }

        public List<UserRole> Users { get; set; } = [];
    }
}
