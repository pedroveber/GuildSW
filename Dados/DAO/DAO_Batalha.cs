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
    public class DAO_Batalha
    {
        public static List<Batalhas> _SelectAll()
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

                StringBuilder select = new StringBuilder();

                select.AppendLine("SET DATEFORMAT dmy;");
                select.AppendLine("Select * from dbo.Batalhas ");


                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;


                List<Batalhas> objRetorno = new List<Batalhas>();
                Batalhas objBatalha;

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objBatalha = new Batalhas();
                    objBatalha.Data = Convert.ToDateTime(reader["Data"].ToString());
                    objBatalha.Guilda = reader["Guilda"].ToString();
                    objBatalha.ID = long.Parse(reader["ID"].ToString());
                    objBatalha.idGuilda = long.Parse(reader["idGuilda"].ToString());
                    objBatalha.idGuildaAtacante = long.Parse(reader["idGuildaAtacante"].ToString());
                    objBatalha.Life = long.Parse(reader["Life"].ToString());
                    objBatalha.PontuacaoGuild = long.Parse(reader["PontuacaoGuild"].ToString());
                    objBatalha.PontuacaoOponente = long.Parse(reader["PontuacaoOponente"].ToString());
                    objBatalha.RankGuild = long.Parse(reader["RankGuild"].ToString());

                    //TODO: Fazer o ListarLutas
                    //objBatalha.Lutas

                    //TODO: Fazer o listar playeroponente
                    //objBatalha.PlayerOponente


                    objRetorno.Add(objBatalha);

                }

                conexao.Close();
                conexao.Dispose();

                return objRetorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Batalhas _SelectByID(long Id)
        {
            try
            {
                SqlConnection conexao = new SqlConnection();
                SqlCommand command = new SqlCommand();

                conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

                StringBuilder select = new StringBuilder();

                select.AppendLine("SET DATEFORMAT dmy;");
                select.AppendLine("Select * from dbo.Batalhas where id = @id");

                command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
                command.Parameters["@id"].Value = Id;

                command.CommandText = select.ToString();
                command.CommandType = System.Data.CommandType.Text;

                Batalhas objBatalha = new Batalhas();

                conexao.Open();
                command.Connection = conexao;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objBatalha = new Batalhas();
                    objBatalha.Data = Convert.ToDateTime(reader["Data"].ToString());
                    objBatalha.Guilda = reader["Guilda"].ToString();
                    objBatalha.ID = long.Parse(reader["ID"].ToString());
                    objBatalha.idGuilda = long.Parse(reader["idGuilda"].ToString());
                    objBatalha.idGuildaAtacante = long.Parse(reader["idGuildaAtacante"].ToString());
                    objBatalha.Life = long.Parse(reader["Life"].ToString());
                    objBatalha.PontuacaoGuild = long.Parse(reader["PontuacaoGuild"].ToString());
                    objBatalha.PontuacaoOponente = long.Parse(reader["PontuacaoOponente"].ToString());
                    objBatalha.RankGuild = long.Parse(reader["RankGuild"].ToString());

                    //TODO: Fazer o ListarLutas
                    //objBatalha.Lutas

                    //TODO: Fazer o listar playeroponente
                    //objBatalha.PlayerOponente

                }

                conexao.Close();
                conexao.Dispose();

                return objBatalha;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static Batalhas _SelectByIdDate(Batalhas obj)
        {
            //faz um select no Batalhas por Id guilda e Data (hoje ou ontem)
            //maior igual a onte ou maior igual a hoje

            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select Guilda,Life,Data,PontuacaoOponente,PontuacaoGuild,RankGuild,idGuilda,ID,IdGuildaAtacante from dbo.Batalhas ");
            select.AppendLine("where idGuilda = @idGuilda and (convert(Date, Data,103) = @Data or convert(Date, Data,103) = @Data2)");

            command.Parameters.Add(new SqlParameter("@idGuilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@idGuilda"].Value = obj.idGuilda;

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.DateTime));
            command.Parameters["@Data"].Value = obj.Data;

            command.Parameters.Add(new SqlParameter("@Data2", System.Data.SqlDbType.DateTime));
            command.Parameters["@Data2"].Value = Convert.ToDateTime(obj.Data).AddDays(-1);

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            Batalhas objBatalha = null;

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objBatalha = new Batalhas();
                objBatalha.ID = long.Parse(reader["ID"].ToString());
                objBatalha.Data = DateTime.Parse(reader["Data"].ToString());
                objBatalha.Guilda = reader["Guilda"].ToString();
                objBatalha.idGuilda = long.Parse(reader["idGuilda"].ToString());

                if (reader["IdGuildaAtacante"].ToString() != string.Empty)
                objBatalha.idGuildaAtacante = long.Parse(reader["IdGuildaAtacante"].ToString());

                if(reader["Life"].ToString() != string.Empty)
                objBatalha.Life = long.Parse(reader["Life"].ToString());

                if(reader["PontuacaoGuild"].ToString()!= string.Empty)
                objBatalha.PontuacaoGuild = long.Parse(reader["PontuacaoGuild"].ToString());

                if(reader["RankGuild"].ToString() != string.Empty)
                objBatalha.RankGuild = long.Parse(reader["RankGuild"].ToString());

                objBatalha.Lutas = DAO.DAO_Lutas._SelectAllByBatalha(objBatalha);
                
            }

            

            conexao.Close();
            conexao.Dispose();

            return objBatalha;


        }

        public static Batalhas Insert(Batalhas obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("INSERT INTO DBO.BATALHAS  (Guilda, Life, Data, PontuacaoOponente, PontuacaoGuild, RankGuild,idGUilda, IdGuildaAtacante) ");
            select.AppendLine("output INSERTED.ID values (@Guilda, @Life, @Data, @PontuacaoOponente, @PontuacaoGuild, @RankGuild,@idGUilda, @IdGuildaAtacante)");

            command.Parameters.Add(new SqlParameter("@Guilda", System.Data.SqlDbType.VarChar));
            command.Parameters["@Guilda"].Value = obj.Guilda;

            command.Parameters.Add(new SqlParameter("@life", System.Data.SqlDbType.BigInt));
            command.Parameters["@life"].Value = obj.Life;

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.DateTime));
            command.Parameters["@Data"].Value = obj.Data;

            command.Parameters.Add(new SqlParameter("@PontuacaoOponente", System.Data.SqlDbType.BigInt));
            command.Parameters["@PontuacaoOponente"].Value = obj.PontuacaoOponente;

            command.Parameters.Add(new SqlParameter("@PontuacaoGuild", System.Data.SqlDbType.BigInt));
            command.Parameters["@PontuacaoGuild"].Value = obj.PontuacaoGuild;

            command.Parameters.Add(new SqlParameter("@RankGuild", System.Data.SqlDbType.BigInt));
            command.Parameters["@RankGuild"].Value = obj.RankGuild;

            command.Parameters.Add(new SqlParameter("@idGUilda", System.Data.SqlDbType.BigInt));
            command.Parameters["@idGUilda"].Value = obj.idGuilda;

            command.Parameters.Add(new SqlParameter("@IdGuildaAtacante", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdGuildaAtacante"].Value = obj.idGuildaAtacante;


            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;


            conexao.Open();
            command.Connection = conexao;
            long modified = (long)command.ExecuteScalar();

            conexao.Close();
            conexao.Dispose();

            obj.ID = modified;
            return obj;

        }

        public static void AtualizarData(Batalhas obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("UPDATE DBO.BATALHAS SET Data =  @Data WHERE ID = @Id");

            command.Parameters.Add(new SqlParameter("@Data", System.Data.SqlDbType.DateTime));
            command.Parameters["@Data"].Value = obj.Data;

            command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.BigInt));
            command.Parameters["@Id"].Value = obj.ID;

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
