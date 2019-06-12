using System;
using MyConcerts.Models;
using MyConcerts.Services;
using Xamarin.Forms;

namespace MyConcerts.Views
{
    public partial class TrackedConcertDetailsPage : ContentPage
    {
        Artist artist;
        string mbid;
        string venue;
        string date;
        string sets;

        public TrackedConcertDetailsPage(ConcertDetails concert)
        {
            BindingContext = concert;
            artist = App.ConcertsDB.GetArtist(concert.ArtistId);
            concert.Artist = artist;
            mbid = concert.Artist.Mbid;
            venue = concert.Venue;
            date = concert.Date.ToString("dd-MM-yyyy");
            InitializeComponent();
        }

        
        async void OnSaveNotesClicked(object sender, EventArgs e)
        {
            var concert = (ConcertDetails)BindingContext;
            await App.ConcertsDB.AddNewNotesAsync(concert);
        }

        async void OnAddSetlistClicked(object sender, EventArgs e)
        {
            string endpointSetlistFm = "https://api.setlist.fm/rest/1.0/artist/"+mbid +"/setlists";

            SetlistFmService setlistFmService = new SetlistFmService();

            var response = await setlistFmService.FetchAllSetlistsAsync(endpointSetlistFm);

            var allSetlists = setlistFmService.GetAllSetlists(response);

            var setlist = setlistFmService.GetSetlist(allSetlists,date,venue);

            var songs = setlistFmService.GetSongs(setlist.Sets, setlist);

            var concert = (ConcertDetails)BindingContext;
            concert.Setlist = songs;
            await App.ConcertsDB.AddNewSetlistAsync(concert);
            await DisplayAlert("Setlist", "The Setlist was successfully retrieved", "Dismiss");

        }

        protected override async void OnAppearing()
        {
            var concert = (ConcertDetails)BindingContext;
            if(concert.Date > DateTime.Today.AddDays(-1)) 
            {
                addSetlistBtn.IsVisible = false;
                setlistLabel.IsVisible = false;
            }
            if (concert.Setlist != null) 
            {
                addSetlistBtn.IsVisible = false;
            }
            else 
            {
                setlistLabel.IsVisible = false;
            }
        }
    }
}
