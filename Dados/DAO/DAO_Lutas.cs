﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dados.Models;
using System.Data.SqlClient;

namespace Dados.DAO
{
    public class DAO_Lutas
    {
        public static Lutas _SelectByPlayer_Oponente(Lutas obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select id,CodBatalhas,CodPlayer,CodPlayerOponente,Vitoria,ValorBarra,DataHora,MomentoVitoria from dbo.Lutas ");
            select.AppendLine("where CodBatalhas=@CodBatalhas and CodPlayer=@CodPlayer and CodPlayerOponente=@CodPlayerOponente");

            command.Parameters.Add(new SqlParameter("@CodBatalhas", System.Data.SqlDbType.BigInt));
            command.Parameters["@CodBatalhas"].Value = obj.CodBatalhas;

            command.Parameters.Add(new SqlParameter("@CodPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@CodPlayer"].Value = obj.CodPlayer;

            command.Parameters.Add(new SqlParameter("@CodPlayerOponente", System.Data.SqlDbType.BigInt));
            command.Parameters["@CodPlayerOponente"].Value = obj.CodPlayerOponente;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            Lutas objLuta=null;

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objLuta = new Lutas();
                objLuta.ID = long.Parse(reader["ID"].ToString());
                objLuta.CodBatalhas = long.Parse(reader["CodBatalhas"].ToString());
                objLuta.CodPlayer = long.Parse(reader["CodPlayer"].ToString());
                objLuta.CodPlayerOponente = long.Parse(reader["CodPlayerOponente"].ToString());
                objLuta.DataHora = Convert.ToDateTime(reader["DataHora"].ToString());
                objLuta.MomentoVitoria = reader["MomentoVitoria"].ToString();
                objLuta.ValorBarra = long.Parse(reader["ValorBarra"].ToString());
                objLuta.Vitoria = int.Parse(reader["Vitoria"].ToString());


                //todo: fazer o get playerOPonente
                //objLuta.PlayerOponente

                //todo: fazer o get player
                //objLuta.Player

            }

            conexao.Close();
            conexao.Dispose();

            return objLuta;


        }
        public static List<Lutas> _SelectAllByBatalha(Batalhas obj)
        {
            //codbatalha = obj.id
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select id,CodBatalhas,CodPlayer,CodPlayerOponente,Vitoria,ValorBarra,DataHora,MomentoVitoria from dbo.Lutas ");
            select.AppendLine("where CodBatalhas=@CodBatalhas");

            command.Parameters.Add(new SqlParameter("@CodBatalhas", System.Data.SqlDbType.BigInt));
            command.Parameters["@CodBatalhas"].Value = obj.ID;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            Lutas objLuta = new Lutas();
            List<Lutas> lstLutas = new List<Lutas>();

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objLuta = new Lutas();
                objLuta.ID = long.Parse(reader["ID"].ToString());

                if (reader["CodBatalhas"].ToString() != string.Empty)
                    objLuta.CodBatalhas = long.Parse(reader["CodBatalhas"].ToString());

                if (reader["CodPlayer"].ToString() != string.Empty)
                    objLuta.CodPlayer = long.Parse(reader["CodPlayer"].ToString());

                if (reader["CodPlayerOponente"].ToString() != string.Empty)
                    objLuta.CodPlayerOponente = long.Parse(reader["CodPlayerOponente"].ToString());

                if (reader["DataHora"].ToString() != string.Empty)
                    objLuta.DataHora = Convert.ToDateTime(reader["DataHora"].ToString());

                if (reader["MomentoVitoria"].ToString() != string.Empty)
                    objLuta.MomentoVitoria = reader["MomentoVitoria"].ToString();

                if (reader["ValorBarra"].ToString() != string.Empty)
                    objLuta.ValorBarra = long.Parse(reader["ValorBarra"].ToString());

                if (reader["Vitoria"].ToString() != string.Empty)
                    objLuta.Vitoria = int.Parse(reader["Vitoria"].ToString());


                //todo: fazer o get playerOPonente
                //objLuta.PlayerOponente

                //todo: fazer o get player
                //objLuta.Player

                lstLutas.Add(objLuta);

            }

            conexao.Close();
            conexao.Dispose();

            return lstLutas;


        }

        public static Lutas Insert(Lutas obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("insert into dbo.Lutas (CodBatalhas,CodPlayer,CodPlayerOponente,Vitoria,ValorBarra,DataHora,MomentoVitoria) ");
            select.AppendLine("output INSERTED.ID values (@CodBatalhas, @CodPlayer, @CodPlayerOponente, @Vitoria, @ValorBarra, @DataHora,@MomentoVitoria)");

            command.Parameters.Add(new SqlParameter("@CodBatalhas", System.Data.SqlDbType.BigInt));
            command.Parameters["@CodBatalhas"].Value = obj.CodBatalhas;

            command.Parameters.Add(new SqlParameter("@CodPlayer", System.Data.SqlDbType.BigInt));
            command.Parameters["@CodPlayer"].Value = obj.CodPlayer;

            command.Parameters.Add(new SqlParameter("@CodPlayerOponente", System.Data.SqlDbType.BigInt));
            command.Parameters["@CodPlayerOponente"].Value = obj.CodPlayerOponente;

            command.Parameters.Add(new SqlParameter("@Vitoria", System.Data.SqlDbType.Int));
            command.Parameters["@Vitoria"].Value = obj.Vitoria;

            command.Parameters.Add(new SqlParameter("@ValorBarra", System.Data.SqlDbType.BigInt));
            command.Parameters["@ValorBarra"].Value = obj.ValorBarra;

            command.Parameters.Add(new SqlParameter("@DataHora", System.Data.SqlDbType.DateTime));
            command.Parameters["@DataHora"].Value = obj.DataHora;

            command.Parameters.Add(new SqlParameter("@MomentoVitoria", System.Data.SqlDbType.VarChar));
            command.Parameters["@MomentoVitoria"].Value = obj.MomentoVitoria;


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
        public static void ApagaTudoByBatalha(long _idBatalha)
        {
           
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("DELETE FROM Lutas where CodBatalhas = @idBatalha ");

            command.Parameters.Add(new SqlParameter("@idBatalha", System.Data.SqlDbType.BigInt));
            command.Parameters["@idBatalha"].Value = _idBatalha;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            conexao.Open();
            command.Connection = conexao;
            command.ExecuteNonQuery();

            conexao.Close();
            conexao.Dispose();

        }

        public static void AtualizarVitoria(long idBatalha)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("update dbo.Lutas set dbo.Lutas.MomentoVitoria = b.momento ");
            select.AppendLine("from( ");
            select.AppendLine("select 'Win' momento, ");
            select.AppendLine("id from ");
            select.AppendLine("dbo.Lutas ");
            select.AppendLine("where ");
            select.AppendLine("id = (select max(id) from dbo.lutas where CodBatalhas = @codBatalha) ");
            select.AppendLine(") b ");
            select.AppendLine("where b.ID = dbo.Lutas.ID ");

            command.Parameters.Add(new SqlParameter("@codBatalha", System.Data.SqlDbType.BigInt));
            command.Parameters["@codBatalha"].Value = idBatalha;

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
