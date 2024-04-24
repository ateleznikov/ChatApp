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

namespace ChatApp.Frames
{
    /// <summary>
    /// Interaction logic for ProfileDetails.xaml
    /// </summary>
    public partial class ProfileDetails : Page
    {
        public ProfileDetails()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Profile profile = new Profile();
            profile.Name = "John";
            profile.Surname = "Doe";
            profile.BIO = "Hello, I am John Doe";
            profile.ProfilePicture = "C:\\ChatApp\\ChatApp\\images\\profile-picture.jpg";

            // Set the DataContext of the page to the profile object
            this.DataContext = profile;

            
          
        }

    }
}
