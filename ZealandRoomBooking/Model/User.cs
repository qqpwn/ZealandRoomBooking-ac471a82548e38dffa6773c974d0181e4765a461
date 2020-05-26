using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactions.Core;
using ZealandRoomBooking.Annotations;
using ZealandRoomBooking.Persistency;
using ZealandRoomBooking.View;

namespace ZealandRoomBooking.Model
{
    public class User 
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Usertype { get; set; }
        public User CheckedUser
        {
            get { return CheckedUserInfo;}

        }

        public User()
        {
        }

        private static readonly List<User> _alleUsers = new List<User>();

        public static List<User> MineUsers
        {
            get { return _alleUsers;}

        }
        public static string InputUsername { get; set; }
        public static string InputPassword { get; set; }

        
        public int LoginInt = 0; //0 = findes ikke, 1 = Elev, 2 = Lærer

        public static User CheckedUserInfo; //Info for den bruger som er logget ind

        //Checker om brugeren findes i databasen ved hjælp af en predicate og navigere til Home siden hvis de findes
        public async void CheckLogin()
        {
            await PersistencyService<User>.GetObjects("User");
            ObservableCollection<User> test = PersistencyService<User>.HentCollection;
            foreach (var o in test)
            {
                _alleUsers.Add(o);
            }


            if (_alleUsers.Find(_userPredicate) != null && CheckedUserInfo.Usertype == SearchElev)
            {
                LoginInt = 1;
                ((Frame)Window.Current.Content).Navigate(typeof(View.Home));
                
            }
            else if (_alleUsers.Find(_userPredicate) != null && CheckedUserInfo.Usertype == SearchLære)
            {
                LoginInt = 2;
                ((Frame)Window.Current.Content).Navigate(typeof(View.Home));

            }
            else
            {
                LoginInt = 0;
                
            }

        }

        private readonly Predicate<User> _userPredicate = new Predicate<User>(LoginSearch);

        public static string SearchElev = "Elev";
        public static string SearchLære = "Lære";
        

        public static bool LoginSearch(User user)
        {
            foreach (var o in MineUsers)
            {
                if (o.Username == InputUsername && o.Password == InputPassword)
                {
                    CheckedUserInfo = o;
                    return true;
                }
            }
            return false;
        }

    }
}
