using System;
using System.Collections.Generic;
using MyConcerts.Models;
using MyConcerts.Views;
using Xamarin.Forms;

namespace MyConcerts
{
    public partial class SavedSearchesPage : ContentPage
    {
        private IList<ConcertSearch> savedSearches;

        public SavedSearchesPage()
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

                savedSearchLabel.IsVisible = true;
                SavedSearchesListView.IsVisible = true;

                savedSearches = await App.ConcertsDB.GetAllSavedSearchedAsync();
                SavedSearchesListView.ItemsSource = savedSearches;
            }
            else if (!App.IsLoggedIn)
            {
                errorImg.IsVisible = true;
                errorLabel1.IsVisible = true;
                errorLabel2.IsVisible = true;

                savedSearchLabel.IsVisible = false;
                SavedSearchesListView.IsVisible = false;
            }
        }

        async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var searchSelected = (ConcertSearch)e.Item;
            await this.Navigation.PushAsync(new SearchPage(searchSelected.Location,searchSelected.MusicStyle,searchSelected.StartDate.ToString("yyyy-MM-dd"), searchSelected.EndDate.ToString("yyyy-MM-dd")));
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Saved Search ", mi.CommandParameter + " delete saved search", "OK", "Cancel");

            var savedSearch = mi.CommandParameter;
            App.ConcertsDB.DeleteConcertSearchAsync((MyConcerts.Models.ConcertSearch)savedSearch);
            Navigation.PopAsync();
        }
    }
}
