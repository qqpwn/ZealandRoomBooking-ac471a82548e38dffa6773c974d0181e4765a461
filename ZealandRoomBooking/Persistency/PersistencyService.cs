using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZealandRoomBooking.Model;

namespace ZealandRoomBooking.Persistency
{
    public class PersistencyService<T> where T: class
    {
        public static ObservableCollection<T> HentCollection = new ObservableCollection<T>();
        public static ObservableCollection<T> HentEtObject = new ObservableCollection<T>();
        private const string ServerUri = "http://localhost:55911/";

        public static HttpClientHandler MyClientHandler()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true
            };
            return handler;
        }

        public static async Task<ObservableCollection<T>> GetObjects(string obj)
        {
            
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var getobject = await client.GetAsync($"api/{obj}");
                    if (getobject.IsSuccessStatusCode)
                    {
                        ObservableCollection<T> hentObject = await getobject.Content.ReadAsAsync<ObservableCollection<T>>();
                        foreach (var o in hentObject)
                        {
                            HentCollection.Add(o);
                        }
                    }

                    return HentCollection;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static async Task<ObservableCollection<T>> GetObjectFromId(int objectId, string obj)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    
                    var getobject = await client.GetAsync($"api/{obj}/{objectId}");
                    if (getobject.IsSuccessStatusCode)
                    {
                        var hentObject = await getobject.Content.ReadAsAsync<ObservableCollection<T>>();
                        foreach (var o in hentObject)
                        {
                            HentEtObject.Add(o);
                        }
                    }

                    return HentEtObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static async void PostObject(string objstring, T obj)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postobj = await client.PostAsJsonAsync($"api/{objstring}", obj);
            }
        }

        public static async void PutObject(int objectId, string objstring, T obj)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var putobject = await client.PutAsJsonAsync($"api/{objstring}/{objectId}", obj);
            }
        }

        public static async void DeleteObject(int objectId, string objstring)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var deleteobject = await client.DeleteAsync($"api/{objstring}/{objectId}");
            }
        }

    }
}
