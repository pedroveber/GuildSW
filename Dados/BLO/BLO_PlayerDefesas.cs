using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.BLO
{
   public class BLO_PlayerDefesas
    {
       public PlayerDefesas Insert(PlayerDefesas _obj)
       {
           if (DAO.DAO_PlayerDefesas._SelectBy_Data_idPlayer(_obj) == null)
               return DAO.DAO_PlayerDefesas.Insert(_obj);
           return _obj;
       }

       public static List<PlayerDefesas> SelectAllByPlayer(PlayerDefesas _obj)
       {          
               return DAO.DAO_PlayerDefesas.SelectAllByPlayer(_obj);           
       }
    }
}
