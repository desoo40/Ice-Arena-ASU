using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ice_Arena_ASU
{
    /// <summary>
    /// Логика взаимодействия для AddOperationWindow.xaml
    /// </summary>
    public partial class AddOperationWindow : Window
    {
        public Transaction trans { get; set; }
        private Regex reg = new Regex("[+-]?([0-9]*[.])?[0-9]+");

        public AddOperationWindow(string type)
        {
            InitializeComponent();

            tbOperType.Text = type.ToUpper();

            if (type == "Расход")
                tbOperType.Foreground = Brushes.Red;

            if (type == "Доход")
                tbOperType.Foreground = Brushes.Green;

            CheckIfBtnActive();
        }

        private void CheckIfBtnActive()
        {
            if (tbTransName.Text == "" | tbTransSum.Text == "" | dpDate.Text == "")
                btnAddTrans.IsEnabled = false;
            else
                btnAddTrans.IsEnabled = true;
        }

        private void btnAddTrans_Click(object sender, RoutedEventArgs e)
        {
            if (!reg.IsMatch(tbTransSum.Text))
            {
                MessageBox.Show("В поле суммы должны быть только числа");
                tbTransSum.Text = "";
                return;
            }

            trans = new Transaction("-1", tbTransName.Text, tbTransSum.Text, dpDate.Text);
            this.Close();
        }

        private void tbTransName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfBtnActive();
        }

        private void tbTransSum_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfBtnActive();
        }

        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfBtnActive();
        }
    }
}
