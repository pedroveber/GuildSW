using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace DemonOragemWeb
{
    /// <summary>
    /// Summary description for Upload
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Upload : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetJsom( List<string> Txt)
        {
            string[] arr4 = new string[Txt.Count];
            for (int i = 0; i < Txt.Count; i++)
            {
                arr4[i] = Txt[i];
            }
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(@"C:\SW\full_log.txt"))
            {
                foreach (string line in arr4)
                    outputFile.WriteLine(line);
            }
            return "Arquivo Enviado";
        }
        [WebMethod]
        public Boolean VerificarServico()
        {
            using (StreamWriter outputFile = new StreamWriter(@"C:\SW\Verifica.txt"))
            {
                outputFile.WriteLine("");
            }
            int count = 0;
            while (true)
            {
                if (File.Exists(@"C:\SW\Verifica.txt"))
                {
                    count++;
                    if (count > 9)
                    {
                        return false;
                    }
                    Thread.Sleep(1000);
                }
                else
                    return true;
            }
        }
        [WebMethod]
        public Boolean DandoCarga()
        {
            if (File.Exists(@"C:\SW\tempDemonOrange.txt"))
            {
                return true;
            }
            else
                return false;
        }
        [WebMethod]
        public string UltimaAtt()
        {
            if (File.Exists(@"C:\SW\UltAtt.txt"))
            {
                return File.ReadAllText(@"C:\SW\UltAtt.txt");
            }
            return "";
        }
    }   
}
