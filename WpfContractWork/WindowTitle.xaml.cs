﻿using System;
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
using System.Windows.Shapes;

namespace WpfContractWork
{
    /// <summary>
    /// Логика взаимодействия для WindowTitle.xaml
    /// </summary>
    public partial class WindowTitle : Window
    {
        public WindowTitle()
        {
            InitializeComponent();
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
