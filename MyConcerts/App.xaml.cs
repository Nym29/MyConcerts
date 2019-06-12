using System;
using System.IO;
using MyConcerts.Data;
using MyConcerts.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyConcerts
{
    public partial class App : Application
    {
        //public static TrackedConcertsRepository ConcertsRepo { get; private set; }
        public static MyConcertsDatabase concertsDB;
        public static bool IsLoggedIn { get; set; }
        public static User LoggedInUser { get; set; }

        public static MyConcertsDatabase ConcertsDB
        {
            get
            {
                if (concertsDB == null)
                {
                    concertsDB = new MyConcertsDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyConcerts.db3"));
                    //concertsDB = new MyConcertsDatabase("/Users/uriell/Desktop/MyConcerts.db3");
                }
                return concertsDB;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }
}
