namespace Assignment.Task.Models
{
    public class Material
    {
        public int Id { get; set; } 

        public string Name { get; set; } = "";

        public string Color { get; set; } = "";

        public int CurrentStock { get; set; }

        public int MinimumRequiredStock { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }


    }
}
