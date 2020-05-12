using System.Collections.Generic;
using ModelKlasser;


namespace DBContext
{
    public interface IManageBookinger
    {
        List<Bookinger> GetAllBookinger();

        Bookinger GetBookingerFromId(int bookingerId);

        bool CreateBookinger(Bookinger bookinger);

        bool UpdateBookinger(Bookinger bookinger, int bookingerId);

        Bookinger DeleteBookinger(int bookingerId);
    }
}