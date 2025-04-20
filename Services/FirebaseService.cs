using Firebase.Database;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using HelmetApiProject.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HelmetBackend.Services
{
    public class FirebaseService
    {
        private readonly IFirebaseClient _client;

        public FirebaseService()
        {
            var config = new FirebaseConfig
            {
                BasePath = "https://ahmedelngar5577-default-rtdb.firebaseio.com/"
            };
            _client = new FireSharp.FirebaseClient(config);
        }
        public async Task<Dictionary<string, T>> GetAllAsync<T>(string path)
        {
            var response = await _client.GetAsync(path);
            if (response.Body == "null") return new Dictionary<string, T>();
            return response.ResultAs<Dictionary<string, T>>();
        }
        public async Task<SetResponse> SetAsync<T>(string path, T data)
        {
            return await _client.SetAsync(path, data);
        }

        public async Task<PushResponse> PushAsync<T>(string path, T data)
        {
            return await _client.PushAsync(path, data);
        }
        public async Task<string> GenerateNewDriverId()
        {
            var lastIdResponse = await _client.GetAsync("DriverLastId");
            int lastId = lastIdResponse.ResultAs<int>();
            int newId = lastId + 1;

            // Update the last ID in Firebase
            await _client.SetAsync("DriverLastId", newId);

            return newId.ToString();
        }
        public async Task<AdminUser> GetUserAsync(string username)
        {
            return await GetAsync<AdminUser>($"admins/{username}");
        }

        public async Task<T> GetAsync<T>(string path)
        {
            var response = await _client.GetAsync(path);
            return response.ResultAs<T>();
        }
        public async Task DeleteAsync(string path)
        {
            await _client.DeleteAsync(path);
        }


    }
}
