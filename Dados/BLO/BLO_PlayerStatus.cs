using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dados.Models;

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
            PlayerStatus objRetorno = new PlayerStatus();
            objRetorno = DAO.DAO_PlayerStatus._SelectByID(_obj);

            if (objRetorno.ID<=0)
                return DAO.DAO_PlayerStatus.Insert(_obj);
            else
            {
                objRetorno.Status = _obj.Status;
                return DAO.DAO_PlayerStatus.UpDate(objRetorno);
            }
                
        }
    }
}
