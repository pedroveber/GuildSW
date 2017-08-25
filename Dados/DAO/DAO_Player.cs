using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DAO
{
    public class DAO_Player
    {
        public static Player _SelectByID(Player _obj)
        {
            //)
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                return ObjEntity.Player.Where(w => w.ID == _obj.ID).FirstOrDefault();
            }
        }
        public static Player Insert(Player _obj)
        {
            using (var ObjEntit = new DB_SW_GuildEntities())
            {
                ObjEntit.Player.Add(_obj);
                ObjEntit.SaveChanges();
            }
            return _obj;
        }
        public static Player UpDate(Player _obj)
        {
            Player ObjTemp = _SelectByID(_obj);
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                ObjTemp.Status = _obj.Status;
                ObjTemp.PontoArena = _obj.PontoArena;

                ObjEntity.Player.Attach(ObjTemp);
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
                deleteCommand.CommandText = "DELETE FROM Player";
                // executa comando
                context.Database.Connection.Open();
                deleteCommand.ExecuteNonQuery();
                context.Database.Connection.Close();
            }
        }
    }
}
