using System;
using System.Collections.Generic;
using MyConcerts.Models;
using MyConcerts.Views;
using Xamarin.Forms;

namespace MyConcerts
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (App.IsLoggedIn == false)
            {
                userNameLabel.Text = "Unknown User";

            }
            else
            {
                userNameLabel.Text = App.LoggedInUser.Email;
            }
        }

            async void OnTrackedConcertsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrackedConcertsPage());
        }

        async void OnSavedSearchesButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedSearchesPage());
        }

        async void OnPreferencesButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PreferencesPage());
        }


        async void OnLogInButtonClicked(object sender, EventArgs e) 
        {
            await Navigation.PushAsync(new LogInPage());
        }

        async void OnLogOutButtonClicked(object sender, EventArgs e) 
        {
            if (App.IsLoggedIn == true) 
            { 
                App.IsLoggedIn = false;
                App.LoggedInUser = null;
                await DisplayAlert("Logged Out", "You're Now Logged out ", "Dismiss");
            }
            else 
            {
                await DisplayAlert("Error", "You're not Logged in ", "Dismiss");
            }

        }

    }
}
