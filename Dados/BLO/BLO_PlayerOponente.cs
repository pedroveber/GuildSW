using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.BLO
{
    public class BLO_PlayerOponente
    {
        public PlayerOponente Insert(PlayerOponente _obj)
        {
            if (DAO.DAO_PlayerOponente._SelectByID(_obj) == null)
                return DAO.DAO_PlayerOponente.Insert(_obj);
            else return DAO.DAO_PlayerOponente._SelectByID(_obj);
        }
    }
}
