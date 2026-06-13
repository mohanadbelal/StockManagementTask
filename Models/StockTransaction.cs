using System.Drawing;
using System.Xml.Linq;

namespace Assignment.Task.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }

        public int MaterialId { get; set; }

        public int Quantity { get; set; }

        public int TransactionType { get; set; } //True Stock In , False Stock Out

        public DateTime TransactionDate { get; set; }

		public override string ToString()
		{
            return string.Format("Material Id : {0} ,  Qty:{1} , TransactionType: {2} ", MaterialId, Quantity, TransactionType == 1 ? "IN Stock":"Out Stock");

		}
	}
}
