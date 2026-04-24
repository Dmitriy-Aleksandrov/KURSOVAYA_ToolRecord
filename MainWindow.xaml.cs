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

namespace APP_WPF_InstrumentControl
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            MainMenu win = new MainMenu();
            win.ShowDialog();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AuthWindow auth = new AuthWindow();
            auth.ShowDialog();
            this.Show();
            
        }

        private void Button_ToolTipClosing(object sender, ToolTipEventArgs e)
        {

        }
    }
}
