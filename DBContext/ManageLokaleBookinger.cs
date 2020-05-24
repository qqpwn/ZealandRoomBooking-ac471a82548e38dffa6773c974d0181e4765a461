using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelKlasser;

namespace DBContext
{
   public class ManageLokaleBookinger : IManageLokaleBookinger
   {

       public const string DBaddress = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ZealandDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<LokaleBookinger> LokaleBookingerList = new List<LokaleBookinger>();

        public List<LokaleBookinger> GetAllLokaleBookinger()
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = "SELECT * FROM LokaleBookinger";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int id2 = reader.GetInt32(1);
                    int id3 = reader.GetInt32(2);
                    

                    LokaleBookinger addLokaleBooking = new LokaleBookinger() { LBId = id, BookingId = id2, LokaleId = id3};
                    LokaleBookingerList.Add(addLokaleBooking);
                }
                connection.Close();
                return LokaleBookingerList;
            }
        }

        public LokaleBookinger GetLokaleBookingerFromId(int lokaleBookingerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = $"SELECT * FROM LokaleBookinger WHERE LBId = {lokaleBookingerId}";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();
                LokaleBookinger nyLokaleBookinger = new LokaleBookinger();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int id2 = reader.GetInt32(1);
                    int id3 = reader.GetInt32(2);


                    nyLokaleBookinger.LBId = id;
                    nyLokaleBookinger.BookingId = id2;
                    nyLokaleBookinger.LokaleId = id3;
                }
                connection.Close();
                return nyLokaleBookinger;
            }
        }

        public bool CreateLokaleBookinger(LokaleBookinger lokaleBookinger)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllLokaleBookinger().Contains(lokaleBookinger);
                if (!check)
                {
                    var querystring =
                    $"INSERT INTO LokaleBookinger VALUES ({lokaleBookinger.BookingId},{lokaleBookinger.LokaleId})";

                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public bool UpdateLokaleBookinger(LokaleBookinger lokaleBookinger, int lokaleBookingerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllLokaleBookinger().Contains(lokaleBookinger);
                if (!check)
                {
                    var querystring = $"UPDATE LokaleBookinger SET BookingId = {lokaleBookinger.BookingId}, LokaleId = {lokaleBookinger.LokaleId} WHERE LBId = {lokaleBookingerId}";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public LokaleBookinger DeleteLokaleBookinger(int lokaleBookingerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetLokaleBookingerFromId(lokaleBookingerId);
                if (check != null)
                {
                    var querystring =
                        $"DELETE FROM LokaleBookinger WHERE LBId = {lokaleBookingerId}";
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
