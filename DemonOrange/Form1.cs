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
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Dados.Models;
using Dados.DAO;


namespace DemonOrange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string PATH = System.AppDomain.CurrentDomain.BaseDirectory.ToString();

        private void btnSalvarConfigs_Click(object sender, EventArgs e)
        {
            string[] conf = new string[2];
            conf[0] = txtDiretorio.Text;
            if (chkAtualizacaoAutomatica.Checked == true)
            { conf[1] = "1"; }
            else
            { conf[1] = "0"; }
            System.IO.File.WriteAllLines(PATH + "\\config.txt", conf);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread YhdIniciar = new Thread(() => TimerIni());
            YhdIniciar.Start();

            string[] lines;
            if (System.IO.File.Exists(PATH + @"config.txt"))
            {
                lines = System.IO.File.ReadAllLines(PATH + @"config.txt");
                txtDiretorio.Text = lines[0];
                if (lines[1] == "1")
                    chkAtualizacaoAutomatica.Checked = true;
                else
                    chkAtualizacaoAutomatica.Checked = false;
                if (System.IO.File.Exists(txtDiretorio.Text + @"//full_log.txt"))
                    System.IO.File.Delete(txtDiretorio.Text + @"//full_log.txt");
            }


        }


        private void btnVerificaArq_Click(object sender, EventArgs e)
        {
            Thread YhdIniciar = new Thread(() => Timer());
            YhdIniciar.Start();
        }
        void TimerIni()
        {
            while (true)
            {
                Thread.Sleep(2000);
                Thread YhdIniciar = new Thread(() => Timer());
                YhdIniciar.Start();
            }
        }
        void Timer()
        {
            ValidarArquivo(true);
            ValidarArquivoSiege(true);
        }
        private void ValidarArquivo(bool timer)
        {
            if (timer)
            { btnAlimentaDB.Invoke(new MethodInvoker(delegate { btnAlimentaDB.Enabled = false; })); }
            else
            { btnAlimentaDB.Enabled = false; }

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
                string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//full_log.txt");
                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildWarContributeList") && line.Contains(@"ret_code"":0"))
                    {
                        Arq1 = true;
                        pl_1.BackColor = Color.Green;
                    }
                    if (line.Contains("GetGuildWarBattleLogByGuildId") && line.Contains(@"ret_code"":0"))
                    {
                        Arq2 = true;
                        pl_2.BackColor = Color.Green;
                    }
                    if (line.Contains("GetGuildWarMatchupInfo") && line.Contains(@"ret_code"":0"))
                    {
                        Arq3 = true;
                        pl_3.BackColor = Color.Green;
                    }
                    if (line.Contains("GetGuildInfo") && line.Contains(@"ret_code"":0"))
                    {
                        Arq4 = true;
                        pl_4.BackColor = Color.Green;
                    }
                    if (line.Contains("GetGuildWarParticipationInfo") && line.Contains(@"ret_code"":0"))
                    {
                        Arq5 = true;
                        pl_5.BackColor = Color.Green;
                    }
                }
                if (Arq1 && Arq2 && Arq3 && Arq4 && Arq5)
                {

                    if (timer)
                    {
                        btnAlimentaDB.Invoke(new MethodInvoker(delegate { btnAlimentaDB.Enabled = true; }));
                        btnEnviarLog.Invoke(new MethodInvoker(delegate { btnEnviarLog.Enabled = true; }));
                        lblErro.Invoke(new MethodInvoker(delegate { lblErro.Text = "-"; }));
                    }
                    else
                    {
                        btnAlimentaDB.Enabled = true;
                        btnEnviarLog.Enabled = true;
                        lblErro.Text = "-";
                    }




                    if (chkAtualizacaoAutomatica.Checked)
                    {
                        if (System.IO.File.Exists(txtDiretorio.Text + @"//tempDemonOrange.txt"))
                            System.IO.File.Delete(txtDiretorio.Text + @"//tempDemonOrange.txt");
                        System.IO.File.Move(txtDiretorio.Text + @"//full_log.txt", txtDiretorio.Text + @"//tempDemonOrange.txt");
                        Thread YhdIniciar = new Thread(() => Autorizacao());
                        YhdIniciar.Start();
                    }
                }
                else
                {
                    if (timer)
                    {
                        btnAlimentaDB.Invoke(new MethodInvoker(delegate { btnAlimentaDB.Enabled = false; }));
                        btnEnviarLog.Invoke(new MethodInvoker(delegate { btnEnviarLog.Enabled = false; }));
                        lblErro.Invoke(new MethodInvoker(delegate { lblErro.Text = "Alguns Registro não forão encontrados"; }));
                    }
                    else
                    {
                        btnAlimentaDB.Enabled = false;
                        btnEnviarLog.Enabled = false;
                        lblErro.Text = "Alguns Registro não forão encontrados";
                    }


                }

            }
            else
        if (timer)
            {
                lblErro.Invoke(new MethodInvoker(delegate { lblErro.Text = "Arquivo não encontrado"; }));
            }
            else
            {
                lblErro.Text = "Arquivo não encontrado";
            }

        }

        private void ValidarArquivoSiege(bool timer)
        {
            if (timer)
            { btnAtualizarSiege.Invoke(new MethodInvoker(delegate { btnAtualizarSiege.Enabled = false; })); }
            else
            { btnAtualizarSiege.Enabled = false; }


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
                string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//full_log.txt");
                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildSiegeMatchupInfo") && line.Contains(@"ret_code"":0"))
                    {
                        Arq1 = true;
                        plSiegeMatchUp.BackColor = Color.Green;
                    }
                    if (line.Contains("GetGuildSiegeDefenseDeckByWizardId") && line.Contains(@"ret_code"":0"))
                    {
                        Arq2 = true;
                        plSiegeDefenseDeck.BackColor = Color.Green;
                    }
                    if (line.Contains("GetGuildSiegeBattleLog") && line.Contains(@"ret_code"":0"))
                    {
                        Arq3 = true;
                        plSiegeBattleLog.BackColor = Color.Green;
                    }
                    if (line.Contains("GetGuildSiegeMatchLog") && line.Contains(@"ret_code"":0"))
                    {
                        Arq4 = true;
                        plSiegeMatchLog.BackColor = Color.Green;
                    }

                }
                if (Arq1 && Arq2 && Arq3 && Arq4)
                {
                    if (timer)
                    {
                        btnAtualizarSiege.Invoke(new MethodInvoker(delegate { btnAtualizarSiege.Enabled = true; }));
                        lblErroSiege.Invoke(new MethodInvoker(delegate { lblErroSiege.Text = "-"; }));
                    }
                    else
                    {
                        btnAtualizarSiege.Enabled = true;
                        lblErroSiege.Text = "-";
                    }



                }
                else
                {

                    if (timer)
                    {
                        btnAtualizarSiege.Invoke(new MethodInvoker(delegate { btnAtualizarSiege.Enabled = false; }));
                        lblErroSiege.Invoke(new MethodInvoker(delegate { lblErroSiege.Text = "Alguns Registro não forão encontrados"; }));
                    }
                    else
                    {
                        btnAtualizarSiege.Enabled = false;
                        lblErroSiege.Text = "Alguns Registro não forão encontrados";
                    }

                }

            }
            else
                 if (timer)
            {
                lblErroSiege.Invoke(new MethodInvoker(delegate { lblErroSiege.Text = "Arquivo não encontrado"; }));
            }
            else
            {
                lblErroSiege.Text = "Arquivo não encontrado";
            }
        }

        private void btnAlimentaDB_Click(object sender, EventArgs e)
        {

            if (System.IO.File.Exists(txtDiretorio.Text + @"//tempDemonOrange.txt"))
                System.IO.File.Delete(txtDiretorio.Text + @"//tempDemonOrange.txt");
            System.IO.File.Move(txtDiretorio.Text + @"//full_log.txt", txtDiretorio.Text + @"//tempDemonOrange.txt");
            Thread YhdIniciar = new Thread(() => Autorizacao());
            YhdIniciar.Start();
        }

        public void PainelLoad(Boolean _painel, string _label, string _label2, Boolean _btnCancelar)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Boolean, string, string, Boolean>(PainelLoad), new object[] { _painel, _label, _label2, _btnCancelar });
                return;
            }
            pl_load.Visible = _painel;
            lbl_msnLoad.Text = _label;
            lbl_msnLoad2.Text = _label2;
            tabControl1.Enabled = !_painel;
            if (!_painel)
                ValidarArquivoSiege(false);


        }

        public void PainelLoadSiege(Boolean _painel, string _label, string _label2, Boolean _btnCancelar)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Boolean, string, string, Boolean>(PainelLoad), new object[] { _painel, _label, _label2, _btnCancelar });
                return;
            }
            pl_loadSiege.Visible = _painel;
            lbl_msnLoadSiege.Text = _label;
            lbl_msnLoad2Siege.Text = _label2;
            tabControl1.Enabled = !_painel;
            if (!_painel)
                ValidarArquivo(false);


        }


        void Autorizacao()
        {
            try
            {


                PainelLoad(true, "Aguarde", "Iniciando Processo!", false);

                FazerBackupArquivo();

                string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//tempDemonOrange.txt");

                // ---- Leitura do Arquivo FullLog ----// 

                //GuildWarContributeList
                InfoPlayer.Root ObjPlayer = LerGuildWarContributeList(lines);

                //BattleLogByGuildId
                InfoBatalha ObjBatalha = LerGuildWarBattleLogByGuildId(lines);

                //MatchupInfo
                InfoOponente.Root ObjOponente = LerGuildWarMatchupInfo(lines);

                //GuildInfo
                InfoOtherInfoPlayer.Root ObjPlayOtherInfo = LerGuildInfo(lines);

                //ParticipationInfo
                InfoParticipantes.Root ObjParticipante = LerGuildWarParticipationInfo(lines);

                //GetGuildWarMatchLog
                InfoLogBatalhas.Root objLogBatalhas = LerGuildWarMatchLog(lines);


                PainelLoad(true, "Leitura de arquivo concluido", "-", false);

                long CodigoBatalha = 0;

                //Para cada GVG
                for (int i = 0; i < ObjBatalha.battle_log_list_group.Count; i++)
                {

                    //Cadastrar a Guilda
                    CadastrarGuilda(ObjBatalha);

                    PainelLoad(true, "Cadastrando a Batalha", ObjBatalha.battle_log_list_group[i].opp_guild_info.name, false);


                    //Se 0 = GVG atual. Incluir os dados dos players e oponentes. 
                    if (i == 0)
                    {

                        //Batalha
                        CodigoBatalha = CadastrarBatalha(ObjBatalha, ObjOponente, ObjParticipante);

                        //Player
                        CadastrarPlayer(ObjPlayer, CodigoBatalha, ObjBatalha);

                        //PlayerStatus
                        CadastrarPlayerStatus(ObjOponente, ObjPlayer, CodigoBatalha);

                        //Oponentes
                        CadastrarOponentes(ObjOponente, CodigoBatalha);

                    }


                    // Unix timestamp is seconds past epoch
                    System.DateTime dBatalha2 = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dBatalha2 = dBatalha2.AddSeconds(ObjBatalha.battle_log_list_group[i].battle_log_list[0].battle_end).ToLocalTime();

                    Dados.Models.Batalhas Batalha = new Dados.BLO.BLO_Batalha().SelectByIdDate(new Dados.Models.Batalhas() { idGuilda = ObjBatalha.battle_log_list_group[i].opp_guild_info.guild_id, Data = Convert.ToDateTime(dBatalha2.ToShortDateString()) });

                    long VidaClan = Batalha != null ? Batalha.Life.Value : 0;
                    int count = 0;

                    if (Batalha != null)
                    {
                        Dados.DAO.DAO_Lutas.ApagaTudoByBatalha(Batalha.ID);
                        for (int j = ObjBatalha.battle_log_list_group[i].battle_log_list.Count - 1; j >= 0; j--)
                        {
                            count++;

                            //Luta
                            VidaClan = CadastrarLuta(ObjBatalha, count, i, j, VidaClan, Batalha, ObjOponente);
                        }
                    }
                }


                PainelLoad(true, "Finalizando", "Aguarde...", false);
                for (int j = 0; j < ObjBatalha.battle_log_list_group.Count; j++)
                {

                    System.DateTime dataBatalha = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dataBatalha = dataBatalha.AddSeconds(ObjBatalha.battle_log_list_group[j].battle_log_list[0].battle_end).ToLocalTime();

                    Dados.Models.Batalhas Obj = new Dados.BLO.BLO_Batalha().SelectByIdDate(new Dados.Models.Batalhas() { idGuilda = ObjBatalha.battle_log_list_group[j].opp_guild_info.guild_id, Data = Convert.ToDateTime(dataBatalha.ToShortDateString()) });
                    if (Obj != null)
                    {
                        List<Dados.Models.Lutas> ObjLuta = Dados.DAO.DAO_Lutas._SelectAllByBatalha(Obj).OrderBy(w => w.DataHora).ToList();
                        Obj.Data = ObjLuta[0].DataHora;
                        Dados.DAO.DAO_Batalha.AtualizarData(Obj);
                    }
                }

                try
                {
                    //Atualizar MatchLogs
                    for (int i = 0; i < objLogBatalhas.match_log.Count; i++)
                    {
                        System.DateTime fimGVG = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        fimGVG = fimGVG.AddSeconds(objLogBatalhas.match_log[i].match_end).ToLocalTime();

                        if (objLogBatalhas.match_log[i].log_type == 1 && objLogBatalhas.match_log[i].win_lose == 1)
                        {
                            //Se encontrar a GVG atualiza o Win
                            Dados.Models.Batalhas filtro = new Batalhas() { idGuilda = objLogBatalhas.match_log[i].opp_guild_id, Data = new DateTime(fimGVG.Year, fimGVG.Month, fimGVG.Day) };
                            Dados.Models.Batalhas objBatalha = DAO_Batalha._SelectByIdDate(filtro);

                            if (objBatalha != null)
                            {
                                //Atualiza o Win no dboLutas
                                DAO_Lutas.AtualizarVitoria(objBatalha.ID);
                            }
                        }


                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    //Atualizar DEFESAS GVG
                    if (CodigoBatalha > 0)
                    {
                        CarregarDefesasGVG(CodigoBatalha, ObjBatalha.battle_log_list_group[0].opp_guild_info.guild_id);
                    }

                }
                catch (Exception)
                {

                }



                if (System.IO.File.Exists(txtDiretorio.Text + @"//tempDemonOrange.txt"))
                    System.IO.File.Delete(txtDiretorio.Text + @"//tempDemonOrange.txt");
                PainelLoad(false, "-", "-", false);
                System.IO.File.WriteAllText(PATH + "\\UltAtt.txt", "Ultima Att no Servidor VPC: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }





        private void btnLimparLog_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txtDiretorio.Text + @"//full_log.txt"))
                System.IO.File.Delete(txtDiretorio.Text + @"//full_log.txt");
        }


        #region Enviar Arquivo Servidor
        void EviarArq()
        {
            PainelLoad(true, "Aguarde", "Verificando Servidor!", false);
            string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//full_log.txt");
            ServiceReference.UploadSoapClient wsUpload = new ServiceReference.UploadSoapClient();
            DemonOrange.ServiceReference.ArrayOfString obj = new ServiceReference.ArrayOfString();
            if (wsUpload.VerificarServico())
            {
                PainelLoad(true, "Aguarde", "Enviando Arquivo!", false);
                foreach (string line in lines)
                {
                    obj.Add(line);
                }
                string txt = wsUpload.GetJsom(obj);
                PainelLoad(true, "Aguarde", "Arquivo Enviando, Iniciando Carga!", false);
                while (true)
                {
                    Thread.Sleep(2000);
                    if (!wsUpload.DandoCarga())
                    {
                        break;
                    }
                }
                PainelLoad(false, "-", "-", false);
            }
            else
            {
                PainelLoad(true, "Aviso", "Servidor Off", false);
                Thread.Sleep(5000);
                PainelLoad(false, "-", "-", false);
            }
            System.IO.File.Delete(txtDiretorio.Text + @"//full_log.txt");
        }

        private void chkAtualizacaoAutomatica_CheckedChanged(object sender, EventArgs e)
        {
            string[] conf = new string[2];
            conf[0] = txtDiretorio.Text;
            if (chkAtualizacaoAutomatica.Checked == true)
            { conf[1] = "1"; }
            else
            { conf[1] = "0"; }
            System.IO.File.WriteAllLines(PATH + "\\config.txt", conf);
        }
        #endregion


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

        private void CarregarDefesasGVG(long idBatalha, long idGuilda)
        {
            string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//tempDemonOrange.txt");
            string Texto = "";

            List<InfoDefesas.Root> lstDefesas = new List<InfoDefesas.Root>();
            Dados.DAO.DAO_TimeDefesa daoTimeDefesa = new Dados.DAO.DAO_TimeDefesa();
            try
            {
                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildWarDefenseUnits") && line.Contains(@"ret_code"":0"))
                    {
                        Texto = "{\"" + line.Substring(line.IndexOf("ret_code"), line.Length - line.IndexOf("ret_code"));

                        JavaScriptSerializer Defesas = new JavaScriptSerializer();
                        InfoDefesas.Root objDefesa = Defesas.Deserialize<InfoDefesas.Root>(Texto);

                        if (objDefesa.guild_id == idGuilda)
                        {
                            lstDefesas.Add(objDefesa);
                        }

                    }
                }
                //var distinctItems = items.GroupBy(x => x.Id).Select(y => y.First());
                lstDefesas = lstDefesas.GroupBy(x => x.defense_wizard_id).Select(y => y.First()).ToList();

                daoTimeDefesa.AtualizarTimeDefesaGVG(lstDefesas, idBatalha);
            }
            catch (Exception ex)
            {
                throw ex;
                lblMsgDefesa.Text += Environment.NewLine + "Erro ao tentar incluir Time Defesa GVG";

            }



        }

        void Defesas()
        {
            for (int j = 0; j < List.Count; j++)
            {
                PainelLoad(true, "Cadastrando os oponentes", (j + 1).ToString() + "/" + List.Count.ToString(), false);
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(List[j].battle_end).ToLocalTime();
                new Dados.BLO.BLO_PlayerDefesas().Insert(new Dados.Models.PlayerDefesas()
                {
                    IdPlayer = List[j].wizard_id,
                    NomeGuilda = List[j].opp_guild_name,
                    NomeOponente = List[j].opp_wizard_name,
                    Vitoria = Convert.ToInt32(List[j].win_count),
                    DataHora = dtDateTime
                });
            }
            PainelLoad(false, "-", "-", false);
        }

        private void btnEfetivarCargaDefesa_Click(object sender, EventArgs e)
        {
            try
            {
                CarregarTimeDefesas();
                lblMsgDefesa.Text = "Defesas carregadas com sucesso.";
            }
            catch (Exception ex)
            {

                lblMsgDefesa.Text = "Erro ao carregar time das defesas+";
                lblMsgDefesa.Text += Environment.NewLine + "Erro: " + ex.Message;

            }

            Thread YhdIniciar = new Thread(() => Defesas());
            YhdIniciar.Start();
        }



        private void btnEnviarLog_Click(object sender, EventArgs e)
        {
            if (txtSenhaEnviaServer.Text == "155468")
            {
                Thread YhdIniciar = new Thread(() => EviarArq());
                YhdIniciar.Start();
            }

        }

        #region Leitura do Arquivo Full Log

        private InfoPlayer.Root LerGuildWarContributeList(string[] lines)
        {
            try
            {

                PainelLoad(true, "Lendo Arquivos 1/5", "GetGuildWarContributeList", false);

                string Texto = "";

                foreach (string line in lines)
                {

                    //pedro
                    if (line.Contains("GetGuildWarContributeList") && line.Contains(@"ret_code"":0"))
                    {
                        Texto += line;
                        break;
                    }
                }
                JavaScriptSerializer Player = new JavaScriptSerializer();
                InfoPlayer.Root ObjPlayer = Player.Deserialize<InfoPlayer.Root>(Texto);

                return ObjPlayer;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o GetGuildWarContributeList.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }

        }

        private InfoBatalha LerGuildWarBattleLogByGuildId(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Arquivos 2/5", "GetGuildWarBattleLogByGuildId", false);
                string Texto = "";
                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildWarBattleLogByGuildId") && line.Contains(@"ret_code"":0"))
                    {

                        Texto += line;
                        break;

                    }
                }
                JavaScriptSerializer Batalhas = new JavaScriptSerializer();
                InfoBatalha ObjBatalha = Batalhas.Deserialize<InfoBatalha>(Texto);

                return ObjBatalha;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o LerGuildWarBattleLogByGuildId.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;

            }


        }
        private InfoOponente.Root LerGuildWarMatchupInfo(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Arquivos 3/5", "GetGuildWarMatchupInfo", false);
                string Texto = "";
                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildWarMatchupInfo") && line.Contains(@"ret_code"":0"))
                    {

                        Texto += line;
                        break;

                    }
                }
                JavaScriptSerializer Oponente = new JavaScriptSerializer();
                InfoOponente.Root ObjOponente = Oponente.Deserialize<InfoOponente.Root>(Texto);

                return ObjOponente;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o GetGuildWarMatchupInfo.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);

                throw ex;
            }


        }
        private InfoOtherInfoPlayer.Root LerGuildInfo(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Arquivos 4/5", "GetGuildInfo", false);

                string Texto = "";
                foreach (string line in lines)
                {
                    if (line.Contains("GetGuildInfo") && line.Contains(@"ret_code"":0"))
                    {

                        Texto += line;
                        break;

                    }
                }
                JavaScriptSerializer PlayOtherInfo = new JavaScriptSerializer();
                InfoOtherInfoPlayer.Root ObjPlayOtherInfo = PlayOtherInfo.Deserialize<InfoOtherInfoPlayer.Root>(Texto);

                return ObjPlayOtherInfo;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o GetGuildInfo.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }

        }

        private InfoParticipantes.Root LerGuildWarParticipationInfo(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Arquivos 5/5", "GetGuildWarParticipationInfo", false);

                string Texto = "";
                foreach (string line in lines)
                {
                    //pedro
                    if (line.Contains("GetGuildWarParticipationInfo") && line.Contains(@"ret_code"":0"))
                    {
                        Texto += "{\"" + line.Substring(line.IndexOf("ret_code"), line.Length - line.IndexOf("ret_code"));
                        break;
                    }
                }
                JavaScriptSerializer Paticipante = new JavaScriptSerializer();
                InfoParticipantes.Root ObjParticipante = Paticipante.Deserialize<InfoParticipantes.Root>(Texto);

                return ObjParticipante;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o GetGuildWarParticipationInfo.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }


        }

        private InfoLogBatalhas.Root LerGuildWarMatchLog(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Match Logs", "GetGuildWarMatchLog", false);

                string Texto = "";
                foreach (string line in lines)
                {
                    if (line.Contains("GetGuildWarMatchLog") && line.Contains(@"ret_code"":0"))
                    {
                        Texto += "{\"" + line.Substring(line.IndexOf("ret_code"), line.Length - line.IndexOf("ret_code"));
                        break;
                    }
                }
                JavaScriptSerializer MatchLog = new JavaScriptSerializer();
                InfoLogBatalhas.Root objMatch = null;

                try
                {
                    objMatch = MatchLog.Deserialize<InfoLogBatalhas.Root>(Texto);
                }
                catch (Exception)
                {

                }

                return objMatch;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o GetGuildWarMatchLog.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }


        }

        #endregion

        #region Leitura do Arquivo Full Log - SIEGE
        private InfoSiege.Root LerGuildSiegeMatchupInfo(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Siege Matchup Info", "GetGuildSiegeMatchupInfo", false);
                string Texto = "";

                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildSiegeMatchupInfo") && line.Contains(@"ret_code"":0"))
                    {
                        Texto += line;
                        break;
                    }
                }
                JavaScriptSerializer Siege = new JavaScriptSerializer();
                InfoSiege.Root objSiege = Siege.Deserialize<InfoSiege.Root>(Texto);

                return objSiege;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o GetGuildSiegeMatchupInfo.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }

        }
        private List<InfoSiegeDefense.Root> LerGuildSiegeDefense(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Siege Defense Info", "GetGuildSiegeDefenseDeckByWizardId", false);
                List<InfoSiegeDefense.Root> lstRetorno = new List<InfoSiegeDefense.Root>();

                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildSiegeDefenseDeckByWizardId") && line.Contains(@"ret_code"":0"))
                    {

                        JavaScriptSerializer Defense = new JavaScriptSerializer();
                        InfoSiegeDefense.Root objDefense = Defense.Deserialize<InfoSiegeDefense.Root>(line);

                        lstRetorno.Add(objDefense);
                    }
                }

                return lstRetorno;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o LerGuildSiegeDefense.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }

        }

        private List<InfoSiegeBattleLog.Root> LerGuildSiegeBattleLog(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Siege Defense Battle Log", "GetGuildSiegeBattleLog", false);
                List<InfoSiegeBattleLog.Root> lstRetorno = new List<InfoSiegeBattleLog.Root>();

                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildSiegeBattleLog") && line.Contains(@"ret_code"":0"))
                    {

                        JavaScriptSerializer BattleLog = new JavaScriptSerializer();
                        InfoSiegeBattleLog.Root objBattleLog = BattleLog.Deserialize<InfoSiegeBattleLog.Root>(line);

                        lstRetorno.Add(objBattleLog);
                    }
                }

                return lstRetorno;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o LerGuildSiegeBattleLog.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }

        }

        private InfoSiegeMatchLog.Root LerGuildSiegeMatchLog(string[] lines)
        {
            try
            {
                PainelLoad(true, "Lendo Siege Defense Match Log", "GetGuildSiegeMatchLog", false);
                List<InfoSiegeMatchLog.Root> lstRetorno = new List<InfoSiegeMatchLog.Root>();
                string Texto = string.Empty;

                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildSiegeMatchLog") && line.Contains(@"ret_code"":0"))
                    {
                        Texto = line;

                        break;
                    }
                }
                JavaScriptSerializer MatchLog = new JavaScriptSerializer();
                InfoSiegeMatchLog.Root objMatchLog = MatchLog.Deserialize<InfoSiegeMatchLog.Root>(Texto);

                return objMatchLog;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Ler o LerGuildSiegeMatchLog.";
                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }

        }



        #endregion


        #region Cadastrar Dados do FullLog no Banco

        private void CadastrarGuilda(InfoBatalha ObjBatalha)
        {
            try
            {
                Dados.Models.Guilda GuildaTemp = new Dados.BLO.BLO_Guilda().Insert(new Dados.Models.Guilda()
                {
                    Id = ObjBatalha.battle_log_list_group[0].battle_log_list[0].guild_id,
                    Nome = ObjBatalha.battle_log_list_group[0].battle_log_list[0].guild_name

                });

            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Cadastrar Guilda.\r\n";

                log += "OBJGuilda\r\n";
                log += "Guild Id: " + ObjBatalha.battle_log_list_group[0].battle_log_list[0].guild_id + "\r\n";
                log += "Guild name: " + ObjBatalha.battle_log_list_group[0].battle_log_list[0].guild_name + "\r\n";
                log += "\nErro:" + ex.Message;

                GravarLog(log);

            }
        }

        private void CadastrarGuilda(long id, string nome)
        {
            try
            {
                Dados.Models.Guilda GuildaTemp = new Dados.BLO.BLO_Guilda().Insert(new Dados.Models.Guilda()
                {
                    Id = id,
                    Nome = nome

                });

            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Cadastrar Guilda.\r\n";

                log += "OBJGuilda\r\n";
                log += "Guild Id: " + id + "\r\n";
                log += "Guild name: " + nome + "\r\n";
                log += "\nErro:" + ex.Message;

                GravarLog(log);

            }
        }



        private long CadastrarBatalha(InfoBatalha ObjBatalha, InfoOponente.Root ObjOponente, InfoParticipantes.Root ObjParticipante)
        {

            try
            {

                long VidaClanAux = ObjOponente.guildwar_match_info.opp_guild_hp_max;
                long AuxLife = ObjOponente.opp_defense_list.Count * 10000;
                VidaClanAux = VidaClanAux - (VidaClanAux - AuxLife);

                //20170612
                // Unix timestamp is seconds past epoch

                System.DateTime dBatalha = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dBatalha = dBatalha.AddSeconds(ObjBatalha.battle_log_list_group[0].battle_log_list[0].battle_end).ToLocalTime();

                Dados.Models.Batalhas BatalhaTemp = new Dados.BLO.BLO_Batalha().Insert(new Dados.Models.Batalhas()
                {
                    //20170612
                    Data = Convert.ToDateTime(dBatalha.ToShortDateString()),
                    idGuilda = ObjBatalha.battle_log_list_group[0].opp_guild_info.guild_id,
                    Guilda = ObjBatalha.battle_log_list_group[0].opp_guild_info.name,
                    Life = VidaClanAux,
                    PontuacaoOponente = ObjOponente.opp_participation_info.match_score,
                    PontuacaoGuild = ObjParticipante.guildwar_ranking_info.match_score,
                    RankGuild = ObjParticipante.guildwar_ranking_info.rank,
                    idGuildaAtacante = ObjBatalha.battle_log_list_group[0].battle_log_list[0].guild_id

                });

                return BatalhaTemp.ID;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar CadastrarBatalha.\r\n";

                log += "OBJBATALHA\r\n";
                foreach (BattleLogListGroup p in ObjBatalha.battle_log_list_group)
                {
                    log += "Guild Id: " + p.opp_guild_info.guild_id + "\r\n";
                    log += "Guild name: " + p.opp_guild_info.name + "\r\n";

                    foreach (BattleLogList item2 in p.battle_log_list)
                    {
                        foreach (System.Reflection.PropertyInfo z in item2.GetType().GetProperties())
                        {
                            if (z.CanRead)
                            {
                                log += z.Name + ": " + z.GetValue(item2, null).ToString() + Environment.NewLine;
                            }
                        }

                    }
                }



                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;
            }

        }

        private void CadastrarPlayer(InfoPlayer.Root ObjPlayer, long idBatalha, InfoBatalha ObjBatalha)
        {
            try
            {
                //Listar os usuarios da Guilda para nao ficar toda hora fazendo Select. 
                List<GuildaPlayer> lstGuildaPlayers = new List<GuildaPlayer>();
                Dados.BLO.BLO_Guilda blGuilda = new Dados.BLO.BLO_Guilda();
                lstGuildaPlayers = blGuilda.ListarGuildaPlayer(ObjBatalha.battle_log_list_group[0].battle_log_list[0].guild_id);


                for (int j = 0; j < ObjPlayer.guildwar_contribute_list.Count; j++)
                {
                    PainelLoad(true, "Cadastrando os Players", (j + 1).ToString() + "/" + ObjPlayer.guildwar_contribute_list.Count.ToString(), false);

                    //Insert Player
                    try
                    {
                        new Dados.BLO.BLO_Player().Insert(new Dados.Models.Player()
                        {
                            ID = ObjPlayer.guildwar_contribute_list[j].wizard_id,
                            Nome = ObjPlayer.guildwar_contribute_list[j].wizard_name,
                            Level = ObjPlayer.guildwar_contribute_list[j].wizard_level,
                            PontoArena = ObjPlayer.guildwar_contribute_list[j].guild_pts,
                            Status = "N"
                        });

                    }
                    //se Der erro, continua para o próximo.
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao incluir Players");
                        string log;
                        log = "Erro ao tentar incluir Player.\r\n ";
                        log += ex.Message + " \r\n ";
                        log += "Nome: " + ObjPlayer.guildwar_contribute_list[j].wizard_name + "\r\n";
                        log += "Level: " + ObjPlayer.guildwar_contribute_list[j].wizard_level + "\r\n";
                        log += "PontoArena: " + ObjPlayer.guildwar_contribute_list[j].guild_pts + "\r\n";
                        GravarLog(log);
                    }


                    //Insert Player Status
                    try
                    {
                        new Dados.BLO.BLO_PlayerStatus().Insert(new Dados.Models.PlayerStatus()
                        {
                            IdPlayer = ObjPlayer.guildwar_contribute_list[j].wizard_id,
                            //20170612
                            IdBatalha = idBatalha,
                            Status = "N"
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao incluir Player Status");
                        string log;
                        log = "Erro ao tentar incluir Player Status \r\n ";
                        log += ex.Message + " \r\n ";
                        log += "ID: " + ObjPlayer.guildwar_contribute_list[j].wizard_id + "\r\n";
                        log += "ID Batalha: " + idBatalha + "\r\n";
                        GravarLog(log);
                    }

                    //Inser Player Guilda
                    try
                    {

                        //Se não existir insere
                        if (!lstGuildaPlayers.Any(x => x.IdPlayer == ObjPlayer.guildwar_contribute_list[j].wizard_id))
                        {
                            blGuilda.InsertGuildaPlayer(ObjBatalha.battle_log_list_group[0].battle_log_list[0].guild_id, ObjPlayer.guildwar_contribute_list[j].wizard_id);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Erro ao incluir Player Guilda");
                        string log;
                        log = "Erro ao tentar incluir Player Guilda \r\n ";
                        log += ex.Message + " \r\n ";
                        log += "ID: " + ObjPlayer.guildwar_contribute_list[j].wizard_id + "\r\n";
                        log += "ID Batalha: " + idBatalha + "\r\n";
                        GravarLog(log);
                    }

                }
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar CadastrarPlayer.\r\n";
                log += "\nErro:" + ex.Message;
                GravarLog(log);

                throw ex;
            }

        }

        private void CadastrarPlayerStatus(InfoOponente.Root ObjOponente, InfoPlayer.Root ObjPlayer, long idBatalha)
        {
            try
            {
                for (int j = 0; j < ObjOponente.my_attack_list.Count; j++)
                {
                    PainelLoad(true, "Verificando Status dos Players", (j + 1).ToString() + "/" + ObjOponente.my_attack_list.Count.ToString(), false);

                    //Insert Player
                    try
                    {
                        new Dados.BLO.BLO_Player().Insert(new Dados.Models.Player()
                        {
                            ID = ObjOponente.my_attack_list[j].wizard_id,
                            Nome = ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjOponente.my_attack_list[j].wizard_id).FirstOrDefault().wizard_name,
                            PontoArena = ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjOponente.my_attack_list[j].wizard_id).FirstOrDefault().guild_pts,
                            Status = "S"
                        });
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Erro ao incluir Players 2");
                        string log;
                        log = "Erro ao tentar incluir Player.\r\n ";
                        log += "ID: " + ObjOponente.my_attack_list[j].wizard_id + "\r\n";
                        log += "Nome: " + ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjOponente.my_attack_list[j].wizard_id).FirstOrDefault().wizard_name + "\r\n";
                        log += "PontoArena: " + ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjOponente.my_attack_list[j].wizard_id).FirstOrDefault().guild_pts + "\r\n";
                        GravarLog(log);
                    }


                    //Insert Player Status
                    try
                    {
                        new Dados.BLO.BLO_PlayerStatus().Insert(new Dados.Models.PlayerStatus()
                        {
                            IdPlayer = ObjOponente.my_attack_list[j].wizard_id,
                            //20170612
                            IdBatalha = idBatalha,
                            Status = "S"
                        });
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Erro ao incluir Player Status 2");
                        string log;
                        log = "Erro ao tentar incluir Player Status \r\n ";
                        log += "ID: " + ObjOponente.my_attack_list[j].wizard_id + "\r\n";
                        log += "ID Batalha: " + idBatalha + "\r\n";
                        GravarLog(log);
                    }

                }
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar CadastrarPlayerStatus.\r\n";
                log += "\nErro:" + ex.Message;
                GravarLog(log);

                throw ex;
            }

        }

        private void CadastrarOponentes(InfoOponente.Root ObjOponente, long idBatalha)
        {
            string log = "";
            try
            {

                for (int j = 0; j < ObjOponente.opp_defense_list.Count; j++)
                {
                    PainelLoad(true, "Cadastrando os oponentes", (j + 1).ToString() + "/" + ObjOponente.opp_defense_list.Count.ToString(), false);
                    log = "";
                    //Insert Oponente

                    log = "ID:" + ObjOponente.opp_defense_list[j].wizard_id + "\r\n";
                    log += "Nome:" + ObjOponente.opp_guild_member_list.Single(a => a.wizard_id == ObjOponente.opp_defense_list[j].wizard_id).wizard_name + "\r\n";
                    log += "Bonus:" + ObjOponente.opp_defense_list[j].guild_point_bonus + "\r\n";
                    log += "CodGuilda:" + idBatalha + "\r\n";

                    new Dados.BLO.BLO_PlayerOponente().Insert(new Dados.Models.PlayerOponente()
                    {
                        ID = ObjOponente.opp_defense_list[j].wizard_id,
                        Nome = ObjOponente.opp_guild_member_list.Single(a => a.wizard_id == ObjOponente.opp_defense_list[j].wizard_id).wizard_name,
                        Bonus = ObjOponente.opp_defense_list[j].guild_point_bonus,
                        IdBatalha = idBatalha

                    });

                }
            }
            catch (Exception ex)
            {
                string log2 = "Erro ao tentar CadastrarOponentes.\r\n";
                log2 += "\nErro:" + ex.Message + "\r\n";
                GravarLog(log2 + log);
                throw ex;
            }

        }

        private long CadastrarLuta(InfoBatalha ObjBatalha, int count, int indexLoopBatalhas, int indexLoopLuta, long vidaClan, Dados.Models.Batalhas Batalha, InfoOponente.Root ObjOponente)
        {
            try
            {
                PainelLoad(true, "Cadastrando as batalhas", count.ToString() + "/" + ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list.Count.ToString(), false);
                int temp = Dados.BLO.BLO_Lutas._SelectAllByBatalha(Batalha).Where(w => w.CodPlayerOponente == ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].opp_wizard_id).Sum(w => w.Vitoria).Value;

                //Percentual e VidaClan
                List<long> retorno = new List<long>();
                retorno = CalcularVidaGVG(temp, ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].win_count, vidaClan);

                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].battle_end).ToLocalTime();

                //Inser Luta
                try
                {
                    new Dados.BLO.BLO_Lutas().Insert(new Dados.Models.Lutas()
                    {
                        CodBatalhas = Batalha.ID,
                        CodPlayer = ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].wizard_id,
                        CodPlayerOponente = ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].opp_wizard_id,
                        //Se Draw = 1 e Win = 1 Então WIN senao pega o que vier mesmo.
                        Vitoria = ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].win_count == 1 && ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].draw_count == 1 ? 2 : ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].win_count,
                        ValorBarra = retorno[0],
                        DataHora = dtDateTime,
                        MomentoVitoria = retorno[1] < ObjOponente.guildwar_match_info.opp_guild_hp_win_cond ? "Win" : "In War"
                    });

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao cadastrar luta");
                    string log = "";
                    log += "Mensagem de erro: " + ex.Message.ToString();
                    log += "CodBatalhas: " + Batalha.ID + "\r\n";
                    log += "CodPlayer: " + ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].wizard_id + "\r\n";
                    log += "CodPlayerOponente: " + ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].opp_wizard_id + "\r\n";
                    log += "Vitoria: " + ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list[indexLoopLuta].win_count + "\r\n";
                    log += "ValorBarra: " + retorno[0] + "\r\n";
                    log += "DataHora: " + dtDateTime + "\r\n";
                    log += "MomentoVitoria: " + (retorno[1] < ObjOponente.guildwar_match_info.opp_guild_hp_win_cond ? "Win" : "In War") + "\r\n";
                    GravarLog(log);

                }

                //Retorno do vidaClan
                return retorno[1];

            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar CadastrarLuta.\r\n";
                log += "\nErro:" + ex.Message;
                GravarLog(log);
                throw ex;
                throw ex;
            }


        }

        private List<long> CalcularVidaGVG(int temp, int win, long vidaClan)
        {
            try
            {
                long porct = 100;
                if (temp == 1)
                    porct -= 30;
                if (temp == 2)
                    porct -= 51;
                if (temp == 3)
                    porct -= 66;
                if (temp == 4)
                    porct -= 76;
                if (temp == 5)
                    porct -= 86;
                if (temp == 6)
                    porct -= 96;
                if (temp == 7)
                    porct -= 100;
                if (temp > 7)
                    porct -= 0;

                if (porct == 100)
                {
                    if (win == 2)
                    {
                        vidaClan -= 5100;
                    }
                    else if (win == 1)
                    {
                        vidaClan -= 3000;
                    }
                }
                if (porct == 70)
                {
                    if (win == 2)
                    {
                        vidaClan -= 3600;
                    }
                    else if (win == 1)
                    {
                        vidaClan -= 2100;
                    }
                }
                if (porct == 49)
                {
                    if (win == 2)
                    {
                        vidaClan -= 2500;
                    }
                    else if (win == 1)
                    {
                        vidaClan -= 1500;
                    }
                }
                if (porct == 34)
                {
                    if (win == 2)
                    {
                        vidaClan -= 2000;
                    }
                    else if (win == 1)
                    {
                        vidaClan -= 1000;
                    }
                }
                if (porct == 24)
                {
                    if (win == 2)
                    {
                        vidaClan -= 2000;
                    }
                    else if (win == 1)
                    {
                        vidaClan -= 1000;
                    }
                }
                if (porct == 14)
                {
                    if (win == 2)
                    {
                        vidaClan -= 1400;
                    }
                    else if (win == 1)
                    {
                        vidaClan -= 1000;
                    }
                }
                if (porct == 4)
                {
                    if (win == 2)
                    {
                        vidaClan -= 400;
                    }
                    else if (win == 1)
                    {
                        vidaClan -= 400;
                    }
                }

                //TODO: tem que fazer retornar o vida clan e o perc.
                List<long> retorno = new List<long>();
                retorno.Add(porct);
                retorno.Add(vidaClan);
                return retorno;
            }
            catch (Exception ex)
            {
                string log;
                log = "Erro a calcular Percentual e Vida do Clan \r\n";
                log += "Erro: " + ex.Message;
                GravarLog(log);
                throw;
            }

        }
        #endregion


        private void FazerBackupArquivo()
        {
            if (!Directory.Exists(txtDiretorio.Text + "//BeckUp"))
            {
                Directory.CreateDirectory(txtDiretorio.Text + "//BeckUp");
            }
            System.IO.File.Copy(txtDiretorio.Text + @"//tempDemonOrange.txt", txtDiretorio.Text + @"//BeckUp//" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".txt");

        }

        private void GravarLog(string mensagem)
        {
            try
            {
                //Grava 1 log da ultima execução
                string nomeArquivo = @"c:\temp\logSWProxy" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt";

                StreamWriter writer = new StreamWriter(nomeArquivo, true);
                writer.WriteLine("--------- " + DateTime.Now.ToString() + "---------------");
                writer.WriteLine("");
                writer.WriteLine(mensagem);
                writer.Flush();
                writer.Close();

            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                //nao pode parar se der pau na gravação do LOG
            }


        }

        private void CarregarTimeDefesas()
        {
            string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//full_log.txt");
            string Texto = "";
            foreach (string line in lines)
            {

                if (line.Contains("GetGuildWarDefenseUnits") && line.Contains(@"ret_code"":0"))
                {
                    Texto += "{\"" + line.Substring(line.IndexOf("ret_code"), line.Length - line.IndexOf("ret_code"));

                    JavaScriptSerializer Defesas = new JavaScriptSerializer();
                    InfoDefesas.Root objDefesa = Defesas.Deserialize<InfoDefesas.Root>(Texto);

                    Dados.DAO.DAO_TimeDefesa daoTimeDefesa = new Dados.DAO.DAO_TimeDefesa();
                    try
                    {
                        daoTimeDefesa.AtualizarTimeDefesa(objDefesa);
                    }
                    catch (Exception)
                    {

                        lblMsgDefesa.Text += Environment.NewLine + "Erro ao tentar incluir Time Defesa";
                        lblMsgDefesa.Text += Environment.NewLine + "IdPlayer: " + objDefesa.defense_wizard_id;
                    }

                    Texto = string.Empty;
                }
            }
        }



        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void btnAtualizarSiege_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txtDiretorio.Text + @"//tempDemonOrange.txt"))
                System.IO.File.Delete(txtDiretorio.Text + @"//tempDemonOrange.txt");
            System.IO.File.Move(txtDiretorio.Text + @"//full_log.txt", txtDiretorio.Text + @"//tempDemonOrange.txt");
            Thread YhdIniciarSiege = new Thread(() => AtualizarSiege());
            YhdIniciarSiege.Start();
        }

        private void AtualizarSiege()
        {

            PainelLoad(true, "Aguarde", "Iniciando Processo!", false);

            FazerBackupArquivo();

            string[] lines = System.IO.File.ReadAllLines(txtDiretorio.Text + @"//tempDemonOrange.txt");


            // ---- Leitura do Arquivo FullLog ----// 
            InfoSiege.Root rootSiege = LerGuildSiegeMatchupInfo(lines);
            List<InfoSiegeDefense.Root> rootDefesas = LerGuildSiegeDefense(lines);
            List<InfoSiegeBattleLog.Root> rootBatalhas = LerGuildSiegeBattleLog(lines);
            InfoSiegeMatchLog.Root rootMatch = LerGuildSiegeMatchLog(lines);

            PainelLoadSiege(true, "Leitura de arquivo concluido", "-", false);

            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(rootSiege.match_info.match_start_time).ToLocalTime();

            //Guarda o Código da Guild e da Siege
            long codGuild = rootMatch.guildsiege_match_log_list[0].guild_id;
            long codSiege = rootSiege.match_info.siege_id;


            //-----> Siege
            Siege objSiege = new Siege() { Id = rootSiege.match_info.siege_id, Data = dtDateTime };
            objSiege = new Dados.BLO.BLO_Siege().InserirSiege(objSiege);

            //----->Guilda e Siege x Guilda
            SiegeGuilda objSiegexGuilda;
            Dados.BLO.BLO_Siege bSiege = new Dados.BLO.BLO_Siege();

            foreach (InfoSiege.GuildList item in rootSiege.guild_list)
            {
                CadastrarGuilda(item.guild_id, item.guild_name);

                //Siege x Guilda
                objSiegexGuilda = new SiegeGuilda()
                {
                    IdSiege = item.siege_id,
                    IdGuilda = item.guild_id,
                    Posicao = item.pos_id
                };
                bSiege.InserirSiegeGuilda(objSiegexGuilda);

            }


            //SiegePlayers
            foreach (InfoSiege.WizardInfoList item in rootSiege.wizard_info_list)
            {

                //Incluir só se for da Guild
                if (item.guild_id == codGuild)
                {
                    bSiege.InserirSiegePlayer
                        (
                            new SiegePlayer()
                            {
                                IdPlayer = item.wizard_id,
                                IdSiege = codSiege,
                                UsedUnits = rootSiege.used_unit_count_list.First(x => x.wizard_id == item.wizard_id).used_unit_count
                            }
                    );
                }
                else
                {
                    bSiege.InserirSiegePlayerOponente
                       (
                           new SiegePlayerOponente()
                           {
                               IdPlayer = item.wizard_id,
                               IdGuild = item.guild_id,
                               IdSiege = codSiege,
                               Nome = item.wizard_name

                           }
                         );
                }

            }

            //SiegeDefenseDeck
            foreach (InfoSiege.DefenseDeckList item in rootSiege.defense_deck_list)
            {
                bSiege.InsertDefenseDeck
                    (
                    new SiegeDefenseDeck()
                        {
                            IdDeck = item.deck_id,
                            IdSiege = codSiege,
                            IdPlayer = item.wizard_id,
                            IdGuild = rootSiege.wizard_info_list.First(x => x.wizard_id == item.wizard_id).guild_id
                        }
                    );
            }


            //SiegeAtaques
            System.DateTime dDataluta = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            
            foreach (InfoSiegeBattleLog.BattleLogList item in rootBatalhas[0].log_list[0].battle_log_list)
            {
                //IF Ataque
                if (item.log_type==1)
                {
                    bSiege.InsertSiegeAtaque
                    (new SiegeAtaque()
                    {
                        Data = dtDateTime = dtDateTime.AddSeconds(item.log_timestamp).ToLocalTime(),
                        IdPlayer = item.wizard_id,
                        IdPlayerOponente = item.opp_wizard_id,
                        IdSiege = item.siege_id,
                        Vitoria = item.win_lose

                    }
                    );
                }
                else
                {
                    bSiege.InserirSiegePlayerDefense(new SiegePlayerDefesa()
                    {
                        Date = dtDateTime = dtDateTime.AddSeconds(item.log_timestamp).ToLocalTime(),
                        IdDeck = item.
                    }

                   )
;                }
                
            }

            //SiegePlayerDefesas

            //SiegeTimeDefesas


        }
    }

    public class PlayerDef
    {
        public string Nome { get; set; }
    }
}
