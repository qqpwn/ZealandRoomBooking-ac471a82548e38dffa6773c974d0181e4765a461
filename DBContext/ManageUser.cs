using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelKlasser;

namespace DBContext
{
    public class ManageUser : IManageUser
    {


        public const string DBaddress = "Server=tcp:zealandroombooking.database.windows.net,1433;Initial Catalog=ZealandRoomBooking;Persist Security Info=False;User ID=Detdetjojaja;Password=Detdetjo42;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public List<User> UserList = new List<User>();

        public List<User> GetAllUser()
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = "SELECT * FROM [User]";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string id2 = reader.GetString(1);
                    string id3 = reader.GetString(2);
                    string id4 = reader.GetString(3);


                    User addUser = new User() { UserId = id, Username = id2, Password = id3, UserType = id4};
                    UserList.Add(addUser);
                }
                connection.Close();
                return UserList;
            }
        }

        public User GetUserFromId(int userId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var quertstring = $"SELECT * FROM [User] WHERE UserId = {userId}";
                SqlCommand command = new SqlCommand(quertstring, connection);
                connection.Open();
                User nyUser = new User();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string id2 = reader.GetString(1);
                    string id3 = reader.GetString(2);
                    string id4 = reader.GetString(3);

                    nyUser.UserId = id;
                    nyUser.Username = id2;
                    nyUser.Password = id3;
                    nyUser.UserType = id4;
                }
                connection.Close();
                return nyUser;
            }
        }

        public bool CreateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllUser().Contains(user);
                if (!check)
                {
                    var querystring =
                        $"INSERT INTO [User] VALUES ('{user.Username}','{user.Password}','{user.UserType}')";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public bool UpdateUser(User user, int userId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetAllUser().Contains(user);
                if (!check)
                {
                    var querystring = $"UPDATE [User] SET Username = '{user.Username}', Password = '{user.Password}', UserType = '{user.UserType}' WHERE UserId = {userId}";
                    SqlCommand command = new SqlCommand(querystring, connection);
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                }

                return check;
            }
        }

        public User DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(DBaddress))
            {
                var check = GetUserFromId(userId);
                if (check != null)
                {
                    var querystring =
                        $"DELETE FROM [User] WHERE UserId = {userId}";
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
