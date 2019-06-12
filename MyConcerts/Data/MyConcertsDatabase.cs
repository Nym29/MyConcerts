using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyConcerts.Models;
using SQLite;
using SQLiteNetExtensionsAsync;

namespace MyConcerts.Data
{
    public class MyConcertsDatabase
    {
        private SQLiteAsyncConnection _conn;
        private SQLiteConnection _connNotAsync;
        
        public string StatusMessage { get; set; }

        public MyConcertsDatabase(string dbPath)
        {
            _conn = new SQLiteAsyncConnection(dbPath);
            _connNotAsync = new SQLiteConnection(dbPath);
            _conn.CreateTableAsync<Artist>().Wait();
            _conn.CreateTableAsync<ConcertDetails>().Wait();
            _conn.CreateTableAsync<ConcertSearch>().Wait();
            _conn.CreateTableAsync<User>().Wait();
        }

        public async Task AddNewConcertAsync(ConcertDetails concert)
        {
            int result = 0;
            try
            {
                if (concert.ToString()=="")
                    throw new Exception("Valid concert required");

                result = await _conn.InsertAsync(concert);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, concert);

            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
        }

        public async Task AddNewArtistAsync(Artist artist)
        {
            int result = 0;
            try
            {
                //basic validation to ensure a name was entered
                if (artist.ToString() == "")
                    throw new Exception("Valid venue required");

                result = await _conn.InsertAsync(artist);

                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, artist);

            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
        }

        public async Task<List<ConcertDetails>> GetAllConcertsAsync()
        {
            try
            {
                return await _conn.Table<ConcertDetails>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<ConcertDetails>();
        }

        public Artist GetArtist(int id)
        {
            return _connNotAsync.Table<Artist>()
                            .Where(i => i.ID == id)
                            .FirstOrDefault();
        }

        public Task<int> DeleteTrackedConcertAsync(ConcertDetails trackedConcert)
        {
            return _conn.DeleteAsync(trackedConcert);
        }

        public async Task AddNewNotesAsync(ConcertDetails concert)
        {
            int result = 0;
            try
            {
                //basic validation to ensure a name was entered
                if (concert.ToString() == "")
                    throw new Exception("Valid concert required");

                result = await _conn.UpdateAsync(concert);

                StatusMessage = string.Format("{0} record(s) added [concert: {1})", result, concert);

            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
        }

        public async Task UpdateConcertArtistIdAsync(ConcertDetails concert)
        {
            int result = 0;
            try
            {
                //basic validation to ensure a name was entered
                if (concert.ToString() == "")
                    throw new Exception("Valid concert required");

                result = await _conn.UpdateAsync(concert);

                StatusMessage = string.Format("{0} record(s) update [concert: {1})", result, concert);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
        }

        public async Task AddNewConcertSearchAsync(ConcertSearch concertSearch)
        {
            int result = 0;
            try
            {
                //basic validation to ensure a name was entered
                if (concertSearch.ToString() == "")
                    throw new Exception("Valid search required");

                result = await _conn.InsertAsync(concertSearch);

                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, concertSearch);

            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
        }

        public async Task<List<ConcertSearch>> GetAllSavedSearchedAsync()
        {
            try
            {
                return await _conn.Table<ConcertSearch>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
            return new List<ConcertSearch>();
        }

        public Task<int> DeleteConcertSearchAsync(ConcertSearch concertSearch)
        {
            return _conn.DeleteAsync(concertSearch);
        }

        public async Task AddNewSetlistAsync(ConcertDetails concert)
        {
            int result = 0;
            try
            {
                //basic validation to ensure a name was entered
                if (concert.ToString() == "")
                    throw new Exception("Valid concert required");

                result = await _conn.UpdateAsync(concert);

                StatusMessage = string.Format("{0} record(s) added [concert: {1})", result, concert);

            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
        }

        public async Task CreateUser(User user) 
        {
            int result = 0;
            try
            {
                //basic validation to ensure a name was entered
                if (user.ToString() == "")
                    throw new Exception("Valid venue required");

                result = await _conn.InsertAsync(user);

                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, user);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }
        }

        public User GetUser(string email, string password) 
        {
            var user = _connNotAsync.Table<User>()
                                    .Where(u => u.Email == email && u.Password == password)
                                    .FirstOrDefault();
            return user;
        }

    }
}
