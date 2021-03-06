﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dados.Models;

namespace Dados.DAO
{
    public class DAO_PlayerOponente
    {
        public static PlayerOponente _SelectByID(PlayerOponente obj)
        {
            //ID
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("SET DATEFORMAT dmy;");
            select.AppendLine("select id,IdBatalha,Nome,Bonus from dbo.PlayerOponente ");
            select.AppendLine("where Id = @Id and IdBatalha = @IdBatalha");

            command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.BigInt));
            command.Parameters["@Id"].Value = obj.ID;

            command.Parameters.Add(new SqlParameter("@IdBatalha", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdBatalha"].Value = obj.IdBatalha;

            command.CommandText = select.ToString();
            command.CommandType = System.Data.CommandType.Text;

            PlayerOponente objPlayerOponente = null;

            conexao.Open();
            command.Connection = conexao;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                objPlayerOponente = new PlayerOponente();
                objPlayerOponente.ID = long.Parse(reader["ID"].ToString());
                objPlayerOponente.Bonus = Convert.ToInt32(reader["Bonus"].ToString());
                objPlayerOponente.IdBatalha = long.Parse(reader["IdBatalha"].ToString());
                objPlayerOponente.Nome = reader["Nome"].ToString();

                //TODO: lutas
                //objPlayerOponente.Lutas


                //todo: batalhas
                //objPlayerOponente.Batalhas


            }

            conexao.Close();
            conexao.Dispose();

            return objPlayerOponente;

        }
        public static PlayerOponente Insert(PlayerOponente obj)
        {
            SqlConnection conexao = new SqlConnection();
            SqlCommand command = new SqlCommand();

            conexao.ConnectionString = BLO.Conexao.ObterStringConexao2();

            StringBuilder select = new StringBuilder();

            select.AppendLine("insert into dbo.PlayerOponente (id,IdBatalha,Nome,Bonus) ");
            select.AppendLine("values (@id, @IdBatalha, @Nome, @Bonus)");

            command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt));
            command.Parameters["@id"].Value = obj.ID;

            command.Parameters.Add(new SqlParameter("@IdBatalha", System.Data.SqlDbType.BigInt));
            command.Parameters["@IdBatalha"].Value = obj.IdBatalha;

            command.Parameters.Add(new SqlParameter("@Nome", System.Data.SqlDbType.VarChar));
            command.Parameters["@Nome"].Value = obj.Nome;

            command.Parameters.Add(new SqlParameter("@Bonus", System.Data.SqlDbType.Int));
            command.Parameters["@Bonus"].Value = obj.Bonus;

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
