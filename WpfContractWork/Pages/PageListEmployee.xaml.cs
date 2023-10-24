using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
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
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace WpfContractWork.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageListEmployee.xaml
    /// </summary>
    public partial class PageListEmployee : Page
    {
        public PageListEmployee()
        {
            InitializeComponent();
            var currentEmployee = Contract_WorkEntities.GetContext().Employee.ToList();
            LViewEmployee.ItemsSource = currentEmployee;
            DataContext = LViewEmployee;
            CmbFiltr.Items.Add("Все должности");
            foreach (var item in Contract_WorkEntities.GetContext().Employee.
                Select(x => x.post).Distinct().ToList())
                CmbFiltr.Items.Add(item);
        }

        private void BtnSaveToPDF_Click(object sender, RoutedEventArgs e)
        {
            var allEmployees = Contract_WorkEntities.GetContext().Employee.ToList();
            var application = new Word.Application();
            Word.Document document = application.Documents.Add();
            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Сотрудники";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlue;
            empRange.InsertParagraphAfter();
            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, allEmployees.Count() + 1, 4);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            Word.Range cellRange;
            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "ФИО";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Адрес";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Номер телефона";
            cellRange = paymentsTable.Cell(1, 4).Range;
            cellRange.Text = "Оклад";
            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            for (int i = 0; i < allEmployees.Count(); i++)
            {
                var currentEmployee = allEmployees[i];
                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentEmployee.FIO;
                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = currentEmployee.adress;
                cellRange = paymentsTable.Cell(i + 2, 3).Range;
                cellRange.Text = currentEmployee.phone;
                cellRange = paymentsTable.Cell(i + 2, 4).Range;
                cellRange.Text = currentEmployee.salary.ToString();
            }
            Employee maxSalary = Contract_WorkEntities.GetContext().Employee
                .OrderByDescending(p => p.salary).FirstOrDefault();
            if (maxSalary != null)
            {
                Word.Paragraph maxSalaryParagraph = document.Paragraphs.Add();
                Word.Range maxSalaryRange = maxSalaryParagraph.Range;
                maxSalaryRange.Text = $"Самый дорогооплачиваемый оклад - {maxSalary.salary}";
                maxSalaryRange.Font.Color = Word.WdColor.wdColorDarkRed;
                maxSalaryRange.InsertParagraphAfter();
            }

            Employee minSalary = Contract_WorkEntities.GetContext().Employee
                .OrderBy(p => p.salary).FirstOrDefault();
            if (minSalary != null)
            {
                Word.Paragraph minSalaryParagraph = document.Paragraphs.Add();
                Word.Range minSalaryRange = minSalaryParagraph.Range;
                minSalaryRange.Text = $"Самый малооплачиваемый оклад - {minSalary.salary}";
                minSalaryRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                minSalaryRange.InsertParagraphAfter();
            }

            application.Visible = true;

            document.SaveAs2(@"C:\Users\Mvideo\OneDrive\Рабочий стол\Эдик\3 курс\Девяткин Практика\WpfContractWork\WpfContractWork\bin\Debug\Test.pdf", Word.WdExportFormat.wdExportFormatPDF);
            
        }

        private void BtnSaveToExcelTemplate_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open($"{Directory.GetCurrentDirectory()}\\Шаблон.xlsx");
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Cells[4, 2] = DateTime.Now.ToString();
            ws.Cells[4, 5] = 7;
            int indexRows = 6;

            ws.Cells[1][indexRows] = "Номер";
            ws.Cells[2][indexRows] = "ФИО";
            ws.Cells[3][indexRows] = "Адрес";
            ws.Cells[4][indexRows] = "Номер телефона";
            ws.Cells[5][indexRows] = "Должность";
            ws.Cells[6][indexRows] = "Оклад";

            var printItems = LViewEmployee.Items;
            foreach (Employee item in printItems)
            {
                ws.Cells[1][indexRows + 1] = indexRows;
                ws.Cells[2][indexRows + 1] = item.FIO;
                ws.Cells[3][indexRows + 1] = item.adress;
                ws.Cells[4][indexRows + 1] = item.phone;
                ws.Cells[5][indexRows + 1] = item.post;
                ws.Cells[6][indexRows + 1] = item.salary;

                indexRows++;
            }
            ws.Cells[indexRows + 2, 3] = "Подпись";
            ws.Cells[indexRows + 2, 5] = "Девяткин Э.М.";
            excelApp.Visible = true;
        }

        private void BtnSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            var app = new Excel.Application();

            Excel.Workbook wb = app.Workbooks.Add();

            Excel.Worksheet worksheet = app.Worksheets.Item[1];
            int indexRows = 1;

            worksheet.Cells[1][indexRows] = "Номер";
            worksheet.Cells[2][indexRows] = "ФИО";
            worksheet.Cells[3][indexRows] = "Адрес";
            worksheet.Cells[4][indexRows] = "Номер телефона";
            worksheet.Cells[5][indexRows] = "Должность";
            worksheet.Cells[6][indexRows] = "Оклад";


            var printItems = LViewEmployee.Items;

            foreach (Employee item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.FIO;
                worksheet.Cells[3][indexRows + 1] = item.adress;
                worksheet.Cells[4][indexRows + 1] = item.phone;
                worksheet.Cells[5][indexRows + 1] = item.post;
                worksheet.Cells[6][indexRows + 1] = item.salary;


                indexRows++;
            }
            Excel.Range range = worksheet.Range[worksheet.Cells[2][indexRows + 1],
                    worksheet.Cells[5][indexRows + 1]];
            range.ColumnWidth = 20; 
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            app.Visible = true;
        }

        private void CmbFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbFiltr.SelectedValue.ToString() == "Все должности")
            {
                LViewEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.ToList();
            }
            else
            {
                LViewEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.
                    Where(x => x.post == CmbFiltr.SelectedValue.ToString()).ToList();
            }
        }

        private void RbDown_Checked(object sender, RoutedEventArgs e)
        {
            LViewEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.OrderByDescending(x => x.salary).ToList();
        }

        private void RbUp_Checked(object sender, RoutedEventArgs e)
        {
            LViewEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.OrderBy(x => x.salary).ToList();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TxtSearch.Text;
            if (TxtSearch.Text != null)
            {
                LViewEmployee.ItemsSource = Contract_WorkEntities.GetContext().Employee.
                    Where(x => x.FIO.Contains(search)
                    || x.adress.Contains(search)
                    || x.phone.Contains(search)
                    || x.post.Contains(search)
                    || x.salary.ToString().Contains(search)).ToList();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            BD.ClassFrame.frmObj.Navigate(new Pages.PageEmployeesAdd((sender as Button).DataContext as Employee));
        }

        private void BtnSaveToWord_Click(object sender, RoutedEventArgs e)
        {
            var allEmployees = Contract_WorkEntities.GetContext().Employee.ToList();
            var application = new Word.Application();
            Word.Document document = application.Documents.Add();
                Word.Paragraph empParagraph = document.Paragraphs.Add();
                Word.Range empRange = empParagraph.Range;
                empRange.Text = "Сотрудники";
                empRange.Font.Bold = 4;
                empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlue;
            empRange.InsertParagraphAfter();
                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table paymentsTable = document.Tables.Add(tableRange, allEmployees.Count() + 1, 4);
                paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                Word.Range cellRange;
                cellRange = paymentsTable.Cell(1, 1).Range;
                cellRange.Text = "ФИО";
                cellRange = paymentsTable.Cell(1, 2).Range;
                cellRange.Text = "Адрес";
                cellRange = paymentsTable.Cell(1, 3).Range;
                cellRange.Text = "Номер телефона";
                cellRange = paymentsTable.Cell(1, 4).Range;
                cellRange.Text = "Оклад";
                paymentsTable.Rows[1].Range.Bold = 1;
                paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                for (int i = 0; i < allEmployees.Count(); i++)
                {
                    var currentEmployee = allEmployees[i];
                    cellRange = paymentsTable.Cell(i + 2, 1).Range;
                    cellRange.Text = currentEmployee.FIO;
                    cellRange = paymentsTable.Cell(i + 2, 2).Range;
                    cellRange.Text = currentEmployee.adress;
                    cellRange = paymentsTable.Cell(i + 2, 3).Range;
                    cellRange.Text = currentEmployee.phone;
                    cellRange = paymentsTable.Cell(i + 2, 4).Range;
                    cellRange.Text = currentEmployee.salary.ToString();
            }
            Employee maxSalary = Contract_WorkEntities.GetContext().Employee
                .OrderByDescending(p => p.salary).FirstOrDefault();
            if (maxSalary != null)
            {
                Word.Paragraph maxSalaryParagraph = document.Paragraphs.Add();
                Word.Range maxSalaryRange = maxSalaryParagraph.Range;
                maxSalaryRange.Text = $"Самый дорогооплачиваемый оклад - {maxSalary.salary}";
                maxSalaryRange.Font.Color = Word.WdColor.wdColorDarkRed;
                maxSalaryRange.InsertParagraphAfter();
            }

            Employee minSalary = Contract_WorkEntities.GetContext().Employee
                .OrderBy(p => p.salary).FirstOrDefault();
            if (minSalary != null)
            {
                Word.Paragraph minSalaryParagraph = document.Paragraphs.Add();
                Word.Range minSalaryRange = minSalaryParagraph.Range;
                minSalaryRange.Text = $"Самый малооплачиваемый оклад - {minSalary.salary}";
                minSalaryRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                minSalaryRange.InsertParagraphAfter();
            }

            application.Visible = true;

            document.SaveAs2(@"C:\Users\Mvideo\OneDrive\Рабочий стол\Эдик\3 курс\Девяткин Практика\WpfContractWork\WpfContractWork\bin\Debug\Test.docx");
        }

        //private void btnImage_Click(object sender, RoutedEventArgs e)
        //{
        //    //OpenFileDialog op = new OpenFileDialog();
        //    //op.Title = "Select a picture";
        //    //op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
        //    //  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        //    //  "Portable Network Graphic (*.png)|*.png";
        //    //if (op.ShowDialog() == true)
        //    //{
        //    //    im.Source = new BitmapImage(new Uri(op.FileName));

        //    //    using (Contract_WorkEntities imageEntities = new Contract_WorkEntities())
        //    //    {
        //    //        Employee imgStore = imageEntities.Employee.Create();

        //    //        imgStore.FIO = new FileInfo(op.FileName).Name;
        //    //        imgStore.photo = File.ReadAllText(op.FileName);
        //    //        imageEntities.Employee.Add(imgStore);
        //    //        imageEntities.SaveChanges();
        //    //    }
        //    //}

        //}
    }
}
