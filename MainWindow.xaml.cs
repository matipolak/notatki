using Notatki.modelCode;
using Notatki.viewHelper;
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

namespace Notatki
{
    public partial class MainWindow : Window
    {
        ModelManager model = new ModelManager();
        public MainWindow()
        {
            model.LoadModel();
            InitializeComponent();
            WindowEnricher.enrichWindow(this);
        }

        private void Edit_reminder_click(object sender, RoutedEventArgs e)
        {
            ReminderSettings win = new ReminderSettings(model.GetActualState().DailyReminders, model);
            win.Show();
        }
    }
}
