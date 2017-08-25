using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
namespace Dados.DAO
{
   public class DAO_TimeDefesa
    {
        public void AtualizarTimeDefesa(Models.InfoDefesas.Root defesa)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand sqlCom = new SqlCommand();

            conn.ConnectionString = BLO.Conexao.ObterStringConexao2();
            StringBuilder cmd = new StringBuilder();
            
            //excluir tudo daaquela Data e Player
            cmd.Append("delete FROM DB_SW.dbo.TimeDefesa WHERE IdPlayer = " + defesa.defense_wizard_id.ToString() + " and Data = convert(date,'" + DateTime.Now.ToString("yyyy-MM-dd") +  "')");

            conn.Open();
            sqlCom.Connection = conn;
            sqlCom.CommandText = cmd.ToString();
            sqlCom.ExecuteNonQuery();

            //Inserir novamente
            cmd.Clear();

            long? monstro1 =0;
            long? monstro2 =0;
            long? monstro3 = 0;
            long? monstro4 = 0;
            long? monstro5 = 0;
            long? monstro6 = 0;


            foreach (List<Models.GuildWarDefenseUnitList> item in defesa.guildwar_defense_unit_list) //Roda 2 vz 
            {
                //Uma Lista de DefenseUNit
                foreach (Models.GuildWarDefenseUnitList item2 in item) //Roda 3 vez (posiscao 1,2,3 no primeiro loop)(4,5,6 no segundo)
                {
                    if (item2.pos_id ==1) monstro1 = item2.unit_info.unit_master_id;
                    if (item2.pos_id == 2) monstro2 = item2.unit_info.unit_master_id;
                    if (item2.pos_id == 3) monstro3 = item2.unit_info.unit_master_id;
                    if (item2.pos_id == 4) monstro4 = item2.unit_info.unit_master_id;
                    if (item2.pos_id == 5) monstro5 = item2.unit_info.unit_master_id;
                    if (item2.pos_id == 6) monstro6 = item2.unit_info.unit_master_id;

                }
            }

            cmd.AppendLine("insert into DB_SW.dbo.TimeDefesa (Idplayer, Data,Monstro1,Monstro2,Monstro3,Monstro4,Monstro5,Monstro6) values ");
            cmd.AppendLine("(@idPlayer,@Data,@Monstro1,@Monstro2,@Monstro3,@Monstro4,@Monstro5,@Monstro6)");

            sqlCom.CommandText = cmd.ToString();
            sqlCom.CommandType = System.Data.CommandType.Text;

            sqlCom.Parameters.Add(new SqlParameter("@idPlayer", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@idPlayer"].Value = defesa.defense_wizard_id;

            sqlCom.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.Date));
            sqlCom.Parameters["@Data"].Value = DateTime.Now.ToShortDateString();

            sqlCom.Parameters.Add(new SqlParameter("@Monstro1", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@Monstro1"].Value = monstro1;

            sqlCom.Parameters.Add(new SqlParameter("@Monstro2", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@Monstro2"].Value = monstro2;

            sqlCom.Parameters.Add(new SqlParameter("@Monstro3", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@Monstro3"].Value = monstro3;

            sqlCom.Parameters.Add(new SqlParameter("@Monstro4", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@Monstro4"].Value = monstro4;

            sqlCom.Parameters.Add(new SqlParameter("@Monstro5", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@Monstro5"].Value = monstro5;

            sqlCom.Parameters.Add(new SqlParameter("@Monstro6", System.Data.SqlDbType.Int));
            sqlCom.Parameters["@Monstro6"].Value = monstro6;

            sqlCom.CommandText = cmd.ToString();
            sqlCom.ExecuteNonQuery();

        }
    }
}
