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
        public static Batalhas _SelectByIdDate(Batalhas _obj)
        {

            using (var ObjEntity = new DB_SW_GuildEntities())
            {
                //gambi.. tive que arrumar rapidao antes de ir embora
                Batalhas temp = _obj;
                temp = ObjEntity.Batalhas.Where(w => w.idGuilda == _obj.idGuilda && w.Data == _obj.Data).FirstOrDefault();
                if (temp == null)
                {
                    temp = _obj;
                }

                if (temp.ID == 0)
                {
                    DateTime tempDAta = Convert.ToDateTime(_obj.Data).AddDays(-1);
                    temp = ObjEntity.Batalhas.Where(w => w.idGuilda == _obj.idGuilda && w.Data == tempDAta).FirstOrDefault();
                }

                return temp;
            }
        }

        public static Batalhas Insert(Batalhas obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("INSERT INTO DBO.BATALHAS  (Guilda, Life, Data, PontuacaoOponente, PontuacaoGuild, RankGuild,idGUilda, IdGuildaAtacante) ");
            select.AppendLine("output INSERTED.ID values (@Guilda, @Life, @Data, @PontuacaoOponente, @PontuacaoGuild, @RankGuild,@idGUilda, @Id, @IdGuildaAtacante)");

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
            int modified = (int)command.ExecuteScalar();
            
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
            command.Parameters["@Id"].Value = obj.idGuilda;

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
