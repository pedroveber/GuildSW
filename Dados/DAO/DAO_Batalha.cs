using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.DAO
{
    public class DAO_Batalha
    {
        public static List<Batalhas> _SelectAll()
        {
            //
            return new DB_SW_GuildEntities().Batalhas.ToList();
        }

        public static Batalhas _SelectByID(Batalhas _obj)
        {
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                return ObjEntity.Batalhas.Where(w => w.ID == _obj.ID).FirstOrDefault();
            }
        }
        public static Batalhas _SelectByIdDate(Batalhas _obj)
        {
            
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                //gambi.. tive que arrumar rapidao antes de ir embora
                Batalhas temp = _obj;
                temp = ObjEntity.Batalhas.Where(w => w.idGuilda == _obj.idGuilda && w.Data == _obj.Data).FirstOrDefault();
                if (temp == null)
                {
                    temp = _obj;
                }

                if (temp.ID==0)
                {
                    DateTime tempDAta = Convert.ToDateTime(_obj.Data).AddDays(-1);
                    temp = ObjEntity.Batalhas.Where(w => w.idGuilda == _obj.idGuilda && w.Data == tempDAta).FirstOrDefault();
                }

                return temp;
            }
        }

        public static Batalhas Insert(Batalhas _obj)
        {
            using (var ObjEntit = new DB_SW_GuildEntities())
            {
                ObjEntit.Batalhas.Add(_obj);
                ObjEntit.SaveChanges();
            }
            return _obj;
        }

        public static void UpDateDate(Batalhas _obj)
        {
            Batalhas ObjTemp = _SelectByID(_obj);
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                ObjTemp.PontuacaoGuild = _obj.PontuacaoGuild;
                ObjTemp.PontuacaoOponente = _obj.PontuacaoOponente;
                ObjTemp.RankGuild = _obj.RankGuild;

                ObjEntity.Batalhas.Attach(ObjTemp);
                ObjEntity.Entry(ObjTemp).State = EntityState.Modified;
                ObjEntity.SaveChanges();
            }
        }

        public static void UpDateDateTime(Batalhas _obj)
        {
            Batalhas ObjTemp = _SelectByID(_obj);
            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                ObjTemp.Data = _obj.Data;

                ObjEntity.Batalhas.Attach(ObjTemp);
                ObjEntity.Entry(ObjTemp).State = EntityState.Modified;
                ObjEntity.SaveChanges();
            }
        }


        public static void ApagaTudo()
        {
            using (var context = new DB_SW_GuildEntities())
            {
                // cria comando
                var deleteCommand = context.Database.Connection.CreateCommand();
                deleteCommand.CommandText = "DELETE FROM Batalhas";
                // executa comando
                context.Database.Connection.Open();
                deleteCommand.ExecuteNonQuery();
                context.Database.Connection.Close();
            }
        }
    }
}
