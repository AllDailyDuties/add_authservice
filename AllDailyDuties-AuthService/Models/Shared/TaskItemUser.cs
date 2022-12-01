namespace AllDailyDuties_AuthService.Models.Shared
{
    public class TaskItemUser
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public TaskItemUser(Guid _id, Guid _userId, string _username, string _email)
        {
            Id = _id;
            UserId = _userId;
            Username = _username;
            Email = _email;
        }

    }
}
