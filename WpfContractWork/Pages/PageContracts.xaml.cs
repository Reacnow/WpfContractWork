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
    /// Логика взаимодействия для PageContracts.xaml
    /// </summary>
    public partial class PageContracts : Page
    {
        public PageContracts()
        {
            InitializeComponent();
            CmbTypeCon.ItemsSource = Contract_WorkEntities.GetContext().Type_Of_Contract.ToList();
            CmbTypeCon.SelectedValue = "id_type";
            CmbTypeCon.DisplayMemberPath = "title";
            CmbStatCon.ItemsSource = Contract_WorkEntities.GetContext().Contract_Status.ToList();
            CmbStatCon.SelectedValue = "id_status";
            CmbStatCon.DisplayMemberPath = "status";
        }

        private void CmbTypeCon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int typecon = CmbTypeCon.SelectedIndex + 1;
            dtgContract.ItemsSource = Contract_WorkEntities.GetContext().Contract.Where(x => x.id_type == typecon).ToList();
        }

        private void BtnResetFiltr_Click(object sender, RoutedEventArgs e)
        {
            dtgContract.ItemsSource = Contract_WorkEntities.GetContext().Contract.ToList();
        }

        private void CmbStatCon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int statcon = CmbStatCon.SelectedIndex + 1;
            dtgContract.ItemsSource = Contract_WorkEntities.GetContext().Contract.Where(x => x.id_status == statcon).ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageContractsAdd(null));
        }

        private void RbUp_Checked(object sender, RoutedEventArgs e)
        {
            dtgContract.ItemsSource = Contract_WorkEntities.GetContext().Contract.OrderBy(x => x.summ).ToList();
        }

        private void RbDown_Checked(object sender, RoutedEventArgs e)
        {
            dtgContract.ItemsSource = Contract_WorkEntities.GetContext().Contract.OrderByDescending(x => x.summ).ToList();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var Remove = dtgContract.SelectedItems.Cast<Contract>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {Remove.Count()} элементов?", "Внимание", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Contract_WorkEntities.GetContext().Contract.RemoveRange(Remove);
                    Contract_WorkEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    dtgContract.ItemsSource = Contract_WorkEntities.GetContext().Contract.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageContractsAdd((sender as Button).DataContext as Contract));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility== Visibility.Visible)
            {
                Contract_WorkEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dtgContract.ItemsSource = Contract_WorkEntities.GetContext().Contract.ToList();
            }
        }
    }
}
