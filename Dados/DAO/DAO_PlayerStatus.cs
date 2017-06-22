using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DAO
{
    public class DAO_PlayerStatus
    {
        public static PlayerStatus _SelectByID(PlayerStatus _obj)
        {
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                return ObjEntity.PlayerStatus.Where(w => w.IdBatalha == _obj.IdBatalha && w.IdPlayer == _obj.IdPlayer).FirstOrDefault();
            }
        }

        public static PlayerStatus Insert(PlayerStatus _obj)
        {
            using (var ObjEntit = new DB_SW_GuildEntities())
            {
                ObjEntit.PlayerStatus.Add(_obj);
                ObjEntit.SaveChanges();
            }
            return _obj;
        }
        public static PlayerStatus UpDate(PlayerStatus _obj)
        {
            PlayerStatus ObjTemp = _SelectByID(_obj);
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                ObjTemp.Status = _obj.Status;
                ObjEntity.PlayerStatus.Attach(ObjTemp);
                ObjEntity.Entry(ObjTemp).State = EntityState.Modified;
                ObjEntity.SaveChanges();
            }
            return ObjTemp;
        }
        public static void ApagaTudo()
        {
            using (var context = new DB_SW_GuildEntities())
            {
                // cria comando
                var deleteCommand = context.Database.Connection.CreateCommand();
                deleteCommand.CommandText = "DELETE FROM PlayerStatus";
                // executa comando
                context.Database.Connection.Open();
                deleteCommand.ExecuteNonQuery();
                context.Database.Connection.Close();
            }
        }
    }
}
