using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MyConcerts.Models;
using MyConcerts.Services;
using MyConcerts.Views;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MyConcerts
{
    public partial class SearchPage : ContentPage
    {

        readonly IList<ConcertDetails> concerts = new ObservableCollection<ConcertDetails>();
        private IList<ConcertDetails> concertsSorted = new ObservableCollection<ConcertDetails>();
        public string _locationParameter = string.Empty;
        public string _styleParameter = string.Empty;
        public string _startDateParameter = string.Empty;
        public string _endDateParamater = string.Empty;
        public DateTime FromMinimumDate = DateTime.Now;
        public DateTime ToMaximumDate = DateTime.Now.AddYears(1);
        private bool _firstLoad = true;
        private bool _fromSavedSearch;
        private string _apiKey = Keys.TicketmasterApiKey;

        public SearchPage()
        {
            BindingContext = concerts;
            InitializeComponent();
        }

        public SearchPage(string locationParameter,string styleParameter, string startDateParameter, string endDateParamater)
        {
            BindingContext = concerts;
            InitializeComponent();

            _locationParameter = locationParameter;
            _styleParameter = styleParameter;
            _startDateParameter = startDateParameter + "T00:00:00Z";
            _endDateParamater = endDateParamater + "T00:00:00Z";
            _firstLoad = false;
            locationInput.Text = locationParameter;
            styleInput.SelectedItem = styleParameter;
            startDateInput.Date = Convert.ToDateTime(startDateParameter);
            endDateInput.Date = Convert.ToDateTime(endDateParamater);
        }

        protected override async void OnAppearing()
        {
            if (_firstLoad == true) { 
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                var cities = await locator.GetAddressesForPositionAsync(position);
                var city = cities.FirstOrDefault();
                locationInput.Text = "Dublin";//city.Locality;

                startDateInput.Date = DateTime.Today;
                endDateInput.Date = DateTime.Today.AddDays(30);
                _locationParameter = "Dublin";//city.Locality;
                _startDateParameter = DateTime.Now.ToString("yyyy-MM-dd") + "T00:00:00Z";
                _endDateParamater = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd") + "T00:00:00Z";
                styleInput.SelectedItem = "Rock";
                GetConcerts();
                _firstLoad = false;
            }

        }

        async void OnSearchButtonClicked(object sender, EventArgs e) 
        {
            GetConcerts();
        }

        public async void GetConcerts() 
        {
            _locationParameter = locationInput.Text;
            string endPointTicketmaster = "https://app.ticketmaster.com/discovery/v2/events.json?apikey="+_apiKey+ "&city=" + _locationParameter;

            if (styleInput.SelectedItem.ToString() == string.Empty)
            {
                await DisplayAlert("Error", "Music style cannot be empty. Rock is selected by default. ", "Dismiss");
                _styleParameter = "";
                _styleParameter = "&classificationName=Rock";
                endPointTicketmaster = endPointTicketmaster + _styleParameter;
            }
            else
            {
                _styleParameter = "";
                _styleParameter = "&classificationName=" + styleInput.SelectedItem.ToString();
                endPointTicketmaster = endPointTicketmaster + _styleParameter;
            }

            if (_startDateParameter == null)
            {
                _startDateParameter = "&startDateTime=" + DateTime.Now.ToString("yyyy-MM-dd") + "T00:00:00Z";
                endPointTicketmaster = endPointTicketmaster + _startDateParameter;
            }
            else
            {
                _startDateParameter = "&startDateTime=" + startDateInput.Date.ToString("yyyy-MM-dd") + "T00:00:00Z";
                endPointTicketmaster = endPointTicketmaster + _startDateParameter;
            }

            if (_endDateParamater == null)
            {
                _endDateParamater = "&endDateTime=" + DateTime.Now.AddDays(30).ToString("yyyy-MM-dd") + "T00:00:00Z";
                endPointTicketmaster = endPointTicketmaster + _endDateParamater;
            }
            else
            {
                if (endDateInput.Date < startDateInput.Date) 
                {
                    await DisplayAlert("Error", "End date cannot be inferior to Start date. End date was changed to " +startDateInput.Date.AddDays(30).ToString("dd/MM/yyy") , "Dismiss");
                    _endDateParamater = "&endDateTime=" + startDateInput.Date.AddDays(30).ToString("yyyy-MM-dd") + "T00:00:00Z";
                    endPointTicketmaster = endPointTicketmaster + _endDateParamater;
                }
                else 
                { 
                _endDateParamater = "&endDateTime=" + endDateInput.Date.ToString("yyyy-MM-dd") + "T00:00:00Z";
                endPointTicketmaster = endPointTicketmaster + _endDateParamater;
                }
            }


            TicketmasterService ticketmasterService = new TicketmasterService();

            var response = await ticketmasterService.FetchConcertAsync(endPointTicketmaster);

            // Turn on network indicator
            this.IsBusy = true;

            try
            {
                if (concerts.Count > 0)
                {
                    concerts.Clear();
                }
                var ticketmasterConcerts = ticketmasterService.ParseJsonResponse(response);

                foreach (ConcertDetails concert in ticketmasterConcerts)
                {
                    concerts.Add(concert);
                }

            }
            finally
            {

                this.IsBusy = false;
                _locationParameter = string.Empty;
                _styleParameter = string.Empty;
                _startDateParameter = string.Empty;
                _endDateParamater = string.Empty;
            }

        }

        async void OnSaveButtonClicked(object sender, EventArgs e) 
        {
            var concertSearch = new ConcertSearch()
            {
                Location = locationInput.Text,
                MusicStyle = styleInput.SelectedItem.ToString(),
                StartDate = startDateInput.Date,
                EndDate = endDateInput.Date
            };

            await App.ConcertsDB.AddNewConcertSearchAsync(concertSearch);
            saveBtn.Text = "Search Saved!";
            saveBtn.BackgroundColor = Color.SlateBlue;
            saveBtn.Clicked -= OnSaveButtonClicked;
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var concertSelected = (ConcertDetails)e.Item;
            await this.Navigation.PushAsync(new ConcertDetailsPage(concertSelected));
        }
    }
}