using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.BLO
{
    public class BLO_Player
    {
        public Player Insert(Player _obj)
        {
            if (DAO.DAO_Player._SelectByID(_obj) == null)
                return DAO.DAO_Player.Insert(_obj);
            else return DAO.DAO_Player.UpDate(_obj);
        }
    }
}
