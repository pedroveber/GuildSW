using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Dados.DAO
{
    public class DAO_PlayerDefesas
    {
        public static PlayerDefesas _SelectBy_Data_idPlayer(PlayerDefesas _obj)
        {
            
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                return ObjEntity.PlayerDefesas.Where(w => w.DataHora == _obj.DataHora && w.IdPlayer == _obj.IdPlayer).FirstOrDefault();
            }
        }

        public static PlayerDefesas Insert(PlayerDefesas _obj)
        {
            using (var ObjEntit = new DB_SW_GuildEntities())
            {
                ObjEntit.PlayerDefesas.Add(_obj);
                ObjEntit.SaveChanges();
            }
            return _obj;
        }
        public static List<PlayerDefesas> SelectAllByPlayer(PlayerDefesas _obj)
        {
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                return ObjEntity.PlayerDefesas.Where(w => w.IdPlayer == _obj.IdPlayer).ToList();
            }
        }
    }

}
