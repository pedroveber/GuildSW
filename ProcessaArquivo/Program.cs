using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Dados;
using System.Threading;

namespace ProcessaArquivo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString() + ": Aguardando arquivos...");

            ProcessaArquivo();

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"C:\ArquivosProxy\Pendente";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*";
            
            watcher.Created += new FileSystemEventHandler(OnChanged);
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
            string strdocPath = @"C:\ArquivosProxy\Pendente";

            System.IO.DirectoryInfo diretorio = new DirectoryInfo(strdocPath);

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

                        Console.WriteLine(DateTime.Now.ToString() + ": Arquivo: " + item.Name + " processado com sucesso");
                        Console.WriteLine("");
                    }
                    if (item.Name.ToUpper().Contains("SIEGE"))
                    {
                        arqConhecido = true;
                        Console.WriteLine(DateTime.Now.ToString() + ": Processando arquivo: " + item.Name);
                        new Dados.BLO.BLO_Arquivo().CarregarSiege(item);

                        Console.WriteLine(DateTime.Now.ToString() + ": Arquivo: " + item.Name + " processado com sucesso");
                        Console.WriteLine("");
                    }
                    if (item.Name.ToUpper().Contains("DEF"))
                    {
                        arqConhecido = true;
                        Console.WriteLine(DateTime.Now.ToString() + ": Processando arquivo: " + item.Name);
                        new Dados.BLO.BLO_Arquivo().CarregarDefesas(item);

                        Console.WriteLine(DateTime.Now.ToString() + ": Arquivo: " + item.Name + " processado com sucesso");
                        Console.WriteLine("");
                    }

                    if(!arqConhecido)
                    {
                        System.IO.File.Move(item.FullName, @"C:\ArquivosProxy\ArquivoDesconhecido\" + item.Name);
                    }
                    
                }
                Thread.Sleep(1000);
                arquivos = diretorio.GetFiles();
            }
        }
    }
}
