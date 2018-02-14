using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Dados;
using System.Threading;
using System.Configuration;

namespace ProcessaArquivo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString() + ": Aguardando arquivos...");

            ProcessaArquivo();

            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = ConfigurationManager.AppSettings["LocalPastaPendente"].ToString();

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*";
            
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("");
            Console.WriteLine("Press \'q\' to quit...");
            while (Console.Read() != 'q') ;
            
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            ProcessaArquivo();
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine(DateTime.Now.ToString() + ": Aguardando arquivos...");
        }
        

        private static void ProcessaArquivo()
        {
            try
            {

            
            string strCaminhoPendente = ConfigurationManager.AppSettings["LocalPastaPendente"].ToString();
            string strCaminhoProcessado = ConfigurationManager.AppSettings["LocalPastaProcessado"].ToString();
            string strCaminhoDesconhecido= ConfigurationManager.AppSettings["LocalPastaDesconhecido"].ToString();

            System.IO.DirectoryInfo diretorio = new DirectoryInfo(strCaminhoPendente);
            
            FileInfo[] arquivos = diretorio.GetFiles();

            
            while (arquivos.Count() > 0)
            {
                Console.WriteLine(DateTime.Now.ToString() + ": Quantidade de arquivos na pasta: " + arquivos.Count());

                //Ler arquivo
                bool arqConhecido = false;
                foreach (FileInfo item in arquivos)
                {
                    if (item.Name.ToUpper().Contains("GVG"))
                    {
                        arqConhecido = true;
                        Console.WriteLine(DateTime.Now.ToString() + ": Processando arquivo: " + item.Name);
                        new Dados.BLO.BLO_Arquivo().CarregarGVG(item);

                        System.IO.File.Move(item.FullName, strCaminhoProcessado + item.Name);

                        Console.WriteLine(DateTime.Now.ToString() + ": Arquivo: " + item.Name + " processado com sucesso");
                        Console.WriteLine("");
                    }
                    if (item.Name.ToUpper().Contains("SIEGE"))
                    {
                        arqConhecido = true;
                        Console.WriteLine(DateTime.Now.ToString() + ": Processando arquivo: " + item.Name);
                        new Dados.BLO.BLO_Arquivo().CarregarSiege(item);
                        System.IO.File.Move(item.FullName, strCaminhoProcessado + item.Name);

                        Console.WriteLine(DateTime.Now.ToString() + ": Arquivo: " + item.Name + " processado com sucesso");
                        Console.WriteLine("");
                    }
                    if (item.Name.ToUpper().Contains("DEF"))
                    {
                        arqConhecido = true;
                        Console.WriteLine(DateTime.Now.ToString() + ": Processando arquivo: " + item.Name);
                        new Dados.BLO.BLO_Arquivo().CarregarDefesas(item);
                        System.IO.File.Move(item.FullName, strCaminhoProcessado + item.Name);

                        Console.WriteLine(DateTime.Now.ToString() + ": Arquivo: " + item.Name + " processado com sucesso");
                        Console.WriteLine("");
                    }

                    if(!arqConhecido)
                    {
                        System.IO.File.Move(item.FullName, strCaminhoDesconhecido + item.Name);
                    }
                    
                }
                Thread.Sleep(1000);
                arquivos = diretorio.GetFiles();
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
