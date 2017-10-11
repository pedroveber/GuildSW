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
    public class DAO_Guilda
    {
        public static Guilda _SelectByID(Guilda obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select ID,Nome from dbo.Guilda ");
            select.AppendLine("where id = @id");

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
            command.Parameters["@id"].Value = obj.Id;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            Guilda objGuilda = null;

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objGuilda = new Guilda();
                objGuilda.Id = long.Parse(reader["ID"].ToString());
                objGuilda.Nome = reader["Nome"].ToString();
              }

            conexao.Close();
            conexao.Dispose();

            return objGuilda;
        }

        public static Guilda Insert(Guilda obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("insert into dbo.Guilda (ID,Nome) ");
            select.AppendLine("values (@ID, @Nome)");

            command.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.BigInt));
            command.Parameters["@ID"].Value = obj.Id;

            command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar));
            command.Parameters["@Nome"].Value = obj.Nome;
                        
            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;
            
            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();

            return obj;
        }

        public static Guilda UpDate(Guilda obj)
        {
            Guilda ObjTemp = _SelectByID(obj);

            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("update dbo.Guilda set Nome = @Nome where ID = @ID ");

            command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar));
            command.Parameters["@Nome"].Value = obj.Nome;
                  
            command.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.BigInt));
            command.Parameters["@ID"].Value = obj.Id;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();

            ObjTemp.Nome = obj.Nome;
            return ObjTemp;
        }

        public static List<GuildaPlayer> ListarGuildaPlayer(long idGuilda)
        {
            //codbatalha = obj.id
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select IdGuilda,IdUsuario,IdPlayer,Ativo from dbo.Guilda_Player ");
            select.AppendLine("where idGuilda=@idGuilda");

            command.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@idGuilda"].Value = idGuilda;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            GuildaPlayer objPlayerGuilda = new GuildaPlayer();
            List<GuildaPlayer> lstPlayers = new List<GuildaPlayer>();

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objPlayerGuilda = new GuildaPlayer();
                objPlayerGuilda.Ativo = Convert.ToBoolean(reader["Ativo"].ToString());
                objPlayerGuilda.IdGuilda = long.Parse(reader["IdGuilda"].ToString());
                objPlayerGuilda.IdPlayer = long.Parse(reader["IdPlayer"].ToString());

                if (reader["idUsuario"].ToString() != string.Empty)
                    objPlayerGuilda.IdUsuario = reader["IdUsuario"].ToString();


                lstPlayers.Add(objPlayerGuilda);

            }

            conexao.Close();
            conexao.Dispose();

            return lstPlayers;
        }

        public static void InsertGuildaPlayer(long idGuilda,long idPlayer)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("insert into dbo.Guilda_Player(IdGuilda,IdPlayer,Ativo)  ");
            select.AppendLine("values (@IdGuilda,@IdPlayer,1)");

            command.Parameters.Add(new SqlParameter("@IdGuilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdGuilda"].Value = idGuilda;

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = idPlayer;
            
            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();
            
        }

    }
}
