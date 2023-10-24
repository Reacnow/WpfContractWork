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

namespace WpfContractWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BD.ClassFrame.frmObj = frmMain;
            BD.ClassFrame.frmObj.Navigate(new Pages.PageContracts());
        }

        private void frmMain_ContentRendered(object sender, EventArgs e)
        {

        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageEmployees());
        }

        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageContracts());
        }
    }
}
