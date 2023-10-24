using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
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
    /// Логика взаимодействия для PageEmployeesAdd.xaml
    /// </summary>
    public partial class PageEmployeesAdd : Page
    {
        private Employee _employee = new Employee();
        public PageEmployeesAdd(Employee selectedemp)
        {
            InitializeComponent();

            if (selectedemp != null)
            {
                _employee = selectedemp;
            }

            DataContext = _employee;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_employee.FIO))
                errors.AppendLine("Укажите имя сотрудника");
            if (string.IsNullOrWhiteSpace(_employee.adress))
                errors.AppendLine("Укажите адрес сотрудника");
            if (string.IsNullOrWhiteSpace(_employee.phone))
                errors.AppendLine("Укажите номер телефона сотрудника");
            if (string.IsNullOrWhiteSpace(_employee.post))
                errors.AppendLine("Укажите должность сотрудника");
            if (_employee.salary < 0)
                errors.AppendLine("Укажите оклад сотрудника");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_employee.id_employee == 0)
                Contract_WorkEntities.GetContext().Employee.Add(_employee);

            try
            {
                Contract_WorkEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация успешно сохранена!");
                BD.ClassFrame.frmObj.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.GoBack();
        }
    }
}
