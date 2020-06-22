using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelKlasser;

namespace DBContext
{
    public class ManageLokaler : IManageLokaler
    {

        public const string DBaddress = "Server=tcp:zealandroombooking.database.windows.net,1433;Initial Catalog=ZealandRoomBooking;Persist Security Info=False;User ID=Detdetjojaja;Password=Detdetjo42;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public List<Lokaler> LokaleList = new List<Lokaler>();

        public List<Lokaler> GetAllLokaler()
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = "SELECT * FROM Lokaler";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);    
                    int etage = reader.GetInt32(1);
                    string type = reader.GetString(2);
                    string navn = reader.GetString(3);
                    string bygning = reader.GetString(4);



                    Lokaler addLokale = new Lokaler() {LokaleId = id, Etage = etage, Type = type, Navn = navn, Bygning = bygning};
                    LokaleList.Add(addLokale);
                }
                connection.Close();
                return LokaleList;
            }
        }

        public Lokaler GetLokalerFromId(int lokalerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = $"SELECT * FROM Lokaler WHERE LokaleId = {lokalerId}";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();
                Lokaler nyLokale = new Lokaler();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int etage = reader.GetInt32(1);
                    string type = reader.GetString(2);
                    string navn = reader.GetString(3);
                    string bygning = reader.GetString(4);


                    nyLokale.LokaleId = id;
                    nyLokale.Etage = etage;
                    nyLokale.Type = type;
                    nyLokale.Navn = navn;
                    nyLokale.Bygning = bygning;

                }
                connection.Close();
                return nyLokale;
            }
        }

        public bool CreateLokaler(Lokaler lokaler)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllLokaler().Contains(lokaler);
                if (!check)
                {
                    var querystring =
                        $"INSERT INTO Lokaler VALUES ({lokaler.Etage},'{lokaler.Type}','{lokaler.Navn}','{lokaler.Bygning}')";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public bool UpdateLokaler(Lokaler lokaler, int lokalerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllLokaler().Contains(lokaler);
                if (!check)
                {
                    var querystring = $"UPDATE Lokaler SET Etage = {lokaler.Etage}, Type = '{lokaler.Type}', Navn = '{lokaler.Navn}', Bygning = '{lokaler.Bygning}' WHERE LokaleId = {lokalerId}";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public Lokaler DeleteLokaler(int lokalerId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetLokalerFromId(lokalerId);
                if (check != null)
                {
                    var querystring =
                        $"DELETE FROM Lokaler WHERE LokaleId = {lokalerId}";
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
