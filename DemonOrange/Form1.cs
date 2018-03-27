using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using Dados;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Dados.Models;
using Dados.DAO;
using System.Threading;


namespace DemonOrange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string PATH = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

        private void Form1_Load(object sender, EventArgs e)
        {
            timerGVG.Start();

            string[] lines;
            if (System.IO.File.Exists(PATH + @"config.txt"))
            {
                lines = System.IO.File.ReadAllLines(PATH + @"config.txt");
                txtDiretorio.Text = lines[0];
                if (System.IO.File.Exists(txtDiretorio.Text + @"//full_log.txt"))
                    System.IO.File.Delete(txtDiretorio.Text + @"//full_log.txt");
            }


        }


        //Tick do Timer da GVG
        private void btnVerificaArq_Click(object sender, EventArgs e)
        {
            Timer();
        }
        //Tick do Timer da Siege    
        private void timerSiege_Tick(object sender, EventArgs e)
        {
            TimerSiege();
        }

        void Timer()
        {
            ValidarArquivo();

        }
        void TimerSiege()
        {
            ValidarArquivoSiege();
        }

        #region ValidarArquivo


        private void ValidarArquivo()
        {
            btnAlimentaDB.Enabled = false;

            Boolean Arq1 = false;
            Boolean Arq2 = false;
            Boolean Arq3 = false;
            Boolean Arq4 = false;
            Boolean Arq5 = false;
            pl_1.BackColor = Color.Tomato;
            pl_2.BackColor = Color.Tomato;
            pl_3.BackColor = Color.Tomato;
            pl_4.BackColor = Color.Tomato;
            pl_5.BackColor = Color.Tomato;
            if (System.IO.File.Exists(txtDiretorio.Text + @"//full_log.txt"))
            {
                //string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//full_log.txt");

                FileStream fs = new FileStream(txtDiretorio.Text + @"//full_log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objReader = new StreamReader(fs);

                string str = objReader.ReadToEnd();
                fs.Close();
                fs.Dispose();


                //foreach (string line in lines)
                //{

                if (str.Contains("GetGuildWarContributeList") && str.Contains(@"ret_code"":0"))
                {
                    Arq1 = true;
                    pl_1.BackColor = Color.Green;
                }
                if (str.Contains("GetGuildWarBattleLogByGuildId") && str.Contains(@"ret_code"":0"))
                {
                    Arq2 = true;
                    pl_2.BackColor = Color.Green;
                }
                if (str.Contains("GetGuildWarMatchupInfo") && str.Contains(@"ret_code"":0"))
                {
                    Arq3 = true;
                    pl_3.BackColor = Color.Green;
                }
                if (str.Contains("GetGuildInfo") && str.Contains(@"ret_code"":0"))
                {
                    Arq4 = true;
                    pl_4.BackColor = Color.Green;
                }
                if (str.Contains("GetGuildWarParticipationInfo") && str.Contains(@"ret_code"":0"))
                {
                    Arq5 = true;
                    pl_5.BackColor = Color.Green;
                }

                if (str.Contains("GetGuildWarMatchLog") && str.Contains(@"ret_code"":0"))
                {
                    pnlMatchLogs.BackColor = Color.Green;
                }
                //}

                if (Arq1 && Arq2 && Arq3 && Arq4 && Arq5)
                {


                    btnAlimentaDB.Enabled = true;
                    lblErro.Text = "-";


                }
                else
                {
                    btnAlimentaDB.Enabled = false;
                    lblErro.Text = "Alguns Registro não forão encontrados";


                }

            }
            else

                lblErro.Text = "Arquivo não encontrado";

        }

        #endregion

        #region Validar Arquivo Siege

        private void ValidarArquivoSiege()
        {


            Boolean Arq1 = false;
            Boolean Arq2 = false;
            Boolean Arq3 = false;
            Boolean Arq4 = false;

            plSiegeMatchUp.BackColor = Color.Tomato;
            plSiegeDefenseDeck.BackColor = Color.Tomato;
            plSiegeBattleLog.BackColor = Color.Tomato;
            plSiegeMatchLog.BackColor = Color.Tomato;

            if (System.IO.File.Exists(txtDiretorio.Text + @"//full_log.txt"))
            {

                //string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//full_log.txt");
                //foreach (string line in lines)
                //{

                FileStream fs = new FileStream(txtDiretorio.Text + @"//full_log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader objReader = new StreamReader(fs);
                
                string str = objReader.ReadToEnd();

                fs.Close();
                fs.Dispose();

                if (str.Contains("GetGuildSiegeMatchupInfo") && str.Contains(@"ret_code"":0"))
                {
                    Arq1 = true;
                    plSiegeMatchUp.BackColor = Color.Green;
                }
                if (str.Contains("GetGuildSiegeDefenseDeckByWizardId") && str.Contains(@"ret_code"":0"))
                {
                    Arq2 = true;
                    plSiegeDefenseDeck.BackColor = Color.Green;
                }
                if (str.Contains("GetGuildSiegeBattleLog") && str.Contains(@"ret_code"":0"))
                {
                    Arq3 = true;
                    plSiegeBattleLog.BackColor = Color.Green;
                }
                if (str.Contains("GetGuildSiegeMatchLog") && str.Contains(@"ret_code"":0"))
                {
                    Arq4 = true;
                    plSiegeMatchLog.BackColor = Color.Green;
                }

                //}

                if (Arq1 && Arq2 && Arq3 && Arq4)
                {

                    btnAtualizarSiege.Enabled = true;
                    lblErroSiege.Text = "-";

                }
                else
                {

                    btnAtualizarSiege.Enabled = false;
                    lblErroSiege.Text = "Alguns Registro não forão encontrados";

                }

            }
            else

                btnAtualizarSiege.Enabled = false;
            lblErroSiege.Text = "Arquivo não encontrado";

        }

        #endregion

        private void btnAlimentaDB_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = false;
                btnAlimentaDB.Enabled = false;
                if (System.IO.File.Exists(txtDiretorio.Text + @"//tempDemonOrange.txt"))
                    System.IO.File.Delete(txtDiretorio.Text + @"//tempDemonOrange.txt");

                System.IO.File.Move(txtDiretorio.Text + @"//full_log.txt", txtDiretorio.Text + @"//tempDemonOrange.txt");

                timerGVG.Stop();

                pictureBox2.Enabled = true;
                PainelLoad(true, "Aguarde \nEnviando Arquivo para o Servidor");

                System.Threading.Thread t = new System.Threading.Thread(() => AtualizaGVG());
                t.Start();



                panel1.Enabled = true;
            }
            catch (Exception ex)
            {
                panel1.Enabled = true;

            }


        }

        public void PainelLoad(Boolean _painel, string _label)
        {
            pl_load.Visible = _painel;
            lbl_msnLoad.Text = _label;
            if (!_painel)
                ValidarArquivo();

        }

        public void PainelLoadSiege(Boolean _painel, string _label)
        {
            pl_loadSiege.Visible = _painel;
            lbl_msnLoadSiege.Text = _label;
            if (!_painel)
                ValidarArquivoSiege();


        }

        void AtualizaGVG()
        {

            try
            {


                //System.Threading.Thread t = new System.Threading.Thread(() => UpdateStatus("Aguarde \nEnviando Arquivo para o Servidor"));
                //t.Start();

                string sfile = @txtDiretorio.Text + "\\tempDemonOrange.txt";

                
                WSArquivo.WSArquivoSoapClient wsArquivo = new WSArquivo.WSArquivoSoapClient();


                FileStream objfilestream = new FileStream(sfile, FileMode.Open, FileAccess.Read);
                int len = (int)objfilestream.Length;
                Byte[] mybytearray = new Byte[len];
                objfilestream.Read(mybytearray, 0, len);

                //new Dados.BLO.BLO_Arquivo().CarregarGVG();
                wsArquivo.SaveDocument(mybytearray, "741852963", 1);

                objfilestream.Close();


                if (pictureBox2.InvokeRequired)
                {
                    this.Invoke(new HabilitaObjetosGVGDelegate(this.HabilitaObjetosGVG), new object[] { });
                    return;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private delegate void HabilitaObjetosGVGDelegate();
        private void HabilitaObjetosGVG()
        {
            if (pictureBox2.InvokeRequired)
            {
                this.Invoke(new HabilitaObjetosGVGDelegate(this.HabilitaObjetosGVG), new object[] { });
                return;
            }

            pictureBox2.Enabled = false;
            PainelLoad(true, "Arquivo enviado com sucesso. \nDentro de alguns minutos ficará disponivel \npara consulta no site.");
            ValidarArquivo();

            timerGVG.Start();

            if (System.IO.File.Exists(txtDiretorio.Text + @"//tempDemonOrange.txt"))
                System.IO.File.Delete(txtDiretorio.Text + @"//tempDemonOrange.txt");


        }


        private void btnLimparLog_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txtDiretorio.Text + @"//full_log.txt"))
                System.IO.File.Delete(txtDiretorio.Text + @"//full_log.txt");
        }



        private void chkAtualizacaoAutomatica_CheckedChanged(object sender, EventArgs e)
        {
            string[] conf = new string[2];
            conf[0] = txtDiretorio.Text;
            conf[1] = "0";
            System.IO.File.WriteAllLines(PATH + "\\config.txt", conf);
        }

        List<InfoBatalhaPlayer.BattleLogList> List = new List<InfoBatalhaPlayer.BattleLogList>();
        private void btnValidarArquivoDefesa_Click(object sender, EventArgs e)
        {
            string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//full_log.txt");

            List<PlayerDef> ListPlayer = new List<PlayerDef>();
            List = new List<InfoBatalhaPlayer.BattleLogList>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("GetGuildWarBattleLogByWizardId") && lines[i].Contains(@"ret_code"":0"))


                {
                    JavaScriptSerializer Player = new JavaScriptSerializer();
                    InfoBatalhaPlayer.Root ObjPlayer = Player.Deserialize<InfoBatalhaPlayer.Root>(lines[i]);
                    for (int j = 0; j < ObjPlayer.battle_log_list.Count; j++)
                    {
                        if (List.Where(w => w.battle_end == ObjPlayer.battle_log_list[j].battle_end && w.wizard_id == ObjPlayer.battle_log_list[j].wizard_id).FirstOrDefault() == null)
                            List.Add(ObjPlayer.battle_log_list[j]);
                        if (ListPlayer.Where(w => w.Nome == ObjPlayer.battle_log_list[j].wizard_name).FirstOrDefault() == null)
                            ListPlayer.Add(new PlayerDef() { Nome = ObjPlayer.battle_log_list[j].wizard_name });
                    }

                }
            }
            dtvArquivoDefesa.DataSource = ListPlayer;


        }



        private void btnEfetivarCargaDefesa_Click(object sender, EventArgs e)
        {
            lblMsgDefesa.Text = "Enviando arquivo para o Servidor.";

            string sfile = txtDiretorio.Text + "\\full_log.txt";

            WSArquivo.WSArquivoSoapClient wsArquivo = new WSArquivo.WSArquivoSoapClient();

            FileStream objfilestream = new FileStream(sfile, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            Byte[] mybytearray = new Byte[len];
            objfilestream.Read(mybytearray, 0, len);

            wsArquivo.SaveDocument(mybytearray, "741852963", 3);

            objfilestream.Close();


            lblMsgDefesa.Text = "Arquivo enviado com sucesso, dentro de alguns minutos estará disponível no site.";
        }


        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void btnAtualizarSiege_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = false;
                btnAtualizarSiege.Enabled = false;


                if (System.IO.File.Exists(txtDiretorio.Text + @"//tempDemonOrange.txt"))
                    System.IO.File.Delete(txtDiretorio.Text + @"//tempDemonOrange.txt");
                System.IO.File.Move(txtDiretorio.Text + @"//full_log.txt", txtDiretorio.Text + @"//tempDemonOrange.txt");

                timerSiege.Stop();
                PainelLoadSiege(true, "Aguarde \nEnviando Arquivo para o Servidor");

                pictureBox1.Enabled = true;

                System.Threading.Thread t = new System.Threading.Thread(() => AtualizarSiege());
                t.Start();

                panel1.Enabled = true;
            }
            catch (Exception)
            {
                panel1.Enabled = true;

            }

        }

        #region "Métodos de Carregar a Siege"

        private void AtualizarSiege()
        {

            //TODO: pegar da config
            string sfile = txtDiretorio.Text + "\\tempDemonOrange.txt";
            WSArquivo.WSArquivoSoapClient wsArquivo = new WSArquivo.WSArquivoSoapClient();

            FileStream objfilestream = new FileStream(sfile, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            Byte[] mybytearray = new Byte[len];
            objfilestream.Read(mybytearray, 0, len);

            //new Dados.BLO.BLO_Arquivo().CarregarSiege();
            wsArquivo.SaveDocument(mybytearray, "741852963", 2);

            objfilestream.Close();


            if (pictureBox1.InvokeRequired)
            {
                this.Invoke(new HabilitaObjetosGVGDelegate(this.HabilitaObjetosSiege), new object[] { });
                return;
            }


        }

        private delegate void HabilitaObjetosSiegeDelegate();
        private void HabilitaObjetosSiege()
        {
            if (pictureBox1.InvokeRequired)
            {
                this.Invoke(new HabilitaObjetosSiegeDelegate(this.HabilitaObjetosSiege), new object[] { });
                return;
            }

            PainelLoadSiege(true, "Arquivo enviado com sucesso. \nDentro de alguns minutos ficará disponivel \npara consulta no site.");
            pictureBox1.Enabled = false;

            ValidarArquivoSiege();

            timerSiege.Start();

            if (System.IO.File.Exists(txtDiretorio.Text + @"//tempDemonOrange.txt"))
                System.IO.File.Delete(txtDiretorio.Text + @"//tempDemonOrange.txt");


        }




        private void btnSalvarConfigs_Click(object sender, EventArgs e)
        {
            string[] conf = new string[2];
            conf[0] = txtDiretorio.Text;
            conf[1] = "0";
            System.IO.File.WriteAllLines(PATH + "\\config.txt", conf);
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    ValidarArquivo();
                    timerGVG.Start();
                    timerSiege.Stop();
                    break;
                case 1:
                    ValidarArquivoSiege();
                    timerGVG.Stop();
                    timerSiege.Start();
                    break;
                default:
                    timerGVG.Stop();
                    timerSiege.Stop();
                    break;
            }
        }
    }
    #endregion

    public class PlayerDef
    {
        public string Nome { get; set; }
    }
}