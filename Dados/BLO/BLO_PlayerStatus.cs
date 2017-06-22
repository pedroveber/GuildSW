using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.BLO
{
    public class BLO_PlayerStatus
    {
        public static PlayerStatus _SelectByID(PlayerStatus _obj)
        {
            return DAO.DAO_PlayerStatus._SelectByID(_obj);
        }
        public PlayerStatus Insert(PlayerStatus _obj)
        {
            if (DAO.DAO_PlayerStatus._SelectByID(_obj) == null)
                return DAO.DAO_PlayerStatus.Insert(_obj);
            else return DAO.DAO_PlayerStatus.UpDate(_obj);
        }
    }
}
