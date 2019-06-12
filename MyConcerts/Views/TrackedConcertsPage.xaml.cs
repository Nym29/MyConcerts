using System;
using System.Collections.Generic;
using System.Linq;
using MyConcerts.Models;
using MyConcerts.Views;
using Xamarin.Forms;

namespace MyConcerts
{
    public partial class TrackedConcertsPage : ContentPage
    {
        private List<ConcertDetails> trackedConcerts ;

        public TrackedConcertsPage()
        {
            InitializeComponent();           
        }

        protected override async void OnAppearing()
        {
            if (App.IsLoggedIn) 
            { 
            base.OnAppearing();

            errorImg.IsVisible = false;
            errorLabel1.IsVisible = false;
            errorLabel2.IsVisible = false;

            upcomingConcertsLabel.IsVisible = true;
            UpcomingConcertsListView.IsVisible = true;
            pastConcertsLabel.IsVisible = true;
            PastConcertsListView.IsVisible = true;
            
            trackedConcerts = await App.ConcertsDB.GetAllConcertsAsync();
            var upcomingConcerts = trackedConcerts.Where(c => c.Date >= DateTime.Today).Select(c => c).OrderBy(c => c.Date).ToList();
            var pastConcerts = trackedConcerts.Where(c => c.Date < DateTime.Today).Select(c => c).OrderBy( c => c.Date).ToList();
            UpcomingConcertsListView.ItemsSource = upcomingConcerts;
            PastConcertsListView.ItemsSource = pastConcerts;
            }
            else if(!App.IsLoggedIn)
            {
                errorImg.IsVisible = true;
                errorLabel1.IsVisible = true;
                errorLabel2.IsVisible = true;

                upcomingConcertsLabel.IsVisible = false;
                UpcomingConcertsListView.IsVisible = false;
                pastConcertsLabel.IsVisible = false;
                PastConcertsListView.IsVisible = false;
            }
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var concertSelected = (ConcertDetails)e.Item;
            await this.Navigation.PushAsync(new TrackedConcertDetailsPage(concertSelected));
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Tracked Concert ", mi.CommandParameter + " delete tracked concert", "OK","Cancel");

            var trackedConcert = mi.CommandParameter;
            App.ConcertsDB.DeleteTrackedConcertAsync((MyConcerts.Models.ConcertDetails)trackedConcert);
            Navigation.PopAsync();
        }
    }
}
