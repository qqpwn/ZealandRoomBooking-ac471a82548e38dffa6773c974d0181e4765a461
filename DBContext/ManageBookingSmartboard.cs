using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelKlasser;

namespace DBContext
{
    public class ManageBookingSmartboard : IManageBookingSmartboard
    {
        public const string DBaddress = "Server=tcp:zealandroombookingdb.database.windows.net,1433;Initial Catalog=ZealandRoomBooking;Persist Security Info=False;User ID=Maxi123;Password=Maximilian123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public List<BookingSmartboard> BookingSmartboardList = new List<BookingSmartboard>();

        public List<BookingSmartboard> GetAllBookingSmartboard()
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = "SELECT * FROM BookingSmartboard";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int id2 = reader.GetInt32(1);
                    int id3 = reader.GetInt32(2);
                    int id4 = reader.GetInt32(3);


                    BookingSmartboard addBookingSmartboard = new BookingSmartboard() { BSId = id, BookingId = id2, LokaleId = id3, TidId = id4};
                    BookingSmartboardList.Add(addBookingSmartboard);
                }
                connection.Close();
                return BookingSmartboardList;
            }
        }

        public BookingSmartboard GetBookingSmartboardFromId(int bookingSmartboardId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = $"SELECT * FROM BookingSmartboard WHERE BSId = {bookingSmartboardId}";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();
                BookingSmartboard nyBookingSmartboard = new BookingSmartboard();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int id2 = reader.GetInt32(1);
                    int id3 = reader.GetInt32(2);
                    int id4 = reader.GetInt32(3);

                    nyBookingSmartboard.BSId = id;
                    nyBookingSmartboard.BookingId = id2;
                    nyBookingSmartboard.LokaleId = id3;
                    nyBookingSmartboard.TidId = id4;
                }
                connection.Close();
                return nyBookingSmartboard;
            }
        }

        public bool CreateBookingSmartboard(BookingSmartboard bookingSmartboard)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllBookingSmartboard().Contains(bookingSmartboard);
                if (!check)
                {
                    var querystring =
                        $"INSERT INTO BookingSmartboard VALUES ({bookingSmartboard.BookingId},{bookingSmartboard.LokaleId},{bookingSmartboard.TidId})";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public bool UpdateBookingSmartboard(BookingSmartboard bookingSmartboard, int bookingSmartboardId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllBookingSmartboard().Contains(bookingSmartboard);
                if (!check)
                {
                    var querystring = $"UPDATE BookingSmartboard SET BookingId = {bookingSmartboard.BookingId}, LokaleId = {bookingSmartboard.LokaleId}, TidId = {bookingSmartboard.TidId} WHERE BSId = {bookingSmartboardId}";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public BookingSmartboard DeleteBookingSmartboard(int bookingSmartboardId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetBookingSmartboardFromId(bookingSmartboardId);
                if (check != null)
                {
                    var querystring =
                        $"DELETE FROM BookingSmartboard WHERE BSId = {bookingSmartboardId}";
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
