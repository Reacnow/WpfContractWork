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
    /// Логика взаимодействия для PageContractsAdd.xaml
    /// </summary>
    public partial class PageContractsAdd : Page
    {
        private Contract _contract = new Contract();
        public PageContractsAdd(Contract selectedContract)
        {
            InitializeComponent();
            if(selectedContract != null)
                _contract= selectedContract;
            DataContext = _contract;
            
            CmbStCntr.ItemsSource = Contract_WorkEntities.GetContext().Contract_Status.ToList();
            CmbStCntr.SelectedValuePath = "id_status";
            CmbStCntr.DisplayMemberPath = "status";

            CmbTpCntr.ItemsSource = Contract_WorkEntities.GetContext().Type_Of_Contract.ToList();
            CmbTpCntr.SelectedValuePath = "id_type";
            CmbTpCntr.DisplayMemberPath = "title";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_contract.title))
                errors.AppendLine("Укажите название контракта!");
            if (_contract.summ < 0)
                errors.AppendLine("Укажите сумму контракта!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_contract.code == 0)
                Contract_WorkEntities.GetContext().Contract.Add(_contract);
            try
            {
                Contract_WorkEntities.GetContext().SaveChanges();
                MessageBox.Show("Инфорация сохранена успешно!");
                BD.ClassFrame.frmObj.GoBack();
            }
            catch ( Exception ex) 
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}