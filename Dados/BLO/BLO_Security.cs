using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Dados.BLO
{
    public static class BLO_Security
    {
        public static string Criptografar(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("pedrocalindo"));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }

        public static string Descriptografar(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("pedrocalindo"));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }
    }

    public static class Conexao
    {
        public static string ObterStringConexao()
        {
            string conn;
            conn = System.Configuration.ConfigurationManager.ConnectionStrings["DB_SW_GuildEntities"].ToString();
            //conn = conn.Substring(conn.IndexOf("password=") + 9, conn.Length - (conn.IndexOf("password=") + 9));
            conn = conn.Substring(conn.IndexOf("password=") + 9, 24);
            //Tenho o password decriptografado
            conn = BLO_Security.Descriptografar(conn);

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DB_SW_GuildEntities"].ToString();
            //conexao = conexao.Replace(conexao.Substring(conexao.IndexOf("password=") + 9, conexao.Length - (conexao.IndexOf("password=") + 9)), conn);
            conexao = conexao.Replace(conexao.Substring(conexao.IndexOf("password=") + 9, 24), conn);

            return conexao;

        }
        //estou com pressa. 


        //DESCOMENTAR DEPOIS Q TESTAR
        public static string ObterStringConexao2()
        {
            //TODO: descomentar depois de terminar os testes
            string conn;
            conn = System.Configuration.ConfigurationManager.ConnectionStrings["DB_SW_ADONET"].ToString();
            conn = conn.Substring(conn.IndexOf("password=") + 9, conn.Length - (conn.IndexOf("password=") + 9));

            //Tenho o password decriptografado
            conn = BLO_Security.Descriptografar(conn);

            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DB_SW_ADONET"].ToString();
            conexao = conexao.Replace(conexao.Substring(conexao.IndexOf("password=") + 9, conexao.Length - (conexao.IndexOf("password=") + 9)), conn);

            return conexao;

        }

        //public static string ObterStringConexao2()
        //{
        //    string conn;
        //    conn = System.Configuration.ConfigurationManager.ConnectionStrings["DB_SW_ADONET"].ToString();
        //    return conn;
        //}
    }

}

