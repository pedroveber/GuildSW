using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dados.Models;
using System.Data.SqlClient;

namespace Dados.DAO
{
    public class DAO_Player
    {
        public static Player _SelectByID(Player obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select ID,Nome,Level,PontoArena,Status,imagem from dbo.Player ");
            select.AppendLine("where id = @id");

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
            command.Parameters["@id"].Value = obj.ID;
            
            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            Player objPlayer = null;

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objPlayer = new Player();
                objPlayer.ID = long.Parse(reader["ID"].ToString());
                objPlayer.Imagem = reader["Imagem"].ToString();
                objPlayer.Level= long.Parse(reader["Level"].ToString());
                objPlayer.Nome = reader["Nome"].ToString();
                objPlayer.PontoArena = long.Parse(reader["PontoArena"].ToString());
                objPlayer.Status = reader["Status"].ToString();
                
                //TODO: PlayerStatus
                //objPlayer.PlayerStatus

                //TODO: PlayerDefesas
                //objPlayer.PlayerDefesas

                //TODO: lutas
                //objPlayer.Lutas



            }

            conexao.Close();
            conexao.Dispose();

            return objPlayer;
        }
        public static Player Insert(Player obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("insert into dbo.Player (ID,Nome,Level,PontoArena) ");
            select.AppendLine("values (@ID, @Nome, @Level, @PontoArena)");

            command.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.BigInt));
            command.Parameters["@ID"].Value = obj.ID;

            command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar));
            command.Parameters["@Nome"].Value = obj.Nome;

            command.Parameters.Add(new SqlParameter("@Level", System.Data.SqlDbType.BigInt));
            command.Parameters["@Level"].Value = obj.Level;

            command.Parameters.Add(new SqlParameter("@PontoArena", System.Data.SqlDbType.BigInt));
            command.Parameters["@PontoArena"].Value = obj.PontoArena;

            
            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;


            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();
                        
            return obj;
        }
        public static Player UpDate(Player obj)
        {
            Player ObjTemp = _SelectByID(obj);

            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("update dbo.Player set Status = @Status, PontoArena=@PontoArena where ID = @ID ");

            command.Parameters.Add(new SqlParameter("@Status", System.Data.SqlDbType.VarChar));
            command.Parameters["@Status"].Value = obj.Status;

            command.Parameters.Add(new SqlParameter("@PontoArena", System.Data.SqlDbType.BigInt));
            command.Parameters["@PontoArena"].Value = obj.PontoArena;

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
            ObjTemp.PontoArena = obj.PontoArena;
            return ObjTemp;
        }
       
    }
}
