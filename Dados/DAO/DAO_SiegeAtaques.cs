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
    public class DAO_SiegeAtaque
    {
        public void InserirSiegeAtaque(SiegeAtaque obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();


            //Select
            select.AppendLine("MERGE DBO.SiegeAtaques AS TARGET ");
            select.AppendLine("USING(SELECT @IdSiege AS IdSiege, @IPlayer as IdPlayer, @IdPlayer as IdPlayer, @IdPlayerOponente as IdPlayerOponente, @Vitoria as Vitoria, @Data as Data) AS SOURCE ");
            select.AppendLine("ON TARGET.IdSiege = SOURCE.IdSiege and ");

            select.AppendLine("Target.IdPlayer SOURCE.IdPlayer and ");
            select.AppendLine("Target.IdPlayerOponente = SOURCE.IdPlayerOponente ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.Vitoria = @Vitoria, Data = @Data ");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdSiege, IdPlayer, IdPlayerOponente, Vitoria, Data) ");
            select.AppendLine("VALUES(IdSiege, IdPlayer, IdPlayerOponente, @IdGuild, @Vitoria, @Data) ");
            select.AppendLine("OUTPUT $action, inserted.Id ");

            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@IdPlayerOponente", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayerOponente"].Value = obj.IdPlayerOponente;

            command.Parameters.Add(new SqlParameter("@Vitoria", System.Data.SqlDbType.Int));
            command.Parameters["@Vitoria"].Value = obj.Vitoria;

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.BigInt));
            command.Parameters["@Data"].Value = obj.Data;


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
