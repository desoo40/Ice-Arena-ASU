using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Transaction> Incomes { get; set; }
        public ObservableCollection<Transaction> Expenses { get; set; }
        private db Database { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Database = new db();

            Expenses = new ObservableCollection<Transaction>();
            Incomes = new ObservableCollection<Transaction>();

            DataContext = this;

            SetStat();
        }

        private void SetStat()
        {
            var inc = Database.GetIncomes();
            var exp = Database.GetExpenses();

            Incomes.Clear();
            Expenses.Clear();

            decimal expenses = 0;
            decimal incomes = 0;

            foreach (var el in exp)
            {
                Expenses.Add(el);
                expenses += el.Amount;
            }
            foreach (var el in inc)
            {
                Incomes.Add(el);
                incomes += el.Amount;
            }

            decimal profit = incomes - expenses;

            tblockIncome.Text = incomes.ToString();
            tblockIncome.Foreground = Brushes.Green;

            tblockExpense.Text = expenses.ToString();
            tblockExpense.Foreground = Brushes.Red;

            tbProfit.Text = profit.ToString();
            tbProfit.Foreground = Brushes.Black;

            if (profit > 0)
                tbProfit.Foreground = Brushes.Green;
            if (profit < 0)
                tbProfit.Foreground = Brushes.Red;

        }

        private void btn_addExpense_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddOperationWindow("Расход");
            wind.ShowDialog();

            if (wind.trans != null)
                Database.AddTransaction(
                    Operation.Expense,
                    wind.trans.Name,
                    wind.trans.Amount,
                    wind.trans.Date
                );

            SetStat();
        }

        private void btn_addIncome_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddOperationWindow("Доход");
            wind.ShowDialog();

            if (wind.trans != null)
                Database.AddTransaction(
                    Operation.Income,
                    wind.trans.Name,
                    wind.trans.Amount,
                    wind.trans.Date
                );

            SetStat();
        }

        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            Database.DeleteAll();
            SetStat();
        }

        private void btnDelById_Click(object sender, RoutedEventArgs e)
        {
            Database.DeleteById(tbIdDel.Text);
            SetStat();

        }
    }
}
