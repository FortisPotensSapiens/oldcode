namespace AleBotApi.Models.RDtos
{
    public class UserServerRDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;
    }
}
