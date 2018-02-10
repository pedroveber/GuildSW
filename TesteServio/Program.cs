using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TesteServio
{
    class Program
    {
        static void Main(string[] args)
        {
            EnviarArquivoServico();

            Console.ReadLine();
        }

        private static void EnviarArquivoServico()
        {
            string sFile= @"C:\Users\pbveber\Source\Repos\DB_SW\GuildSw\full_log erro.txt";

            FileStream objfilestream = new FileStream(sFile, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            Byte[] mybytearray = new Byte[len];
            objfilestream.Read(mybytearray, 0, len);

            ServiceReference1.WSArquivoSoapClient myService = new ServiceReference1.WSArquivoSoapClient();
                        
            myService.SaveDocument(mybytearray, "741852963");

            objfilestream.Close();

            //localhost.Service1 myservice = new localhost.Service1();
            //myservice.SaveDocument(mybytearray, sFile.Remove(0, sFile.LastIndexOf("\\") + 1));


        }
    }
    
}
