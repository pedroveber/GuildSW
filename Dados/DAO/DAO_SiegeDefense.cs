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

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("MERGE DBO.SiegeDefenseDeck AS TARGET ");
            select.AppendLine("USING(SELECT @IdDeck AS IdDeck, @IdSiege as IdSiege, @IdPlayer as IdPlayer, @IdGuild as IdGuild) AS SOURCE ");
            select.AppendLine("ON TARGET.IdDeck = SOURCE.IdDeck and ");
            select.AppendLine("Target.IdSiege = SOURCE.IdSiege and ");
            select.AppendLine("Target.IdPlayer = SOURCE.IdPlayer and ");
            select.AppendLine("Target.IdGuild = SOURCE.IdGuild ");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdDeck, IdSiege, IdPlayer, IdGuild) ");
            select.AppendLine("VALUES(@IdDeck, @IdSiege, @IdPlayer, @IdGuild); ");



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
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();
        }

        public List<SiegeDefenseDeck> ListarDefenseDecks(long idSiege)
        {
            List<SiegeDefenseDeck> objRetorno = new List<SiegeDefenseDeck>();

            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("SELECT ");
            select.AppendLine("ID, IDDECK, IDSIEGE, IDPLAYER, IDGUILD ");
            select.AppendLine("FROM DBO.SIEGEDEFENSEDECK ");
            select.AppendLine("where IDSIEGE = @idSiege");

            command.Parameters.Add(new SqlParameter("@idSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@idSiege"].Value = idSiege;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            SiegeDefenseDeck objDefense;

            while (reader.Read())
            {
                objDefense = new SiegeDefenseDeck();
                objDefense.Id = long.Parse(reader["ID"].ToString());
                objDefense.IdDeck = long.Parse(reader["IDDECK"].ToString());
                objDefense.IdGuild = long.Parse(reader["IDGUILD"].ToString());
                objDefense.IdPlayer = long.Parse(reader["IDPLAYER"].ToString());
                objDefense.IdSiege = long.Parse(reader["IDSIEGE"].ToString());

                objRetorno.Add(objDefense);
            }

            conexao.Close();
            conexao.Dispose();

            return objRetorno;
        }

        public void InserirSiegePlayerDefense(SiegePlayerDefesa obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();


            //Select
            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("MERGE DBO.SiegePlayerDefesa AS TARGET ");
            select.AppendLine("USING(SELECT @IdDeck AS IdDeck, @IdPlayerOponente as IdPlayerOponente,@IdPlayer as IdPlayer, @Vitoria as Vitoria, @Data as Data, @IdGuildOpp as IdGuildOpp,@IdSiege as IdSiege,@Base as Base) AS SOURCE ");
            select.AppendLine("ON TARGET.IdDeck = SOURCE.IdDeck and ");
            select.AppendLine("Target.IdPlayerOponente = SOURCE.IdPlayerOponente ");
            select.AppendLine("and Target.Base = SOURCE.Base ");
            select.AppendLine("and Target.Data = SOURCE.Data ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.Vitoria = @Vitoria, Data = @Data, Base = @Base ");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdDeck, IdPlayerOponente,IdPlayer, Vitoria, Data, IdGuildOpp ,Base, IdSiege) ");
            select.AppendLine("VALUES(@IdDeck, @IdPlayerOponente,@IdPlayer, @Vitoria, @Data, @IdGuildOpp, @Base, @IdSiege ) ");
            select.AppendLine("OUTPUT inserted.Id; ");

            command.Parameters.Add(new SqlParameter("@IdDeck", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdDeck"].Value = obj.IdDeck;

            command.Parameters.Add(new SqlParameter("@IdPlayerOponente", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayerOponente"].Value = obj.IdPlayerOponente;

            command.Parameters.Add(new SqlParameter("@IdPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdPlayer"].Value = obj.IdPlayer;

            command.Parameters.Add(new SqlParameter("@Vitoria", System.Data.SqlDbType.Int));
            command.Parameters["@Vitoria"].Value = obj.Vitoria;

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.DateTime));
            command.Parameters["@Data"].Value = obj.Date;

            command.Parameters.Add(new SqlParameter("@Base", System.Data.SqlDbType.Int));
            command.Parameters["@Base"].Value = obj.Base;

            command.Parameters.Add(new SqlParameter("@IdGuildOpp", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdGuildOpp"].Value = obj.IdGuildaOpp;

            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;


            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            long modified = (long)command.ExecuteScalar();

            conexao.Close();
            conexao.Dispose();

        }

        public void InserirSiegeTimeDefesa(SiegeTimeDefesa obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();


            //Select
            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("MERGE DBO.SiegeTimesDefesas AS TARGET ");

            if (obj.Base > 0)
            { select.AppendLine("USING(SELECT @IdDeck AS IdDeck, @Base as Base, @Monstro1 as Monstro1, @Monstro2 as Monstro2, @Monstro3 as Monstro3) AS SOURCE "); }
            else
            { select.AppendLine("USING(SELECT @IdDeck AS IdDeck, @Monstro1 as Monstro1, @Monstro2 as Monstro2, @Monstro3 as Monstro3) AS SOURCE "); }

            select.AppendLine("ON TARGET.IdDeck = SOURCE.IdDeck ");
            select.AppendLine("WHEN MATCHED THEN ");

            if (obj.Base > 0)
            { select.AppendLine("UPDATE SET TARGET.Base = @Base, Monstro1 = @Monstro1, Monstro2 = @Monstro2, Monstro3 = @Monstro3 "); }
            else
            { select.AppendLine("UPDATE SET Monstro1 = @Monstro1, Monstro2 = @Monstro2, Monstro3 = @Monstro3 "); }

            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");

            if (obj.Base > 0)
            { select.AppendLine("INSERT(IdDeck, Base, Monstro1, Monstro2, Monstro3) "); }
            else
            { select.AppendLine("INSERT(IdDeck, Monstro1, Monstro2, Monstro3) "); }

            if (obj.Base > 0)
            { select.AppendLine("VALUES(@IdDeck, @Base, @Monstro1, @Monstro2, @Monstro3); "); }
            else
            { select.AppendLine("VALUES(@IdDeck, @Monstro1, @Monstro2, @Monstro3); "); }

            command.Parameters.Add(new SqlParameter("@IdDeck", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdDeck"].Value = obj.IdDeck;

            if (obj.Base > 0)
            {
                command.Parameters.Add(new SqlParameter("@Base", System.Data.SqlDbType.Int));
                command.Parameters["@Base"].Value = obj.Base;
            }

            command.Parameters.Add(new SqlParameter("@Monstro1", System.Data.SqlDbType.BigInt));
            command.Parameters["@Monstro1"].Value = obj.Monstro1;


            command.Parameters.Add(new SqlParameter("@Monstro2", System.Data.SqlDbType.BigInt));
            command.Parameters["@Monstro2"].Value = obj.Monstro2;

            command.Parameters.Add(new SqlParameter("@Monstro3", System.Data.SqlDbType.BigInt));
            command.Parameters["@Monstro3"].Value = obj.Monstro3;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();

        }

        public void InserirSiegeDefenseDeckAssign(SiegeDefenseDeckAssign obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();


            //Select
            select.AppendLine("SET DATEFORMAT dmy; ");
            select.AppendLine("MERGE DBO.SiegeDefenseDeckAssign AS TARGET ");
            select.AppendLine("USING(SELECT @IdSiege AS IdSiege, @Base as Base, @IdDeck as IdDeck) AS SOURCE ");
            select.AppendLine("ON TARGET.IdSiege = SOURCE.IdSiege ");
            select.AppendLine("and Target.Base = SOURCE.Base ");
            select.AppendLine("and Target.IdDeck = SOURCE.IdDeck ");
            select.AppendLine("WHEN MATCHED THEN ");
            select.AppendLine("UPDATE SET TARGET.IdSiege = @IdSiege, Base = @Base, IdDeck = @IdDeck ");
            select.AppendLine("WHEN NOT MATCHED BY TARGET THEN ");
            select.AppendLine("INSERT(IdSiege, Base, IdDeck) ");
            select.AppendLine("VALUES(@IdSiege, @Base, @IdDeck); ");
            

            command.Parameters.Add(new SqlParameter("@IdSiege", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdSiege"].Value = obj.IdSiege;

            command.Parameters.Add(new SqlParameter("@Base", System.Data.SqlDbType.Int));
            command.Parameters["@Base"].Value = obj.Base;

            command.Parameters.Add(new SqlParameter("@IdDeck", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdDeck"].Value = obj.IdDeck;
                       

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
