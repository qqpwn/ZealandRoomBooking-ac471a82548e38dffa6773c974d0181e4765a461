using System.Collections.Generic;
using ModelKlasser;

namespace DBContext
{
    public interface IManageTid
    {
        List<Tid> GetAllTid();

        Tid GetTidFromId(int tidId);

        bool CreateTid(Tid tid);

        bool UpdateTid(Tid tid, int tidId);

        Tid DeleteTid(int tidId);
    }
}