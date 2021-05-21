using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ice_Arena_ASU
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public Transaction(int id, string name, decimal amount, DateTime date)
        {
            Id = id;

            Name = name;
            Amount = amount;
            Date = date;
        }
        public Transaction(string id, string name, string amount, string date)
        {
            Id = Convert.ToInt32(id);
            Name = name;
            Amount = Convert.ToDecimal(amount);
            Date = DateTime.Parse(date);
        }
    }
}
