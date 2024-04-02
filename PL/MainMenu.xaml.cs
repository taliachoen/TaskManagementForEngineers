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

        private void EngineerView_Click(object sender, RoutedEventArgs e)
        {
            new EnterEngineerWindow().Show();
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

    }
}
