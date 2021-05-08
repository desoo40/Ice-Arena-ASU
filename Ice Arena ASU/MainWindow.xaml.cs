using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ice_Arena_ASU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Transaction
        {
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }

            public Transaction(string name, decimal amount, DateTime date)
            {
                Name = name;
                Amount = amount;
                Date = date;
            }

        }

        public List<Transaction> Transactions { get; set; } 

        public MainWindow()
        {
            InitializeComponent();

            Transactions = new List<Transaction>();

            var tr1 = new Transaction("Ice", 5000, DateTime.Today);
            var tr2 = new Transaction("Skates", 25000, DateTime.Today);
            var tr3 = new Transaction("Sticks", 53000, DateTime.Today);

            Transactions.Add(tr1);
            Transactions.Add(tr2);
            Transactions.Add(tr3);

            DataContext = this;
        }
    }
}
