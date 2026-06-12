namespace Assignment.Task.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public  byte[]? PasswordHash { get; set; }

        public string Role { get; set; } = "";

        public DateTime CreatedAt { get; set; }


    }
}
