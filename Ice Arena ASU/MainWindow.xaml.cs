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
            var fromTxt = dpFrom.Text;
            var toTxt = dpTo.Text;

            if (fromTxt == "")
                fromTxt = "1200-01-01";

            if (toTxt == "")
                toTxt = "3000-01-01";

            var from = Convert.ToDateTime(fromTxt);
            var to = Convert.ToDateTime(toTxt);

            var inc = Database.GetIncomes(from, to);
            var exp = Database.GetExpenses(from, to);

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
            {
                tbProfit.Foreground = Brushes.Green;
                tbProfitName.Text = "Прибыль";
            }

            if (profit < 0)
            {
                tbProfitName.Text = "Убыток";
                tbProfit.Foreground = Brushes.Red;
            }
            if (profit == 0)
            {
                tbProfitName.Text = "В ноль";
                tbProfit.Foreground = Brushes.Black;
            }

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
            var res = MessageBox.Show("Точно удалить?", "Подтвертиде удаление", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)
                Database.DeleteAll();
            
            SetStat();
        }

        private void btnDelById_Click(object sender, RoutedEventArgs e)
        {
            Database.DeleteById(tbIdDel.Text);
            SetStat();

        }

        private void DpTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetStat();
        }

        private void DpFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetStat();
        }
    }
}
