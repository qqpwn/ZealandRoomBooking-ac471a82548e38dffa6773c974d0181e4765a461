using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Windows.AI.MachineLearning;
using ZealandRoomBooking.Annotations;
using ZealandRoomBooking.Model;
using ZealandRoomBooking.Persistency;

namespace ZealandRoomBooking.ViewModel
{
   public class ViewModel
    {
        public User RefUser { get; set; }

        //Imput brugerinfo
        public string Username
        {
           set { User.InputUsername = value; }
        }
        public string Password {
           
            set { User.InputPassword = value; }
        }

        public ViewModel()
        {
            
        }


        //Login knap binding
        public void CheckLoginMethode()
        {
            RefUser = new User();
            RefUser.CheckLogin();
        }
    }
}
