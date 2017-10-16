using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dados.Models;

namespace Dados.DAO
{
    public class DAO_PlayerStatus
    {
        public static PlayerStatus _SelectByID(PlayerStatus obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select id, IdPlayer,IdBatalha,Status  from dbo.PlayerStatus ");
            select.AppendLine("where IdBatalha = @IdBatalha and IdPlayer=@IdPlayer");

            command.Parameters.Add(new SqlParameter("@IdBatalha", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdBatalha"].Value = obj.IdBatalha;

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            PlayerStatus objPlayerStatus = new PlayerStatus();

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objPlayerStatus = new PlayerStatus();
                objPlayerStatus.ID = long.Parse(reader["ID"].ToString());
                objPlayerStatus.IdBatalha = long.Parse(reader["IdBatalha"].ToString());
                objPlayerStatus.IdPlayer = long.Parse(reader["IdPlayer"].ToString());
                objPlayerStatus.Player = DAO_Player._SelectByID(new Player() { ID = Convert.ToInt32(obj.IdPlayer) });
                objPlayerStatus.Status = reader["Status"].ToString();
                
                //TODO: Batalhas
                //objPlayerStatus.Batalhas

            }

            conexao.Close();
            conexao.Dispose();

            return objPlayerStatus;

        }

        public static PlayerStatus Insert(PlayerStatus obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("insert into dbo.PlayerStatus (IdPlayer,IdBatalha,Status) ");
            select.AppendLine("output INSERTED.ID values (@IdPlayer, @IdBatalha, @Status)");

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@IdBatalha", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdBatalha"].Value = obj.IdBatalha;

            command.Parameters.Add(new SqlParameter("@Status", System.Data.SqlDbType.VarChar));
            command.Parameters["@Status"].Value = obj.Status;
            
            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            obj.ID = (long)command.ExecuteScalar();

            conexao.Close();
            conexao.Dispose();

            return obj;
        }
        public static PlayerStatus UpDate(PlayerStatus obj)
        {
            
            PlayerStatus ObjTemp = _SelectByID(obj);
            
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("update dbo.PlayerStatus set Status= @Status where Id = @id ");

            command.Parameters.Add(new SqlParameter("@Status", System.Data.SqlDbType.VarChar));
            command.Parameters["@Status"].Value = obj.Status;
            
            command.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.BigInt));
            command.Parameters["@ID"].Value = obj.ID;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();

            ObjTemp.Status = obj.Status;
            return ObjTemp;
        }
      
    }
}
