using System.Collections.Generic;
using ModelKlasser;

namespace DBContext
{
    public interface IManageLokaler
    {
        List<Lokaler> GetAllLokaler();

        Lokaler GetLokalerFromId(int lokalerId);

        bool CreateLokaler(Lokaler lokaler);

        bool UpdateLokaler(Lokaler lokaler, int lokalerId);

        Lokaler DeleteLokaler(int lokalerId);
    }
}