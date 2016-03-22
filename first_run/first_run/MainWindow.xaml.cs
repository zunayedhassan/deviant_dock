using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace first_run
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Hide();

            if (!Directory.Exists(path: Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Deviant Dock\Settings"))
                Directory.CreateDirectory(path: Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Deviant Dock\Settings");

            this.Close();
        }
    }
}
