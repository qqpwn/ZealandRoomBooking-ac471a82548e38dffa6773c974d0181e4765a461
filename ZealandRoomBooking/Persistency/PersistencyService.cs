using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ZealandRoomBooking.Persistency
{
   public class PersistencyService 
    {
        public static ObservableCollection<object> HentObjectsCollection = new ObservableCollection<object>();
        private const string ServerUri = "http://localhost:55911/";

        public static HttpClientHandler MyClientHandler()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                UseDefaultCredentials = true
            };
            return handler;
        }

        public static async Task<ObservableCollection<object>> GetObjects(object obj)
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
                        ObservableCollection<object> hentObject = await getobject.Content.ReadAsAsync<ObservableCollection<object>>();
                        foreach (var o in hentObject)
                        {
                            HentObjectsCollection.Add(o);
                        }
                    }

                    return HentObjectsCollection;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        
        public static async Task<object> GetObjectFromId(int objectId,object obj)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                try
                {
                    ObservableCollection<object> hentEtObject = new ObservableCollection<object>();
                    var getobject = await client.GetAsync($"api/{obj}/{objectId}");
                    if (getobject.IsSuccessStatusCode)
                    {
                        var hentObject = await getobject.Content.ReadAsAsync<object>();
                        foreach (var o in hentEtObject)
                        {
                           hentEtObject.Add(o);
                        }
                    }

                    return hentEtObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static async void PostObject(object obj)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postobject = await client.PostAsJsonAsync($"api/{obj}", obj);
            }
        }

        public static async void PutObject(int objectId, object obj)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var putobject = await client.PutAsJsonAsync($"api/{obj}/{objectId}", obj);
            }
        }

        public static async void DeleteObject(int objectId, object obj)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var deleteobject = await client.DeleteAsync($"api/{obj}/{objectId}");
            }
        }

    }
}
