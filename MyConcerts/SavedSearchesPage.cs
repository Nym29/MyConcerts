using System;

using Xamarin.Forms;

namespace MyConcerts
{
    public class SavedSearchesPage : ContentPage
    {
        public SavedSearchesPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

