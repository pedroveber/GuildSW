using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DAO
{
    public class DAO_PlayerOponente
    {
        public static PlayerOponente _SelectByID(PlayerOponente _obj)
        {
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                return ObjEntity.PlayerOponente.Where(w => w.ID == _obj.ID).FirstOrDefault();
            }
        }
        public static PlayerOponente Insert(PlayerOponente _obj)
        {
            using (var ObjEntit = new DB_SW_GuildEntities())
            {
                ObjEntit.PlayerOponente.Add(_obj);
                ObjEntit.SaveChanges();
            }
            return _obj;
        }
        public static void ApagaTudo()
        {
            using (var context = new DB_SW_GuildEntities())
            {
                // cria comando
                var deleteCommand = context.Database.Connection.CreateCommand();
                deleteCommand.CommandText = "DELETE FROM PlayerOponente";
                // executa comando
                context.Database.Connection.Open();
                deleteCommand.ExecuteNonQuery();
                context.Database.Connection.Close();
            }
        }
    }
}
