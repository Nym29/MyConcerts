using System.ComponentModel;
using Xamarin.Forms;

namespace MyConcerts
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

            var trackedConcertsPage = new NavigationPage(new TrackedConcertsPage());
            trackedConcertsPage.Title = "Concerts";
            trackedConcertsPage.Icon = "speakers.png";

            var savedSearchesPage = new NavigationPage(new SavedSearchesPage());
            savedSearchesPage.Title=("Saved Searches");
            savedSearchesPage.Icon=("bookmark.png");

            var profilePage = new NavigationPage(new ProfilePage());
            if (App.IsLoggedIn == false) 
            {
                profilePage.Title = "Profile";
            }
            else 
            {
                profilePage.Title = App.LoggedInUser.Email;
            }
            profilePage.Icon = "profile.png";


            this.Children.Add(new SearchPage());
            this.Children.Add(trackedConcertsPage);
            this.Children.Add(savedSearchesPage);
            this.Children.Add(profilePage);
        }


    }
}
