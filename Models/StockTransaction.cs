namespace Assignment.Task.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }

        public int MaterialId { get; set; }

        public int Quantity { get; set; }

        public int TransactionType { get; set; } //True Stock In , False Stock Out

        public DateTime TransactionDate { get; set; }
    }
}
