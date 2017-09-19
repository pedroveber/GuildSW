using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Dados.Models;


namespace Dados.DAO
{
    public class DAO_PlayerDefesas
    {
        public static PlayerDefesas _SelectBy_Data_idPlayer(PlayerDefesas obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select ID,IdPlayer,NomeOponente,NomeGuilda,Vitoria,DataHora from dbo.PlayerDefesas ");
            select.AppendLine("where IdPlayer = @IdPlayer and DataHora=@DataHora");

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@DataHora", System.Data.SqlDbType.DateTime));
            command.Parameters["@DataHora"].Value = obj.DataHora;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            PlayerDefesas objPlayerDefesa = new PlayerDefesas();

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objPlayerDefesa = new PlayerDefesas();
                objPlayerDefesa.ID = long.Parse(reader["ID"].ToString());
                objPlayerDefesa.DataHora = DateTime.Parse(reader["DataHora"].ToString());
                objPlayerDefesa.IdPlayer = long.Parse(reader["IdPlayer"].ToString());
                objPlayerDefesa.NomeGuilda = reader["NomeGuilda"].ToString();
                objPlayerDefesa.NomeOponente = reader["NomeOponente"].ToString();

                objPlayerDefesa.Player = DAO_Player._SelectByID(new Player() { ID = Convert.ToInt32(obj.IdPlayer) });
                objPlayerDefesa.Vitoria = int.Parse(reader["Vitoria"].ToString());

            }

            conexao.Close();
            conexao.Dispose();

            return objPlayerDefesa;

        }

        public static PlayerDefesas Insert(PlayerDefesas obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("insert into dbo.PlayerDefesas (IdPlayer,NomeOponente,NomeGuilda,Vitoria,DataHora) ");
            select.AppendLine("output INSERTED.ID values (@IdPlayer, @NomeOponente, @NomeGuilda, @Vitoria, @DataHora)");

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@NomeOponente", System.Data.SqlDbType.VarChar));
            command.Parameters["@NomeOponente"].Value = obj.NomeOponente;

            command.Parameters.Add(new SqlParameter("@NomeGuilda", System.Data.SqlDbType.VarChar));
            command.Parameters["@NomeGuilda"].Value = obj.NomeGuilda;

            command.Parameters.Add(new SqlParameter("@Vitoria", System.Data.SqlDbType.Int));
            command.Parameters["@Vitoria"].Value = obj.Vitoria;

            command.Parameters.Add(new SqlParameter("@DataHora", System.Data.SqlDbType.DateTime));
            command.Parameters["@DataHora"].Value = obj.DataHora;


            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;


            conexao.Open();
            command.Connection = conexao;
            obj.ID = (int)command.ExecuteScalar();
            
            conexao.Close();
            conexao.Dispose();

            return obj;
        }
        public static List<PlayerDefesas> SelectAllByPlayer(PlayerDefesas obj)
        {
            //idplayer = idplauer
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select ID,IdPlayer,NomeOponente,NomeGuilda,Vitoria,DataHora from dbo.PlayerDefesas ");
            select.AppendLine("where IdPlayer = @IdPlayer");

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            
            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            PlayerDefesas objPlayerDefesa;
            List<PlayerDefesas> objRetorno = new List<PlayerDefesas>();

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objPlayerDefesa = new PlayerDefesas();
                objPlayerDefesa.ID = long.Parse(reader["ID"].ToString());
                objPlayerDefesa.DataHora = DateTime.Parse(reader["DataHora"].ToString());
                objPlayerDefesa.IdPlayer = long.Parse(reader["IdPlayer"].ToString());
                objPlayerDefesa.NomeGuilda = reader["NomeGuilda"].ToString();
                objPlayerDefesa.NomeOponente = reader["NomeOponente"].ToString();

                objPlayerDefesa.Player = DAO_Player._SelectByID(new Player() { ID = Convert.ToInt32(obj.IdPlayer) });
                objPlayerDefesa.Vitoria = int.Parse(reader["Vitoria"].ToString());

                objRetorno.Add(objPlayerDefesa);

            }

            conexao.Close();
            conexao.Dispose();

            return objRetorno;
        }
    }

}
