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
    public class DAO_SiegeDefense
    {
        public void InsertDefenseDeck(SiegeDefenseDeck obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();


            select.AppendLine("MERGE DBO.SiegeDefenseDeck AS TARGET ");
            select.AppendLine("USING(SELECT @IdDeck AS IdDeck, @IdSiege as IdSiege, @IdPlayer as IdPlayer, @IdGuild as IdGuild) AS SOURCE ");
            select.AppendLine("ON TARGET.IdDeck = SOURCE.IdDeck and ");

            select.AppendLine("Target.IdSiege = SOURCE.IdSiege and ");

            select.AppendLine("Target.IdPlayer = SOURCE.IdPlayer and ");

            select.AppendLine("Target.IdGuild = SOURCE.IdGuild ");

            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdDeck, IdSiege, IdPlayer, IdGuild) ");
            select.AppendLine("VALUES(@IdDeck, @IdSiege, @IdPlayer, @IdGuild) ");

            select.AppendLine("OUTPUT $action, inserted.Id ");


            command.Parameters.Add(new SqlParameter("@IdDeck", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdDeck"].Value = obj.IdDeck;

            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@IdGuild", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdGuild"].Value = obj.IdGuild;
            
            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            long modified = (long)command.ExecuteScalar();

            conexao.Close();
            conexao.Dispose();
        }

        public void InserirSiegePlayerDefense(SiegePlayerDefesa obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();


            //Select
            select.Append("MERGE DBO.SiegePlayerDefesa AS TARGET ");
           select.Append("USING(SELECT @IdDeck AS IdDeck, @IdPlayerOponente as IdPlayerOponente, @Vitoria as Vitoria, @Data as Data) AS SOURCE ");
            select.Append("ON TARGET.IdDeck = SOURCE.IdDeck and ");
            select.Append("Target.IdPlayerOponente SOURCE.IdPlayerOponente ");
            select.Append("WHEN MATCHED THEN ");
            select.Append("UPDATE SET TARGET.Vitoria = @Vitoria, Data = @Data ");
            select.Append("WHEN NOT MATCHED BY TARGET THEN ");
            select.Append("INSERT(IdDeck, IdPlayerOponente, Vitoria, Data) ");
            select.Append("VALUES(@IdDeck, @IdPlayerOponente, @Vitoria, @Data) ");
            select.Append("OUTPUT $action, inserted.Id ");

            command.Parameters.Add(new SqlParameter("@IdDeck", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdDeck"].Value = obj.IdDeck;

            command.Parameters.Add(new SqlParameter("@IdPlayerOponente", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayerOponente"].Value = obj.IdPlayerOponente;

            command.Parameters.Add(new SqlParameter("@Vitoria", System.Data.SqlDbType.Int));
            command.Parameters["@Vitoria"].Value = obj.Vitoria;

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.BigInt));
            command.Parameters["@Data"].Value = obj.Date;


            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            long modified = (long)command.ExecuteScalar();

            conexao.Close();
            conexao.Dispose();

        }
    }
}
