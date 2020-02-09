using Notatki.modelCode;
using Notatki.modelCode.Reminder;
using Notatki.viewHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Notatki
{
    public partial class ReminderSettings : Window
    { 
        List<Grid> ListOfRemindersGrids = new List<Grid>();
        List<DailyReminder> OldListOfReminders = new List<DailyReminder>();
        ModelManager model;
        ResourceDictionary myResourceDictionary = new ResourceDictionary
        {
            Source = new Uri("Dictionary1.xaml", UriKind.Relative)
        };

        public ReminderSettings(List<DailyReminder> OldListOfReminders, ModelManager model)
        {
            this.model = model;
            this.OldListOfReminders = OldListOfReminders;
            InitializeComponent();
            WindowEnricher.enrichWindow(this);
            addElements(OldListOfReminders);
        }

        private void AddButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            DailyReminder dailyReminder = new DailyReminder();
            addElements(new List<DailyReminder>() { dailyReminder} );
        }

        private void addElements(List<DailyReminder> listOfReminders)
        {
            foreach (DailyReminder elemet in listOfReminders)
            {
                Grid newElement = createElementList(elemet);
                ListOfRemindersGrids.Add(newElement);
                this.ReminderListGrid.Children.Add(newElement);
            }

            updateVerticalPositionOfListElements();
        }

        private void removeElement(String name)
        {
            foreach (Grid elemet in ListOfRemindersGrids)
            {
                String elementName = ((TextBox)LogicalTreeHelper.FindLogicalNode(elemet, textboxName)).Text;
                if(elementName.Equals(name)) {
                    ListOfRemindersGrids.Remove(elemet);
                    this.ReminderListGrid.Children.Remove(elemet);
                    return;
                }
            }

            updateVerticalPositionOfListElements();
        }

        private void updateVerticalPositionOfListElements()
        {
            int verticalPosition = 40;
            foreach(Grid elemet in ListOfRemindersGrids) {
                elemet.Margin = new Thickness(10, verticalPosition, 10, 0);
                verticalPosition += 65;
            }
        }

        static String textboxName = "name";
        static String[] daysName = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        public Grid createElementList(DailyReminder reminder)
        {
            Grid grid = new Grid();
            SolidColorBrush myBrush = new SolidColorBrush(Colors.Red);
            myBrush.Opacity = 0.1;
            grid.Background = myBrush;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Top;
            
            grid.Height = 60;
            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            TextBox textbox = new TextBox();
            textbox.Width = 180;
            textbox.HorizontalAlignment = HorizontalAlignment.Left;
            grid.Children.Add(textbox);
            textbox.Name = textboxName;
            textbox.Text = reminder.Name;
            Boolean[] days = new Boolean[7] { reminder.Monday, reminder.Tuesday, reminder.Wednesday, reminder.Thursday, reminder.Friday, reminder.Saturday, reminder.Sunday };
            for (int i = 0; i < 7; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.IsChecked = days[i];
                checkBox.Name = daysName[i];
                checkBox.Margin = new Thickness((i*100) + 220, 24, 10, 0);
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                grid.Children.Add(checkBox);
            }

            ComboBox comboBox = new ComboBox();
            ComboBoxItem comboBoxItem = new ComboBoxItem();
            comboBoxItem.Content = "INTEGER";
            comboBoxItem.Style = myResourceDictionary["ComboBoxItemStyle"] as Style;
            ComboBoxItem comboBoxItem2 = new ComboBoxItem();
            comboBoxItem2.Content = "BOOLEAN";
            comboBoxItem2.Style = myResourceDictionary["ComboBoxItemStyle"] as Style;
            comboBoxItem.IsSelected = true;

            comboBox.Items.Add(comboBoxItem);
            comboBox.Items.Add(comboBoxItem2);
            comboBox.Margin = new Thickness( 870, 0, 10, 0);
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.Height = 32;
            comboBox.Style = myResourceDictionary["ComboBoxStyle"] as Style;
            comboBox.Name = "type";
            grid.Children.Add(comboBox);

            if (reminder.type.Equals(ReminderType.BOOLEAN))
            {
                comboBox.SelectedItem = comboBoxItem2;
            }
            else
            {
                comboBox.SelectedItem = comboBoxItem;
            }

            return grid;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            String name = ((TextBox)LogicalTreeHelper.FindLogicalNode(grid, textboxName)).Text;
            DragDrop.DoDragDrop(grid, name, DragDropEffects.Move);
        }

        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            String gridName = (String)e.Data.GetData(DataFormats.UnicodeText);
            removeElement(gridName);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            List<DailyReminder> NewListOfReminders = new List<DailyReminder>();
            foreach (Grid elemet in ListOfRemindersGrids)
            {
                DailyReminder DailyReminder = new DailyReminder();
                DailyReminder.Name = ((TextBox)LogicalTreeHelper.FindLogicalNode(elemet, textboxName)).Text;
                DailyReminder.Monday = ((CheckBox)LogicalTreeHelper.FindLogicalNode(elemet, daysName[0])).IsChecked ?? false;
                DailyReminder.Tuesday = ((CheckBox)LogicalTreeHelper.FindLogicalNode(elemet, daysName[1])).IsChecked ?? false;
                DailyReminder.Wednesday = ((CheckBox)LogicalTreeHelper.FindLogicalNode(elemet, daysName[2])).IsChecked ?? false;
                DailyReminder.Thursday = ((CheckBox)LogicalTreeHelper.FindLogicalNode(elemet, daysName[3])).IsChecked ?? false;
                DailyReminder.Friday = ((CheckBox)LogicalTreeHelper.FindLogicalNode(elemet, daysName[4])).IsChecked ?? false;
                DailyReminder.Saturday = ((CheckBox)LogicalTreeHelper.FindLogicalNode(elemet, daysName[5])).IsChecked ?? false;
                DailyReminder.Sunday = ((CheckBox)LogicalTreeHelper.FindLogicalNode(elemet, daysName[6])).IsChecked ?? false;
                ComboBoxItem comboBoxItem = (ComboBoxItem)((ComboBox)LogicalTreeHelper.FindLogicalNode(elemet, "type")).SelectedItem;
                Enum.TryParse(comboBoxItem.Content.ToString(), out ReminderType type);
                DailyReminder.type = type;
                NewListOfReminders.Add(DailyReminder);
            }
            var events = CreateEvents(NewListOfReminders, this.OldListOfReminders);
            foreach (Event actualEvent in events)
            {
                model.CommitEvent(actualEvent);
            }
            model.SaveModel();
            this.Close();
        }

        private List<Event> CreateEvents(List<DailyReminder> NewListOfReminders, List<DailyReminder> OldListOfReminders)
        {
            List<Event> result = new List<Event>();
            foreach (DailyReminder actualReminder in NewListOfReminders)
            {
                if (OldListOfReminders.FindAll(x => x.Name.Equals(actualReminder.Name)).Count == 0)
                {
                    result.Add(new AddDailyReminderEvent(actualReminder));
                }
            }

            foreach (DailyReminder oldReminder in OldListOfReminders)
            {
                if (NewListOfReminders.FindAll(x => x.Name.Equals(oldReminder.Name)).Count == 0)
                {
                    result.Add(new RemoveDailyReminderEvent(oldReminder));
                }
            }

            foreach (DailyReminder actualReminder in NewListOfReminders)
            {
                int oldIndex = OldListOfReminders.FindIndex(x => x.Name.Equals(actualReminder.Name));
                if (oldIndex != -1 && !OldListOfReminders[oldIndex].Equals(actualReminder))
                {
                    result.Add(new EditDailyReminderEvent(OldListOfReminders[oldIndex], actualReminder));
                }
            }

            return result;
        }

    }
}
