using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelKlasser;

namespace DBContext
{
    public class ManageTid : IManageTid
    {
        public const string DBaddress = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = ZealandDB; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public List<Tid> TidList = new List<Tid>();

        public List<Tid> GetAllTid()
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = "SELECT * FROM Tid";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    TimeSpan id2 = reader.GetTimeSpan(1);
                    TimeSpan id3 = reader.GetTimeSpan(2);


                    Tid addTid = new Tid() { TidId = id, TidFra = id2, TidTil = id3 };
                    TidList.Add(addTid);
                }
                connection.Close();
                return TidList;
            }
        }

        public Tid GetTidFromId(int tidId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = $"SELECT * FROM Tid WHERE TidId = {tidId}";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();
                Tid nytid = new Tid();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    TimeSpan id2 = reader.GetTimeSpan(1);
                    TimeSpan id3 = reader.GetTimeSpan(2);


                    nytid.TidId = id;
                    nytid.TidFra = id2;
                    nytid.TidTil = id3;
                }
                connection.Close();
                return nytid;
            }
        }

        public bool CreateTid(Tid tid)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllTid().Contains(tid);
                if (!check)
                {
                    var querystring =
                        $"INSERT INTO Tid VALUES ('{tid.TidFra}','{tid.TidTil}')";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public bool UpdateTid(Tid tid, int tidId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllTid().Contains(tid);
                if (!check)
                {
                    var querystring = $"UPDATE Tid SET TidFra = '{tid.TidFra}', TidTil = '{tid.TidTil}' WHERE TidId = {tidId}";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public Tid DeleteTid(int tidId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetTidFromId(tidId);
                if (check != null)
                {
                    var querystring =
                        $"DELETE FROM Tid WHERE TidId = {tidId}";
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
