using PL.Engineer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MainMenu()
        {
            InitializeComponent();
            CurrentTime = s_bl.Clock;
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime),
                typeof(MainMenu),
                new PropertyMetadata(DateTime.Now));

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }


        private void ManagerView_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }

        private void AdvanceDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceDay(1);
            CurrentTime = s_bl.Clock;
        }

        private void AdvanceHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceHour(1);
            CurrentTime = s_bl.Clock;
        }

        private void ResetClock_Click(object sender, RoutedEventArgs e)
        {
            s_bl.InitializeTime();
            CurrentTime = s_bl.Clock;
        }
        private void EngineerView_Click(object sender, RoutedEventArgs e)
        {
            // הצגת תיבת הקלט להזנת מספר תעודת זהות של המהנדס
            string engineerId = Microsoft.VisualBasic.Interaction.InputBox("הזן מספר תעודת זהות של המהנדס:", "מספר תעודת זהות", "");

            // בדיקה אם המספר שהוזן אינו ריק וניתן להמיר אותו למספר שלם
            if (!string.IsNullOrWhiteSpace(engineerId) && int.TryParse(engineerId, out int Id))
            {
                // בדיקה האם המהנדס קיים במערכת
                try
                {
                    //if (string.IsNullOrEmpty(Id))
                    //    throw new BO.BlInvalidDataException("Alias cannot be null or empty.");

                    var engineerExist = s_bl.Engineer.Read(Id);
                    if (s_bl.Task.IsCurrentTask(int.Parse(engineerId)))
                    {
                        new ShowEngineerTask(int.Parse(engineerId)).ShowDialog();
                    }
                    else
                    {
                        //רשימת משימות שניתן לבחור מהן משימה
                        new ListOfTask(int.Parse(engineerId)).ShowDialog();
                    }
                }
                catch
                {
                    MessageBox.Show("מספר תעודת הזהות שהוזן אינו קיים במערכת!", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("מספר תעודת זהות שגוי או לא הוזן!", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //האם יש משימה שהוא עובד עליה כרג


        }

    }
}
