using System;
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
    public class TicketmasterService
    {
        string strResponseValue = string.Empty;

        public TicketmasterService()
        {
        }

        public async Task<string> FetchConcertAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

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

        public IEnumerable<ConcertDetails> ParseJsonResponse(string json)
        {
            try { 
            JObject o = JObject.Parse(json);

            JArray a = (JArray)o["_embedded"]["events"];
            IList<ConcertDetails> concertsSorted = new List<ConcertDetails>();

            IList<ConcertDetails> concerts = a.Select(c => new ConcertDetails {

                EventName = (string)c["name"],
                Location = (string)c["_embedded"]["venues"][0]["city"]["name"],
                Venue = (string)c["_embedded"]["venues"][0]["name"],
                Date = (System.DateTime)c["dates"]["start"]["localDate"],
                //Time =(System.DateTime)c["dates"]["start"]["localTime"],
                ImageUrl = (string)c["images"][0]["url"],
                BuyTicketsUrl = (string)c["url"],
                Artist = new Artist() { Name = (string)c["name"], 
                Tmid = (string)c["_embedded"]["attractions"][0]["id"],
                Mbid = (string)c["_embedded"]["attractions"][0].ToString()
                }
            }
            )
            .ToList();

            string pattern = @"musicbrainz"":\s.\s*.\s*""id"":\s""(.*)""";
            foreach(var concert in concerts) 
            {
                concert.Time = "20:00";
                Match match = Regex.Match(concert.Artist.Mbid, pattern);
                var mbid = match.Groups[1].ToString();
                if(mbid!= null)
                {
                    concert.Artist.Mbid = mbid;
                }
                else 
                {
                    concert.Artist.Mbid = "";
                }
            }

            concertsSorted = concerts.OrderBy(c => c.Date).ToList();

            return concertsSorted;
            }
            catch (Exception ex) 
            {
                
                throw;
            }
        }
    }
}
