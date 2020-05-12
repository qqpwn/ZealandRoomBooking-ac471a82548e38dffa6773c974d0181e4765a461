using System.Collections.Generic;
using ModelKlasser;

namespace DBContext
{
    public interface IManageLokaleBookinger
    {
        List<LokaleBookinger> GetAllLokaleBookinger();

        LokaleBookinger GetLokaleBookingerFromId(int lokaleBookingerId);

        bool CreateLokaleBookinger(LokaleBookinger lokaleBookinger);

        bool UpdateLokaleBookinger(LokaleBookinger lokaleBookinger, int lokaleBookingerId);

        LokaleBookinger DeleteLokaleBookinger(int lokaleBookingerId);
    }
}