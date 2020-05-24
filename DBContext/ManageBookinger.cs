using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelKlasser;

namespace DBContext
{
    public class ManageBookinger : IManageBookinger
    {

        public const string DBaddress = "Server=tcp:zealandroombookingdb.database.windows.net,1433;Initial Catalog=ZealandRoomBooking;Persist Security Info=False;User ID=Maxi123;Password=Maximilian123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<Bookinger> BookingList = new List<Bookinger>();

        public List<Bookinger> GetAllBookinger()
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = "SELECT * FROM Bookinger";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    DateTime dato = reader.GetDateTime(1);
                    int userid = reader.GetInt32(2);
                    

                    Bookinger addBookinger = new Bookinger() { BookingId = id, Date = dato, UserId = userid};
                    BookingList.Add(addBookinger);
                }
                connection.Close();
                return BookingList;
            }
        }

        public Bookinger GetBookingerFromId(int bookingerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = $"SELECT * FROM Bookinger WHERE BookingId = {bookingerId}";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();
                Bookinger nyBookinger = new Bookinger();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    DateTime dato = reader.GetDateTime(1);
                    int userid = reader.GetInt32(2);

                    nyBookinger.BookingId = id;
                    nyBookinger.Date = dato;
                    nyBookinger.UserId = userid;

                }
                connection.Close();
                return nyBookinger;
            }
        }

        public bool CreateBookinger(Bookinger bookinger)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllBookinger().Contains(bookinger);
                if (!check)
                {
                    var querystring =
                        $"INSERT INTO Bookinger VALUES ('{bookinger.DateString}',{bookinger.UserId})";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public bool UpdateBookinger(Bookinger bookinger, int bookingerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllBookinger().Contains(bookinger);
                if (!check)
                {
                    var querystring = $"UPDATE Bookinger SET Date = '{bookinger.Date}', UserId = {bookinger.UserId} WHERE BookingId = {bookingerId}";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public Bookinger DeleteBookinger(int bookingerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetBookingerFromId(bookingerId);
                if (check != null)
                {
                    var querystring =
                        $"DELETE FROM Bookinger WHERE BookingId = {bookingerId}";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }
    }
}
