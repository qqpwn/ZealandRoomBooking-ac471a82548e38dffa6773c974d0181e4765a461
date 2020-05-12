using System.Collections.Generic;
using ModelKlasser;

namespace DBContext
{
    public interface IManageUser
    {
        List<User> GetAllUser();

        User GetUserFromId(int userId);

        bool CreateUser(User user);

        bool UpdateUser(User user, int userId);

        User DeleteUser(int userId);
    }
}