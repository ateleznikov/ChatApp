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
using System.Data.Entity;

namespace ChatApp.Frames
{
    /// <summary>
    /// Interaction logic for ProfileDetails.xaml
    /// </summary>
    /// 

    public partial class ProfileDetails : Page
    {
        private ChatAppContext db;

        public ProfileDetails()
        {
            InitializeComponent();
            db = new ChatAppContext();
            LoadProfileData();
        }

        private void LoadProfileData()
        {
            int profileId = 1;
            var profile = db.Profiles.FirstOrDefault(p => p.Id == profileId);

            if (profile != null)
            {
                ImageBlock.Source = new BitmapImage(new System.Uri(profile.ProfilePicture, System.UriKind.RelativeOrAbsolute));
                NameBlock.Text = profile.Name;
                SurnameBlock.Text = profile.Surname;
                BioBlock.Text = profile.Bio;
            }
        }
    }
}
