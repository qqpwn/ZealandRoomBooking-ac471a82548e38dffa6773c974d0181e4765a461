using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using ZealandRoomBooking.Model;

namespace ZealandRoomBooking.Persistency
{
    public class PersistencyService<T> where T: class
    {
        public static ObservableCollection<T> HentCollection = new ObservableCollection<T>();
        public static T HentEtObject;
        private const string ServerUri = "https://wszealand20200622011108.azurewebsites.net/";

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
                        HentCollection.Clear();
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

        public static async Task<T> GetObjectFromId(int objectId, string obj)
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
                        
                        var hentObject = await getobject.Content.ReadAsAsync<T>();
                        HentEtObject = hentObject;
                    }

                    return HentEtObject;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        public static async Task<T> PostObject(string objstring, T obj)
        {
            using (var client = new HttpClient(MyClientHandler()))
            {
                client.BaseAddress = new Uri(ServerUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var postobj = await client.PostAsJsonAsync($"api/{objstring}", obj);
            }
            return obj;
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
