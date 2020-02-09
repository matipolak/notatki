using Notatki.modelCode;
using Notatki.modelCode.Achievement;
using Notatki.modelCode.Reminder;
using Notatki.viewHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Logika interakcji dla klasy DailyAchievement.xaml
    /// </summary>
    public partial class DailyAchievement : Window
    {
        List<Grid> ListOfAchivements = new List<Grid>();
        ResourceDictionary myResourceDictionary = new ResourceDictionary
        {
            Source = new Uri("Dictionary1.xaml", UriKind.Relative)
        };
        DailyAchievements dailyAchievement;
        MainWindow mainWindow;

        public DailyAchievement(DateTime date, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            WindowEnricher.enrichWindow(this);
            this.dailyAchievement = AchievementManager.LoadAchievementl(date);

            for (int i = 0; i < dailyAchievement.achievements.Count; i++)
            {
                Grid newElement = createAchivementElementList(dailyAchievement.achievements[i]);
                ListOfAchivements.Add(newElement);
                this.AchievementsListGrid.Children.Add(newElement);
            }
            updateVerticalPositionOfListElements();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            String fileName = "achievements/" + AchievementManager.DateToString(dailyAchievement.date) + ".xml";
            AchievementManager.SaveAchievement(fileName, dailyAchievement);
            mainWindow.refreshDailyAchivement(dailyAchievement.date);
            this.Close();
        }

        private void AddButton_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        public Grid createAchivementElementList(Achievement achievement)
        {
            String name = achievement.Name;
            String achivement = achievement.Required ? "Required" : "Optional";
            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Height = 60;
            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            grid.Name = name;
            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;

            Label label = new Label();
            label.Width = 180;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = name;
            label.Name = "dateName";
            label.Style = myResourceDictionary["LabelStyle"] as Style;
            label.FontSize = 20;
            label.Margin = new Thickness(0, 10, 0, 0);
            grid.Children.Add(label);

            if(achievement.type == ReminderType.BOOLEAN)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.HorizontalAlignment = HorizontalAlignment.Right;
                checkBox.Content = achivement;
                checkBox.IsChecked = achievement.Achived;
                checkBox.Style = myResourceDictionary["CheckBoxStyle"] as Style;
                checkBox.FontSize = 20;
                checkBox.Margin = new Thickness(0, 10, 0, 0);
                checkBox.Name = "booleanCheckBox";
                checkBox.Click += checkBox_MouseLeftButtonDown;

                grid.Children.Add(checkBox);
            }
            if (achievement.type == ReminderType.INTEGER)
            {
                TextBox textbox = new TextBox();
                textbox.HorizontalAlignment = HorizontalAlignment.Right;
                //checkBox.Style = myResourceDictionary["LabelStyle"] as Style;
                textbox.FontSize = 20;
                textbox.Margin = new Thickness(0, 2, 10, 0);
                textbox.Name = "integerTextBox";
                textbox.PreviewTextInput += NumberValidationTextBox;
                textbox.TextChanged += textBox_TextChanged;
                textbox.Width = 150;
                textbox.Height = 50;
                grid.Children.Add(textbox);
                textbox.Text = achievement.achivedInt.ToString();
            }

            SolidColorBrush myBrush;
            if (achievement.Achived || achievement.achivedInt > 0)
            {
                myBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                if (achievement.Required)
                {
                    myBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    myBrush = new SolidColorBrush(Colors.Yellow);
                }
            } 
            myBrush.Opacity = 0.1;
            grid.Background = myBrush;

            return grid;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Grid grid = (Grid)textBox.Parent;
            Achievement achievement = dailyAchievement.achievements.Find(x => x.Name.Equals(grid.Name));
            achievement.achivedInt = Int32.Parse(textBox.Text);
            SolidColorBrush myBrush;
            if (achievement.achivedInt > 0 )
            {
                myBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                myBrush = new SolidColorBrush(Colors.Red);
            }
            myBrush.Opacity = 0.1;
            grid.Background = myBrush;
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }

        private void checkBox_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Grid grid = (Grid)checkBox.Parent;
            Achievement achievement = dailyAchievement.achievements.Find(x => x.Name.Equals(grid.Name));
            achievement.Achived = !achievement.Achived;
            checkBox.IsChecked = achievement.Achived;
            SolidColorBrush myBrush;
            if (achievement.Achived)
            {
                myBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                if (achievement.Required)
                {
                    myBrush = new SolidColorBrush(Colors.Red);
                } else
                {
                    myBrush = new SolidColorBrush(Colors.Yellow);
                }
            }
            myBrush.Opacity = 0.1;
            grid.Background = myBrush;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            String name = ((Label)LogicalTreeHelper.FindLogicalNode(grid, "dateName")).Content.ToString();
            DragDrop.DoDragDrop(grid, name, DragDropEffects.Move);
        }

        private void updateVerticalPositionOfListElements()
        {
            int verticalPosition = 40;
            foreach (Grid elemet in ListOfAchivements)
            {
                elemet.Margin = new Thickness(10, verticalPosition, 10, 0);
                verticalPosition += 65;
            }
        }

        private void Rectangle_Drop(object sender, DragEventArgs e)
        {
            String gridName = (String)e.Data.GetData(DataFormats.UnicodeText);
            removeElement(gridName);
        }

        private void removeElement(String name)
        {
            foreach (Grid elemet in ListOfAchivements)
            {
                String elementName = ((Label)LogicalTreeHelper.FindLogicalNode(elemet, "dateName")).Content.ToString();
                if (elementName.Equals(name))
                {
                    ListOfAchivements.Remove(elemet);
                    this.AchievementsListGrid.Children.Remove(elemet);
                    return;
                }
            }

            updateVerticalPositionOfListElements();
        }

    }
}
