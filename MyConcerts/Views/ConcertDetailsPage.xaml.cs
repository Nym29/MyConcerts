using System;
using MyConcerts.Models;
using Xamarin.Forms;

namespace MyConcerts.Views
{
    public partial class ConcertDetailsPage : ContentPage
    {
    
        private string concertSelectedTicketsUrl = string.Empty;

        public ConcertDetailsPage(ConcertDetails concert)
        {
            BindingContext = concert;
            concertSelectedTicketsUrl = concert.BuyTicketsUrl;
            InitializeComponent();
        }

        public void OnBuyTicketsClicked(object sender, EventArgs e) 
        {
            Device.OpenUri(new Uri(concertSelectedTicketsUrl));
        }

        public async void OnTrackConcertClicked(object sender, EventArgs e) 
        {
            if (App.IsLoggedIn == false)
            {
                await DisplayAlert("Error", "You need to be logged in to track a concert.", "Dismiss");
            }
            else if (App.IsLoggedIn == true) 
            { 
                var concert = (ConcertDetails)BindingContext;
                await App.ConcertsDB.AddNewArtistAsync(concert.Artist);
                await App.ConcertsDB.AddNewConcertAsync(concert);
                concert.ArtistId = concert.Artist.ID;
                concert.Artist = concert.Artist;
                await App.ConcertsDB.UpdateConcertArtistIdAsync(concert);
                trackConcertBtn.Text = "Concert Tracked!";
                trackConcertBtn.BackgroundColor =Color.SlateBlue;
                trackConcertBtn.Clicked -= OnTrackConcertClicked;
            }
        }


        protected override async void OnAppearing()
        {
            var alreadyTracked = false;
            var concertSelected = (ConcertDetails)BindingContext;
            var trackedConcerts = await App.ConcertsDB.GetAllConcertsAsync();
            foreach (var concert in trackedConcerts)
            {
                if(concert.EventName == concertSelected.EventName && concert.Location == concertSelected.Location && concert.Date == concertSelected.Date)
                {
                    alreadyTracked = true;
                }
            }

            if(alreadyTracked == true) 
            {
                trackConcertBtn.Text = "Concert Tracked!";
                trackConcertBtn.BackgroundColor = Color.SlateBlue;
                trackConcertBtn.Clicked -= OnTrackConcertClicked;
            }
        }
    }
}
