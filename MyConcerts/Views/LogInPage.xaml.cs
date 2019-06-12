using System;
using System.Collections.Generic;
using System.Net.Http;
using MyConcerts.Data;
using MyConcerts.Models;
using Newtonsoft.Json;
using RestSharp;
using Xamarin.Forms;

namespace MyConcerts.Views
{
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();
        }

        async void OnLogInButtonClicked(object sender, EventArgs e) 
        {
            //var token = await DependencyService.Get<IFirebaseAuthenticator>().LoginWithEmailPassword(email.Text, password.Text);
            var user = App.ConcertsDB.GetUser(emailInput.Text, passwordInput.Text);
            if(user !=null)
            {
                await DisplayAlert("Logged In", "You're Now Logged In as " + emailInput.Text, "Dismiss");
                App.LoggedInUser = user;
                App.IsLoggedIn = true;
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Unknown User", "The user " + emailInput.Text + " does not exist. Please sign up first.", "Dismiss");
                emailInput.Text = "";
                passwordInput.Text = "";
            }
        }

        protected override async void OnAppearing()
        {
            if (App.IsLoggedIn == true)
            {
                emailInput.Text = App.LoggedInUser.Email;
                emailInput.IsEnabled = false;
                passwordInput.IsEnabled = false;
                logInBtn.IsEnabled = false;
                signUpBtn.IsEnabled = false;
            }
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e) 
        {

            var user = App.ConcertsDB.GetUser(emailInput.Text, passwordInput.Text);
            if (user != null)
            {
                await DisplayAlert("Logged In", "This account already exists. You're Now Logged In as " + user.Email, "Dismiss");
                App.LoggedInUser = user;
                App.IsLoggedIn = true;
                await Navigation.PopAsync();
            }
            else
            {
                if (emailInput.Text != null && passwordInput.Text != null) 
                {
                    user = new User()
                    {
                        Email = emailInput.Text,
                        Password = passwordInput.Text
                    };
                    await App.ConcertsDB.CreateUser(user);
                    App.ConcertsDB.GetUser(user.Email, user.Password);
                    await DisplayAlert("Account created", "You're Now Signed In as " + emailInput.Text, "Dismiss");
                    App.LoggedInUser = user;
                    App.IsLoggedIn = true;
                    await Navigation.PopAsync();
                }
                else if (emailInput.Text == null) 
                {
                    await DisplayAlert("Error", "Username cannot be empty ", "Dismiss");
                }
                else if (passwordInput.Text == null)
                {
                    await DisplayAlert("Error", "Password cannot be empty ", "Dismiss");
                }

            }
        }

    }

}
