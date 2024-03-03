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
using System.Windows.Shapes;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerWindow(int engineerId = 0)
        {
            InitializeComponent();
            if (engineerId == 0)
            {
                // Create a new object with default values
                CurrentEngineer = new BO.Engineer()
                {
                    Id = 0,
                    Cost = null,
                    Email = null,
                    Level = 0,
                    Name = null
                };
            }
            else
            {
                try
                {
                    // Call the BL method to get the existing object by Id
                    CurrentEngineer = s_bl.Engineer.Read(engineerId);
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., engineer not found)
                    MessageBox.Show($"Error: {ex.Message}");
                    Close(); // Close the window if an error occurs
                }
            }
        }


        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("CurrentEngineer",
                typeof(BO.Engineer),
                typeof(EngineerWindow),
                new PropertyMetadata(null));




    }
}
