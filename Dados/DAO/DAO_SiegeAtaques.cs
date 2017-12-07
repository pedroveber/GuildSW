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
            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("MERGE DBO.SiegeAtaques AS TARGET ");
            select.AppendLine("USING(SELECT @IdSiege AS IdSiege, @IdPlayer as IdPlayer, @IdPlayerOponente as IdPlayerOponente, ");
            select.AppendLine("@Vitoria as Vitoria, @Data as Data, @Base as Base, @IdGuildaOpp as IdGuildaOpp) AS SOURCE ");
            select.AppendLine("ON TARGET.IdSiege = SOURCE.IdSiege and ");

            select.AppendLine("Target.IdPlayer = SOURCE.IdPlayer and ");
            select.AppendLine("Target.IdPlayerOponente = SOURCE.IdPlayerOponente ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.Vitoria = @Vitoria, TARGET.Data = @Data, TARGET.Base = @Base ");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdSiege, IdPlayer, IdPlayerOponente, Vitoria, Data,Base,IdGuildaOpp) ");
            select.AppendLine("VALUES(@IdSiege,@IdPlayer,@IdPlayerOponente, @Vitoria, @Data,@Base,@IdGuildaOpp)");
            select.AppendLine("OUTPUT inserted.Id; ");

            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@IdGuildaOpp", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdGuildaOpp"].Value = obj.IdGuildaOpp;

            command.Parameters.Add(new SqlParameter("@IdPlayerOponente", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayerOponente"].Value = obj.IdPlayerOponente;


            command.Parameters.Add(new SqlParameter("@Vitoria", System.Data.SqlDbType.Int));
            command.Parameters["@Vitoria"].Value = obj.Vitoria;

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.Date));
            command.Parameters["@Data"].Value = obj.Data;

            command.Parameters.Add(new SqlParameter("@Base", System.Data.SqlDbType.Int));
            command.Parameters["@Base"].Value = obj.Base;


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
