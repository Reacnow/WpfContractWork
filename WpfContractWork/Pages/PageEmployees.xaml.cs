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
using WpfContractWork.BD;

namespace WpfContractWork.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageEmployees.xaml
    /// </summary>
    public partial class PageEmployees : Page
    {
        public PageEmployees()
        {
            InitializeComponent();
            //dtgEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.ToList();
            CmbPost.ItemsSource = Contract_WorkEntities.GetContext().Employee.Select(x => x.post).Distinct().ToList();
        }

        private void BtnResetFiltr_Click(object sender, RoutedEventArgs e)
        {
            dtgEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.ToList();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TxtSearch.Text;
            dtgEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.Where(x => x.FIO.Contains(search)).ToList();
        }

        private void CmbPost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string pst = CmbPost.SelectedValue.ToString();
            dtgEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.Where(x => x.post == pst).ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageEmployeesAdd(null));
        }

        private void RbUp_Checked(object sender, RoutedEventArgs e)
        {
            dtgEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.OrderBy(x => x.salary).ToList();
        }

        private void RbDown_Checked(object sender, RoutedEventArgs e)
        {
            dtgEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.OrderByDescending(x => x.salary).ToList();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var Remove = dtgEmployee.SelectedItems.Cast<Employee>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {Remove.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Contract_WorkEntities.GetContext().Employee.RemoveRange(Remove);
                    Contract_WorkEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    dtgEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageEmployeesAdd((sender as Button).DataContext as Employee));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Contract_WorkEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dtgEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.ToList();
            }
        }

        private void BtnToList_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageListEmployee());
        }

        private void BtnToDiagram_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageDiagramEmp());
        }
    }
}
