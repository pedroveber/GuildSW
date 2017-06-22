using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DAO
{
    public class DAO_Lutas
    {
        public static Lutas _SelectByPlayer_Oponente(Lutas _obj)
        {
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                return ObjEntity.Lutas.Where(w => w.CodBatalhas == _obj.CodBatalhas && w.CodPlayer == _obj.CodPlayer && w.CodPlayerOponente == _obj.CodPlayerOponente).FirstOrDefault();
            }
        }
        public static List<Lutas> _SelectAllByBatalha(Batalhas _obj)
        {
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                return ObjEntity.Lutas.Where(w => w.CodBatalhas == _obj.ID).ToList();
            }
        }

        public static Lutas Insert(Lutas _obj)
        {
            using (var ObjEntit = new DB_SW_GuildEntities())
            {
                ObjEntit.Lutas.Add(_obj);
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
                deleteCommand.CommandText = "DELETE FROM Lutas";
                // executa comando
                context.Database.Connection.Open();
                deleteCommand.ExecuteNonQuery();
                context.Database.Connection.Close();
            }
        }
        public static void ApagaTudoByBatalha(long _idBatalha)
        {
            using (var context = new DB_SW_GuildEntities())
            {
                // cria comando
                var deleteCommand = context.Database.Connection.CreateCommand();
                deleteCommand.CommandText = "DELETE FROM Lutas where CodBatalhas = " + _idBatalha;
                // executa comando
                context.Database.Connection.Open();
                deleteCommand.ExecuteNonQuery();
                context.Database.Connection.Close();
            }
        }
    }
}
