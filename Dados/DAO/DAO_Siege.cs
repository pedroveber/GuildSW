using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dados.Models;
using System.Data.SqlClient;

namespace Dados.DAO
{
    public class DAO_Siege
    {
        /// <summary>
        /// Insere ou dá UPDATE
        /// </summary>
        public Models.Siege InserirSiege(Models.Siege obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("MERGE DBO.SIEGE AS TARGET ");
            select.AppendLine("USING(SELECT @ID AS ID) AS SOURCE ");
            select.AppendLine("ON TARGET.ID = SOURCE.ID ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.DATA = GETDATE() ");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(ID, DATA) ");
            select.AppendLine("VALUES(@ID, @Data); ");
            
            command.Parameters.Add(new SqlParameter("@ID", System.Data.SqlDbType.BigInt));
            command.Parameters["@ID"].Value = obj.Id;

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.Date));
            command.Parameters["@Data"].Value = obj.Data;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();

            return obj;
        }

        public Models.SiegeGuilda InserirSiegeGuilda(Models.SiegeGuilda obj)
        {

            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.Append("MERGE DBO.SiegeGuilda AS TARGET ");
            select.Append("USING(SELECT @IdSiege AS IdSiege, @IdGuilda as IdGuilda) AS SOURCE ");
            select.Append("ON TARGET.IdSiege = SOURCE.IdSiege and ");
            select.Append("Target.IdGuilda = SOURCE.IdGuilda ");
            select.Append("WHEN MATCHED THEN ");
            select.Append("UPDATE SET TARGET.Posicao = @Posicao ");
            select.Append("WHEN NOT MATCHED BY TARGET THEN ");
            select.Append("INSERT(IdSiege, IdGuilda, Posicao) ");
            select.Append("VALUES(@IdSiege, @IdGuilda, @Posicao); ");

            
            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@IdGuilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdGuilda"].Value = obj.IdGuilda;

            command.Parameters.Add(new SqlParameter("@Posicao", System.Data.SqlDbType.Int));
            command.Parameters["@Posicao"].Value = obj.Posicao;
            
            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();

            return obj;
        }
    }
}
