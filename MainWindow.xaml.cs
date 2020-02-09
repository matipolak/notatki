using Notatki.modelCode;
using Notatki.modelCode.Achievement;
using Notatki.modelCode.Reminder;
using Notatki.viewHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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

        //LEFT HISTORY
        public List<Grid> ListOfDailyAchivements = new List<Grid>();
        ResourceDictionary myResourceDictionary = new ResourceDictionary
        {
            Source = new Uri("Dictionary1.xaml", UriKind.Relative)
        };

        /// RIGHT REMINDERS
        public DailyAchievements actualAchievements;
        public List<Grid> ListOfAchivements = new List<Grid>();
        public MainWindow()
        {
            model.LoadModel();
            InitializeComponent();
            System.IO.Directory.CreateDirectory("achievements");

            WindowEnricher.enrichWindow(this);
            DateTime date = DateTime.Now;
            DateTime lastUpdateDate = model.GetActualState().lastTimeUpdateReminders;
            while (!(date.Year == lastUpdateDate.Year && date.Month == lastUpdateDate.Month && date.Day == lastUpdateDate.Day - 1))
            {
                String fileName = "achievements/" + AchievementManager.DateToString(date)+".xml";
                if (File.Exists(fileName)) {
                    break;
                }
                DailyAchievements dailyAchievements = new DailyAchievements();
                dailyAchievements.date = date;
                dailyAchievements.achievements = model.GetActualState().DailyReminders.Select(x =>
                {
                    Achievement achievement = new Achievement();
                    achievement.Name = x.Name;
                    achievement.Achived = false;
                    achievement.type = x.type;
                    achievement.achivedInt = 0;
                    achievement.Required = isDateRequired(date, x);
                    return achievement;
                }).ToList();
                AchievementManager.SaveAchievement(fileName, dailyAchievements);
                date = date.AddDays(-1);
            }

            date = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                DailyAchievements dailyAchievements = AchievementManager.LoadAchievementl(date);

                Grid newElement = createHistoryElementList(dailyAchievements);
                ListOfDailyAchivements.Add(newElement);
                this.HistoryGrid.Children.Add(newElement);
                date = date.AddDays(-1);
            }

            updateVerticalPositionOfListElements(ListOfDailyAchivements, 60);


            date = DateTime.Now;
            this.actualAchievements = AchievementManager.LoadAchievementl(date);

            for (int i = 0; i < actualAchievements.achievements.Count; i++)
            {
                if((!actualAchievements.achievements[i].Achived) && actualAchievements.achievements[i].achivedInt == 0)
                {
                    Grid newElement = createAchivementElementList(actualAchievements.achievements[i]);
                    ListOfAchivements.Add(newElement);
                    this.ReminderGrid.Children.Add(newElement);
                }
            }
            updateVerticalPositionOfListElements(ListOfAchivements, 120);
        }

        public void refreshDailyAchivement(DateTime date)
        {
            DailyAchievements dailyAchievements = AchievementManager.LoadAchievementl(date);
            Grid grid = ListOfDailyAchivements.Find(x =>
                ((Label)LogicalTreeHelper.FindLogicalNode(x, "dateName")).Content.ToString().Equals(AchievementManager.DateToString(dailyAchievements.date))
            );

            refreshDailyAchivement(dailyAchievements, grid);
        }

        public void refreshDailyAchivement(DailyAchievements dailyAchievements, Grid grid)
        {
            int RequierCount = AchievementAnalizer.getRequierCount(dailyAchievements);
            int AchivedRequier = AchievementAnalizer.getAchivedRequierCount(dailyAchievements);

            String achivementLabelContent = AchivedRequier.ToString() + "/"
                 + RequierCount.ToString() + "     " +
                 AchievementAnalizer.getAchivedCount(dailyAchievements).ToString() + "/"
                + AchievementAnalizer.getCount(dailyAchievements).ToString();

            ((Label)LogicalTreeHelper.FindLogicalNode(grid, "achivementLabel")).Content = achivementLabelContent;
            SolidColorBrush myBrush;
            if ((AchivedRequier == RequierCount) && RequierCount != 0)
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


        public Boolean isDateRequired(DateTime date, DailyReminder dailyReminder)
        {
            if (date.DayOfWeek == DayOfWeek.Monday && dailyReminder.Monday)
            {
                return true;
            }
            if (date.DayOfWeek == DayOfWeek.Tuesday && dailyReminder.Tuesday)
            {
                return true;
            }
            if (date.DayOfWeek == DayOfWeek.Wednesday && dailyReminder.Wednesday)
            {
                return true;
            }
            if (date.DayOfWeek == DayOfWeek.Thursday && dailyReminder.Thursday)
            {
                return true;
            }
            if (date.DayOfWeek == DayOfWeek.Friday && dailyReminder.Friday)
            {
                return true;
            }
            if (date.DayOfWeek == DayOfWeek.Saturday && dailyReminder.Saturday)
            {
                return true;
            }
            if (date.DayOfWeek == DayOfWeek.Sunday && dailyReminder.Sunday)
            {
                return true;
            }
            return false;

        }

        private void Edit_reminder_click(object sender, RoutedEventArgs e)
        {
            ReminderSettings win = new ReminderSettings(model.GetActualState().DailyReminders, model);
            win.Show();
        }

        public Grid createHistoryElementList(DailyAchievements dailyAchievements)
        {
            String name = AchievementManager.DateToString(dailyAchievements.date);

            Grid grid = new Grid();
            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Height = 60;
            
            Label label = new Label();
            label.Width = 180;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = name;
            label.Name = "dateName";
            label.Style = myResourceDictionary["LabelStyle"] as Style;
            label.FontSize = 20;
            label.Margin = new Thickness(0, 10, 0, 0);
            grid.Children.Add(label);

            Label achivementLabel = new Label();
            achivementLabel.HorizontalAlignment = HorizontalAlignment.Right;
            achivementLabel.Style = myResourceDictionary["LabelStyle"] as Style;
            achivementLabel.FontSize = 20;
            achivementLabel.Name = "achivementLabel";
            achivementLabel.Margin = new Thickness(0, 10, 0, 0);
            grid.Children.Add(achivementLabel);
            refreshDailyAchivement(dailyAchievements, grid);

            return grid;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = (Grid)sender;
            String elementName = ((Label)LogicalTreeHelper.FindLogicalNode(grid, "dateName")).Content.ToString();
            DailyAchievement win = new DailyAchievement(AchievementManager.StringToDate(elementName), this);
            win.Show();
        }

        private void updateVerticalPositionOfListElements(List<Grid> elements, int height)
        {
            int verticalPosition = 0;
            foreach (Grid elemet in elements)
            {
                elemet.Margin = new Thickness(10, verticalPosition, 10, 0);
                verticalPosition += height+5;
            }
        }

        private void Label3_Copy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(this.OptionalGrid.Visibility == Visibility.Visible)
            {
                this.Width = 230;
                this.OptionalGrid.Visibility = Visibility.Hidden;
                arrowButton.Content = "<";
                this.RemindersGrid.Margin = new Thickness(0, 10, 0, 0);
            } else
            {
                this.Width = 800;
                this.OptionalGrid.Visibility = Visibility.Visible;
                arrowButton.Content = ">";
                this.RemindersGrid.Margin = new Thickness(569, 10, 0, 0);
            }
            
        }

        public Grid createAchivementElementList(Achievement achievement)
        {
            String name = achievement.Name;
            String achivement = achievement.Required ? "Required" : "Optional";
            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Height = 120;
            grid.Name = name;

            Label label = new Label();
            label.Width = 180;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.Content = name;
            label.Name = "dateName";
            label.Style = myResourceDictionary["LabelStyle"] as Style;
            label.FontSize = 20;
            label.Margin = new Thickness(0, 10, 0, 0);
            grid.Children.Add(label);

            if (achievement.type == ReminderType.BOOLEAN)
            {
                Button buttonYes = new Button();
                buttonYes.HorizontalAlignment = HorizontalAlignment.Left;
                buttonYes.Content = "Done";
                //checkBox.Style = myResourceDictionary["LabelStyle"] as Style;
                buttonYes.FontSize = 20;
                buttonYes.Margin = new Thickness(5, 70, 0, 10);
                buttonYes.Name = "booleanCheckBox";
                buttonYes.Width = 85;
                buttonYes.Click += Done_Achivement_click;

                Button buttonNo = new Button();
                buttonNo.HorizontalAlignment = HorizontalAlignment.Left;
                buttonNo.Content = "Nope";
                //checkBox.Style = myResourceDictionary["LabelStyle"] as Style;
                buttonNo.FontSize = 20;
                buttonNo.Margin = new Thickness(90, 70, 0, 10);
                buttonNo.Name = "booleanCheckBox";
                buttonNo.Width = 85;
                buttonNo.Click += Nope_Achivement_click;

                grid.Children.Add(buttonYes);
                grid.Children.Add(buttonNo);
            }
            if (achievement.type == ReminderType.INTEGER)
            {
                TextBox textbox = new TextBox();
                textbox.HorizontalAlignment = HorizontalAlignment.Left;
                //checkBox.Style = myResourceDictionary["LabelStyle"] as Style;
                textbox.FontSize = 15;
                textbox.Margin = new Thickness(10, 70, 0, 10);
                textbox.Name = "integerTextBox";
                textbox.PreviewTextInput += NumberValidationTextBox;
                //textbox.TextChanged += textBox_TextChanged;
                textbox.Width = 85;
                grid.Children.Add(textbox);
                textbox.Text = achievement.achivedInt.ToString();

                Button buttonSubmit = new Button();
                buttonSubmit.HorizontalAlignment = HorizontalAlignment.Left;
                buttonSubmit.Content = "Save";
                //checkBox.Style = myResourceDictionary["LabelStyle"] as Style;
                buttonSubmit.FontSize = 20;
                buttonSubmit.Margin = new Thickness(90, 70, 0, 10);
                buttonSubmit.Name = "booleanCheckBox";
                buttonSubmit.Width = 85;
                buttonSubmit.Click += Submit_Achivement_click;
                grid.Children.Add(buttonSubmit);
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
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }

        private void Done_Achivement_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Parent;
            Achievement achievement = actualAchievements.achievements.Find(x => x.Name.Equals(grid.Name));
            achievement.Achived = true;
            String fileName = "achievements/" + AchievementManager.DateToString(actualAchievements.date) + ".xml";
            AchievementManager.SaveAchievement(fileName, actualAchievements);
            refreshDailyAchivement(actualAchievements.date);
            ListOfAchivements.Remove(grid);
            this.ReminderGrid.Children.Remove(grid);
            updateVerticalPositionOfListElements(ListOfAchivements, 120);
        }

        private void Nope_Achivement_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Parent;
            ListOfAchivements.Remove(grid);
            this.ReminderGrid.Children.Remove(grid);
            System.Timers.Timer timer = new System.Timers.Timer(60000*60);
            timer.Elapsed += (x, a) => OnTimedEvent(x, a, grid, this);
            timer.Start();
            this.updateVerticalPositionOfListElements(ListOfAchivements, 120);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e, Grid grid, MainWindow mainWindow)
        {
            mainWindow.ListOfAchivements.Add(grid);
            mainWindow.Dispatcher.Invoke(() => {
                ReminderGrid.Children.Add(grid);
                updateVerticalPositionOfListElements(mainWindow.ListOfAchivements, 120);
            });
            ((System.Timers.Timer)source).Stop();

        }

        private void Submit_Achivement_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Parent;
            Achievement achievement = actualAchievements.achievements.Find(x => x.Name.Equals(grid.Name));
            String mayInteger = ((TextBox)LogicalTreeHelper.FindLogicalNode(grid, "integerTextBox")).Text;

            achievement.achivedInt = Int32.Parse(mayInteger);
            String fileName = "achievements/" + AchievementManager.DateToString(actualAchievements.date) + ".xml";
            AchievementManager.SaveAchievement(fileName, actualAchievements);
            refreshDailyAchivement(actualAchievements.date);
            ListOfAchivements.Remove(grid);
            this.ReminderGrid.Children.Remove(grid);
            updateVerticalPositionOfListElements(ListOfAchivements, 120);
        }

    }
}
