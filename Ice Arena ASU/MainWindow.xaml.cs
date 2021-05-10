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
        private db Database { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Database = new db();

            Transactions = new List<Transaction>();

            var tr1 = new Transaction("Ice", 5000, DateTime.Today);
            var tr2 = new Transaction("Skates", 25000, DateTime.Today);
            var tr3 = new Transaction("Sticks", 53000, DateTime.Today);

            Transactions.Add(tr1);
            Transactions.Add(tr2);
            Transactions.Add(tr3);

            DataContext = this;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Database.AddTransaction(Operation.Expense, "test"+DateTime.Now, 69);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var transactions = Database.GetTransactionsByPeriod(DateTime.Now.AddDays(-30), DateTime.Now);
            var result = "Transactions:\n";
            foreach(var t in transactions)
            {
                result += t + "\n";
            }
            MessageBox.Show(result);
        }

        private void btn_addExpense_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddOperationWindow();
            wind.Show();
        }
    }
}
