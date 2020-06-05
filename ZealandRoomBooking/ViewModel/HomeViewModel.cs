using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealandRoomBooking.Model;

namespace ZealandRoomBooking.ViewModel
{
    class HomeViewModel
    {
        public string LoggedInUserText { get; set; }
        public User RefUser = new User();

        public HomeViewModel()
        {
            SetUserText();
        }

        //Sætter Home textblock afhængig af bruger type, så man kan se hvilken bruger er logget ind
        public void SetUserText()
        {
            if (RefUser.CheckedUser.Usertype == "Elev")
            {
                LoggedInUserText = "Studerende";
            }
            else
            {
                LoggedInUserText = "Underviser";
            }
        }
    }
}
