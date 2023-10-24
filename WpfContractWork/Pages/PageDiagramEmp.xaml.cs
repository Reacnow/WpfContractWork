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
using System.Windows.Forms.DataVisualization.Charting;
using WpfContractWork.BD;

namespace WpfContractWork.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageDiagramEmp.xaml
    /// </summary>
    public partial class PageDiagramEmp : Page
    {
        public PageDiagramEmp()
        {
            InitializeComponent();
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Оклад")
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);

            ComboUser.ItemsSource = Contract_WorkEntities.GetContext().Employee.ToList();
            ComboChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (ComboUser.SelectedItem is Employee currentUser &&
                ComboChartTypes.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();
                var categoriesList = Contract_WorkEntities.GetContext().Employee.ToList();
                foreach (var category in categoriesList)
                {

                    currentSeries.Points.AddXY(category.FIO, category.salary);


                }
            }
        }
    }
}
