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



		public override string ToString()
		{
            if(Id != 0)
            {
				return string.Format("Material Id : {4} ,  Name:{0} , Color: {1} , Current Stock: {2} , MinimumRequiredStock : {3} ", Name, Color, CurrentStock, MinimumRequiredStock , Id );

			}
			return string.Format("Name:{0} , Color: {1} , Current Stock: {2} , MinimumRequiredStock : {3} ", Name, Color, CurrentStock, MinimumRequiredStock);

		}
	}
}
