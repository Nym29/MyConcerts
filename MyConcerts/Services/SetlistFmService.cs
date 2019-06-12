using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyConcerts.Models;
using Newtonsoft.Json.Linq;

namespace MyConcerts.Services
{
    public class SetlistFmService
    {
        string strResponseValue = string.Empty;
        private string _apiKey = Keys.SetlistFmApiKey;
        private Setlist setlist;

        public SetlistFmService()
        {

        }

        public async Task<string> FetchAllSetlistsAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.Accept = "application/json";
            request.Method = "GET";
            request.Headers.Add("x-api-key", _apiKey);

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {

                // Get a stream representation of the HTTP web response:
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                    // Return the JSON document:
                    return strResponseValue;
                }
            }
        }

        public IEnumerable<Setlist> GetAllSetlists(string reponse) 
        {

            JObject o = JObject.Parse(reponse);

            JArray a = (JArray)o["setlist"];

            IList<Setlist> setlists = a.Select(s => new Setlist
            {

                Name = (string)s["artist"]["name"],
                Venue = (string)s["venue"]["name"],
                Date = (string)s["eventDate"],
                Sets = (JObject)s["sets"]
            }).ToList();

            return setlists;
        }

        public Setlist GetSetlist(IEnumerable<Setlist> setlists, string date, string venue) 
        {
            foreach (var item in setlists)
            {
                if (item.Date == date && item.Venue == venue)
                {
                    setlist = new Setlist()
                    {
                        Name = item.Name,
                        Venue = item.Venue,
                        Date = item.Date,
                        Sets = item.Sets
                    };
                }
            }
            return setlist;
        }

        public string GetSongs(JObject sets, Setlist setlist)
        {
            //setlist.Songs = new List<string>();
            string pattern = @":.""([A-Za-z\s\|])+";
            MatchCollection matchList = Regex.Matches(sets.ToString(),pattern);
            var list = matchList.Cast<Match>().Select(match => match.Value.Substring(3)).ToList();
            
            foreach (var song in list) 
            {
                //setlist.Songs.Add(song); 
                setlist.Songs = setlist.Songs + song + ", ";
            }
            return setlist.Songs;
        }
    }
}
