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


            select.AppendLine("MERGE DBO.SiegePlayers AS TARGET ");
            select.AppendLine("USING(SELECT @IdSiege AS IdSiege, @IdPlayer as IdPlayer) AS SOURCE ");
            select.AppendLine("ON TARGET.IdSiege = SOURCE.IdSiege and ");

            select.AppendLine("Target.IdPlayer = SOURCE.IdPlayer ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.UsedUnitCount = @UsedUnits ");

            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdSiege, IdPlayer, UsedUnitCount) ");
            select.AppendLine("VALUES(@IdSiege, @IdPlayer, @UsedUnits) ");


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

        public void InserirSiegePlayerOponente(Models.SiegePlayerOponente obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();


            select.AppendLine("MERGE DBO.SiegePlayersOponente AS TARGET ;");
            select.AppendLine("USING(SELECT @IdSiege AS IdSiege, @IdPlayer as IdPlayer, @idGuild as IdGuild) AS SOURCE ;");
            select.AppendLine("ON TARGET.IdSiege = SOURCE.IdSiege and ;");
            select.AppendLine("Target.IdPlayer = SOURCE.IdPlayer and ;");
            select.AppendLine("Target.IdGuild = SOURCE.IdGuild ;");
            select.AppendLine("WHEN MATCHED THEN ;");
            select.AppendLine("UPDATE SET TARGET.Nome = @Nome ;");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ;");
            select.AppendLine("INSERT(IdPlayer, IdSiege, IdGuilda, Nome) ;");
            select.AppendLine("VALUES(@IdPlayer, @IdSiege, @IdGuilda, @Nome) ;");

            select.AppendLine("OUTPUT $action, inserted.Id ;");




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
        }
    }
}
