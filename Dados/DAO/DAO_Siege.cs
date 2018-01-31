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

            select.AppendLine("SET DATEFORMAT dmy; ");
            select.AppendLine("MERGE DBO.SIEGE AS TARGET ");
            select.AppendLine("USING(SELECT @idSiege AS idSiege, @idMatch as idMatch) AS SOURCE ");
            select.AppendLine("ON TARGET.idSiege = SOURCE.idSiege ");
            select.AppendLine("and target.idMatch = source.idMatch ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.DATA = @Data ");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdSiege, DATA, IdMatch) ");
            select.AppendLine("VALUES(@idSiege, @Data, @idMatch) ");
            select.AppendLine("OUTPUT inserted.Id; ");

            command.Parameters.Add(new SqlParameter("@idSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@idSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@idMatch", System.Data.SqlDbType.BigInt));
            command.Parameters["@idMatch"].Value = obj.IdMatch;

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.Date));
            command.Parameters["@Data"].Value = obj.Data;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            long idInserido = (long)command.ExecuteScalar();

            conexao.Close();
            conexao.Dispose();

            obj.Id = idInserido;
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
            select.Append("UPDATE SET TARGET.Posicao = @Posicao, Rating = @Rating, MatchScore = @MatchScore, Members = @Members ");
            select.Append("WHEN NOT MATCHED BY TARGET THEN ");
            select.Append("INSERT(IdSiege, IdGuilda, Posicao,Rating,MatchScore,Members) ");
            select.Append("VALUES(@IdSiege, @IdGuilda, @Posicao,@Rating,@MatchScore,@Members); ");


            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@IdGuilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdGuilda"].Value = obj.IdGuilda;

            command.Parameters.Add(new SqlParameter("@Posicao", System.Data.SqlDbType.Int));
            command.Parameters["@Posicao"].Value = obj.Posicao;

            command.Parameters.Add(new SqlParameter("@Rating", System.Data.SqlDbType.Int));
            command.Parameters["@Rating"].Value = obj.Rating;

            command.Parameters.Add(new SqlParameter("@MatchScore", System.Data.SqlDbType.Float));
            command.Parameters["@MatchScore"].Value = obj.MatchScore;

            command.Parameters.Add(new SqlParameter("@Members", System.Data.SqlDbType.Int));
            command.Parameters["@Members"].Value = obj.Members;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();

            return obj;
        }

        public List<Dados.Models.Siege> ListarSieges()
        {
            List<Dados.Models.Siege> objRetorno = new List<Dados.Models.Siege>();

            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();


            select.AppendLine("SELECT ");
            select.AppendLine("Id,Data,IdSiege,IdMatch");
            select.AppendLine("FROM dbo.Siege ");

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            Models.Siege objSiege;

            while (reader.Read())
            {
                objSiege = new Siege();
                objSiege.Id = long.Parse(reader["Id"].ToString());
                objSiege.IdMatch = long.Parse(reader["IdMatch"].ToString());
                objSiege.IdSiege = long.Parse(reader["IdSiege"].ToString());
                if (reader["Data"].ToString() != string.Empty)
                {
                    objSiege.Data = Convert.ToDateTime(reader["Data"].ToString());
                }


                objRetorno.Add(objSiege);
            }

            conexao.Close();
            conexao.Dispose();


            return objRetorno;
        }
    }
}
