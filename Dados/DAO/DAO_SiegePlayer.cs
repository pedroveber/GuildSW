using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dados.Models;
using System.Data.SqlClient;

namespace Dados.DAO
{
    public class DAO_SiegePlayer
    {
        public void InserirSiegePlayer(Models.SiegePlayer obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("MERGE DBO.SiegePlayers AS TARGET ");
            select.AppendLine("USING(SELECT @IdSiege AS IdSiege, @IdPlayer as IdPlayer) AS SOURCE ");
            select.AppendLine("ON TARGET.IdSiege = SOURCE.IdSiege and ");

            select.AppendLine("Target.IdPlayer = SOURCE.IdPlayer ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.UsedUnitCount = @UsedUnits ");

            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdSiege, IdPlayer, UsedUnitCount) ");
            select.AppendLine("VALUES(@IdSiege, @IdPlayer, @UsedUnits); ");


            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@UsedUnits", System.Data.SqlDbType.Int));
            command.Parameters["@UsedUnits"].Value = obj.UsedUnits;



            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();
        }

        public Models.SiegePlayerOponente InserirSiegePlayerOponente(Models.SiegePlayerOponente obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("MERGE DBO.SiegePlayerOponente AS TARGET ");
            select.AppendLine("USING(SELECT @IdSiege AS IdSiege, @IdPlayer as IdPlayer, @IdGuilda as IdGuild) AS SOURCE ");
            select.AppendLine("ON TARGET.IdSiege = SOURCE.IdSiege and ");
            select.AppendLine("Target.IdPlayer = SOURCE.IdPlayer and ");
            select.AppendLine("Target.IdGuilda = SOURCE.IdGuild ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.Nome = @Nome ");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdPlayer, IdSiege, IdGuilda, Nome) ");
            select.AppendLine("VALUES(@IdPlayer, @IdSiege, @IdGuilda, @Nome)");
            select.AppendLine("OUTPUT inserted.Id ;");
            
            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@IdGuilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdGuilda"].Value = obj.IdGuild;

            command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar));
            command.Parameters["@Nome"].Value = obj.Nome;



            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            long modified =(long)command.ExecuteScalar();

            conexao.Close();
            conexao.Dispose();
            obj.Id = modified;
            return obj;
            
        }

        public List<Models.SiegePlayerOponente> ListarPlayersOponentesSiege(long idSiege)
        {
            List<Models.SiegePlayerOponente> objRetorno = new List<SiegePlayerOponente>();

            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            
            select.AppendLine("SELECT ");
            select.AppendLine("Id,IdPlayer,IdSiege,IdGuilda,Nome");
            select.AppendLine("FROM dbo.SiegePlayerOponente ");
            select.AppendLine("where IdSiege = @idSiege");

            command.Parameters.Add(new SqlParameter("@idSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@idSiege"].Value = idSiege;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            Models.SiegePlayerOponente objOponente;

            while (reader.Read())
            {
                objOponente = new SiegePlayerOponente();
                objOponente.Id = long.Parse(reader["Id"].ToString());
                objOponente.IdGuild = long.Parse(reader["IdGuilda"].ToString());
                objOponente.IdPlayer = long.Parse(reader["IdPlayer"].ToString());
                objOponente.IdSiege = long.Parse(reader["IdSiege"].ToString());
                objOponente.Nome = reader["Nome"].ToString();

                objRetorno.Add(objOponente);
            }

            conexao.Close();
            conexao.Dispose();


            return objRetorno;

        }
    }
}
