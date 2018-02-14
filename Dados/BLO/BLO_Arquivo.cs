using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using Dados.Models;
using Models;
using System.Web.Script.Serialization;
using Dados.DAO;

namespace Dados.BLO
{
    public class BLO_Arquivo
    {


        public void CarregarGVG(FileInfo arquivo)
        {
            CarregaArquivoGVG(arquivo.FullName);
            
        }

        public void CarregarGVG()
        {
            string txtDiretorio = @"C:\Users\pbveber\Source\Repos\DB_SW\GuildSw";
            CarregaArquivoGVG(txtDiretorio + @"//tempDemonOrange.txt");
        }

        public void CarregarSiege(FileInfo arquivo)
        {
            CarregarArquivoSiege(arquivo.FullName);
            
        }

        public void CarregarSiege()
        {
            string txtDiretorio = @"C:\Users\pbveber\Source\Repos\DB_SW\GuildSw";
            CarregarArquivoSiege(txtDiretorio + @"//tempDemonOrange.txt");
        }




        private void CarregaArquivoGVG(string caminhoArquivo)
        {
            //FazerBackupArquivo();

            string[] lines = System.IO.File.ReadAllLines(caminhoArquivo);

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

            long CodigoBatalha = 0;

            //Para cada GVG
            for (int i = 0; i < ObjBatalha.battle_log_list_group.Count; i++)
            {

                //Cadastrar a Guilda
                CadastrarGuilda(ObjBatalha);

                //PainelLoad(true, "Cadastrando a Batalha", ObjBatalha.battle_log_list_group[i].opp_guild_info.name, false);


                //Se 0 = GVG atual. Incluir os dados dos players e oponentes. 
                if (i == 0)
                {

                    //Batalha
                    CodigoBatalha = CadastrarBatalha(ObjBatalha, ObjOponente, ObjParticipante);

                    //Player
                    CadastrarPlayer(ObjPlayer, CodigoBatalha, ObjBatalha);

                    //PlayerStatus
                    CadastrarPlayerStatus(ObjParticipante, ObjOponente, ObjPlayer, CodigoBatalha);

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


            //PainelLoad(true, "Finalizando", "Aguarde...", false);
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
                    CarregarDefesasGVG(CodigoBatalha, ObjBatalha.battle_log_list_group[0].opp_guild_info.guild_id, caminhoArquivo);
                }

            }
            catch (Exception)
            {

            }
        }


        public void CarregarArquivoSiege(string caminhoArquivo)
        {
            //FazerBackupArquivo();

            string[] lines = System.IO.File.ReadAllLines(caminhoArquivo);


            // ---- Leitura do Arquivo FullLog ----// 
            InfoSiege.Root rootSiege = LerGuildSiegeMatchupInfo(lines);
            List<InfoSiegeDefense.Root> rootDefesas = LerGuildSiegeDefense(lines);
            List<InfoSiegeBattleLog.Root> rootBatalhas = LerGuildSiegeBattleLog(lines);
            InfoSiegeMatchLog.Root rootMatch = LerGuildSiegeMatchLog(lines);

            //PainelLoadSiege(true, "Leitura de arquivo concluido", "-", false);

            //Guarda o Código da Guild e da Siege
            long codGuild = rootMatch.guildsiege_match_log_list[0].guild_id;
            long idSiege = 0;


            //----> Siege
            //PainelLoadSiege(true, "Gravando Siege", "-", false);
            idSiege = InserirSiege(rootSiege, rootMatch);

            //----> Defense Deck Assign
            //PainelLoadSiege(true, "Gravando Decks", "-", false);
            GravarDefenseDeckAssign(rootSiege, idSiege);

            //----> Siege x Guilda
            //PainelLoadSiege(true, "Gravando Guildas", "-", false);
            GravarSiegeGuilda(rootSiege, rootMatch, idSiege);

            //----> Siege Players
            //PainelLoadSiege(true, "Gravando Players", "-", false);
            GravarSiegePlayers(rootSiege, idSiege, codGuild);

            //----> Defense Decks
            //PainelLoadSiege(true, "Gravando Deck Defesas", "-", false);
            GravarDefenseDecks(rootSiege, idSiege, codGuild);

            //----> Ataques/Defesas
            //PainelLoadSiege(true, "Gravando Ataques", "-", false);
            GravarAtaquesDefesas(rootBatalhas, rootSiege, idSiege);

            //----> Times Defesas
            //PainelLoadSiege(true, "Gravando Times Defesa", "-", false);
            GravarTimeDefesa(rootDefesas, idSiege);
        }

        public void CarregarDefesas(FileInfo arquivo)
        {
            try
            {
                List<InfoBatalhaPlayer.BattleLogList> list = new List<InfoBatalhaPlayer.BattleLogList>();
                string[] lines = System.IO.File.ReadAllLines(arquivo.FullName);

                List<PlayerDef> ListPlayer = new List<PlayerDef>();

                list = new List<InfoBatalhaPlayer.BattleLogList>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("GetGuildWarBattleLogByWizardId") && lines[i].Contains(@"ret_code"":0"))


                    {
                        JavaScriptSerializer Player = new JavaScriptSerializer();
                        InfoBatalhaPlayer.Root ObjPlayer = Player.Deserialize<InfoBatalhaPlayer.Root>(lines[i]);
                        for (int j = 0; j < ObjPlayer.battle_log_list.Count; j++)
                        {
                            if (list.Where(w => w.battle_end == ObjPlayer.battle_log_list[j].battle_end && w.wizard_id == ObjPlayer.battle_log_list[j].wizard_id).FirstOrDefault() == null)
                                list.Add(ObjPlayer.battle_log_list[j]);
                            if (ListPlayer.Where(w => w.Nome == ObjPlayer.battle_log_list[j].wizard_name).FirstOrDefault() == null)
                                ListPlayer.Add(new PlayerDef() { Nome = ObjPlayer.battle_log_list[j].wizard_name });
                        }

                    }
                }
                
                CarregarTimeDefesas(arquivo.FullName);
                GravarDefesas(list);

                

            }
            catch (Exception)
            {

                //TODO: Gravar LOG
            }

        }


        #region Leitura do Arquivo Full Log
        private InfoPlayer.Root LerGuildWarContributeList(string[] lines)
        {
            try
            {

                //PainelLoad(true, "Lendo Arquivos 1/5", "GetGuildWarContributeList", false);

                string Texto = "";

                foreach (string line in lines)
                {

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
                //PainelLoad(true, "Lendo Arquivos 2/5", "GetGuildWarBattleLogByGuildId", false);
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
                //PainelLoad(true, "Lendo Arquivos 3/5", "GetGuildWarMatchupInfo", false);
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
                //PainelLoad(true, "Lendo Arquivos 4/5", "GetGuildInfo", false);

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
                //PainelLoad(true, "Lendo Arquivos 5/5", "GetGuildWarParticipationInfo", false);

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
                //PainelLoad(true, "Lendo Match Logs", "GetGuildWarMatchLog", false);

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
                //PainelLoad(true, "Lendo Siege Matchup Info", "GetGuildSiegeMatchupInfo", false);
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
                //PainelLoad(true, "Lendo Siege Defense Info", "GetGuildSiegeDefenseDeckByWizardId", false);
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
                // PainelLoad(true, "Lendo Siege Defense Battle Log", "GetGuildSiegeBattleLog", false);
                List<InfoSiegeBattleLog.Root> lstRetorno = new List<InfoSiegeBattleLog.Root>();

                foreach (string line in lines)
                {

                    if (line.Contains("GetGuildSiegeBattleLog") && line.Contains(@"ret_code"":0") && !line.Contains("GetGuildSiegeBattleLogByWizardId"))
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
                //PainelLoad(true, "Lendo Siege Defense Match Log", "GetGuildSiegeMatchLog", false);
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
                    //PainelLoad(true, "Cadastrando os Players", (j + 1).ToString() + "/" + ObjPlayer.guildwar_contribute_list.Count.ToString(), false);

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
                        //MessageBox.Show("Erro ao incluir Players");
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
                        //MessageBox.Show("Erro ao incluir Player Status");
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

                        // MessageBox.Show("Erro ao incluir Player Guilda");
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

        private void CadastrarPlayerStatus(InfoParticipantes.Root objParticipante, InfoOponente.Root ObjOponente, InfoPlayer.Root ObjPlayer, long idBatalha)
        {
            try
            {
                string nome = string.Empty;
                long pontoArena = 0;

                for (int j = 0; j < ObjOponente.my_attack_list.Count; j++)
                {
                    //PainelLoad(true, "Verificando Status dos Players", (j + 1).ToString() + "/" + ObjOponente.my_attack_list.Count.ToString(), false);

                    //Quando não encntrar o player no contribute_list é porque ele pode ser novo na Guild.
                    if (!ObjPlayer.guildwar_contribute_list.Any(w => w.wizard_id == ObjOponente.my_attack_list[j].wizard_id))
                    {
                        if (objParticipante.guildwar_member_list.Any(y => y.wizard_id == ObjOponente.my_attack_list[j].wizard_id))
                        {
                            nome = objParticipante.guildwar_member_list.Where(y => y.wizard_id == ObjOponente.my_attack_list[j].wizard_id).FirstOrDefault().wizard_name;
                        }
                        else
                        {
                            nome = "NãoEncontrado";
                        }
                        
                        pontoArena = 0;
                    }
                    else
                    {
                        nome = ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjOponente.my_attack_list[j].wizard_id).FirstOrDefault().wizard_name;
                        pontoArena = ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjOponente.my_attack_list[j].wizard_id).FirstOrDefault().guild_pts;
                    }

                    //Insert Player
                    try
                    {
                        new Dados.BLO.BLO_Player().Insert(new Dados.Models.Player()
                        {
                            ID = ObjOponente.my_attack_list[j].wizard_id,
                            Nome = nome,
                            PontoArena = pontoArena,
                            Status = "S",
                            Level = 0
                        });
                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show("Erro ao incluir Player");
                        string log;
                        log = "Erro ao tentar incluir Player.\r\n ";
                        log += "ID: " + ObjOponente.my_attack_list[j].wizard_id + "\r\n";
                        log += "Exception:" + ex.ToString();

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

                        //MessageBox.Show("Erro ao incluir Player Status 2");
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
                    //PainelLoad(true, "Cadastrando os oponentes", (j + 1).ToString() + "/" + ObjOponente.opp_defense_list.Count.ToString(), false);
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
                //PainelLoad(true, "Cadastrando as batalhas", count.ToString() + "/" + ObjBatalha.battle_log_list_group[indexLoopBatalhas].battle_log_list.Count.ToString(), false);
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
                    //MessageBox.Show("Erro ao cadastrar luta");
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

        private void CarregarDefesasGVG(long idBatalha, long idGuilda,string caminhoArquivo)
        {
            string[] lines = System.IO.File.ReadAllLines(caminhoArquivo);
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
                //lblMsgDefesa.Text += Environment.NewLine + "Erro ao tentar incluir Time Defesa GVG";

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

        #region Cadastrar Dados do Fullog Siege

        #region Inserir Siege
        private long InserirSiege(InfoSiege.Root rootSiege, InfoSiegeMatchLog.Root rootMatch)
        {
            Siege objSiege = new Siege();
            bool historico = false;
            long retorno = 0;

            try
            {
                Dados.BLO.BLO_Siege bSiege = new Dados.BLO.BLO_Siege();

                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(rootSiege.match_info.match_start_time).ToLocalTime();

                //-----> Siege
                objSiege = new Siege() { IdSiege = rootSiege.match_info.siege_id, Data = dtDateTime, IdMatch = rootSiege.match_info.match_id };
                objSiege = new Dados.BLO.BLO_Siege().InserirSiege(objSiege);
                retorno = objSiege.Id;

                List<Dados.Models.Siege> lSieges = bSiege.ListarSieges();

                //-----> Siege (rootMatch -- Histórico)
                foreach (InfoSiegeMatchLog.GuildsiegeMatchLogList matchSieges in rootMatch.guildsiege_match_log_list)
                {
                    //Só insere se não existir no banco.
                    if (!lSieges.Any(x => x.IdSiege == matchSieges.siege_id && x.IdMatch == matchSieges.match_id))
                    {
                        historico = true;
                        //Neste objeto eu nao tenho a data da Siege, então estou pegando no "LastUpdate".
                        dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        dtDateTime = dtDateTime.AddSeconds(matchSieges.guild_info[0].match_score_last_update_time).ToLocalTime();

                        objSiege = new Siege() { IdSiege = matchSieges.siege_id, Data = dtDateTime, IdMatch = matchSieges.match_id };
                        objSiege = new Dados.BLO.BLO_Siege().InserirSiege(objSiege);
                    }

                }

                return retorno;
            }
            catch (Exception ex)
            {
                string log = "Erro ao tentar Inserir Siege.";

                log += "OBJGuilda\r\n";
                log += "Siege Id: " + objSiege.IdSiege.ToString() + "\r\n";
                log += "Match Id: " + objSiege.IdMatch.ToString() + "\r\n";
                log += "Data: " + objSiege.Data.ToString() + "\r\n";
                if (historico)
                {
                    log += "Histórico: Sim. \r\n";
                }

                log += "\nErro:" + ex.Message;

                GravarLog(log);
                throw ex;

            }

        }
        #endregion

        #region Deck Assign

        private void GravarDefenseDeckAssign(InfoSiege.Root rootSiege, long idSiege)
        {
            try
            {
                Dados.BLO.BLO_Siege bSiege = new Dados.BLO.BLO_Siege();
                //----> SiegeDefenseDeckAssign
                int cont = 1;
                foreach (var item in rootSiege.defense_deck_assign_list)
                {
                    //PainelLoadSiege(true, "Gravando Decks", cont + "/" + rootSiege.defense_deck_assign_list.Count.ToString(), false);

                    //bSiege.InserirSiegeGuilda
                    bSiege.InserirSiegeDefenseDeckAssign(new SiegeDefenseDeckAssign()
                    {
                        Base = item.base_number,
                        IdSiege = idSiege,
                        IdDeck = item.deck_id


                    });
                    cont++;
                }
            }
            catch (Exception ex)
            {

                string log = "Erro ao tentar Inserir DeckAssign.";
                log += "\nErro:" + ex.Message;
                GravarLog(log);
                throw ex;
            }

        }

        #endregion

        #region Siege x Guilda

        private void GravarSiegeGuilda(InfoSiege.Root rootSiege, InfoSiegeMatchLog.Root rootMatch, long idSiege)
        {
            SiegeGuilda objSiegexGuilda = new SiegeGuilda();
            try
            {
                Dados.BLO.BLO_Siege bSiege = new Dados.BLO.BLO_Siege();

                //Siege x Guilda
                foreach (InfoSiege.GuildList item in rootSiege.guild_list)
                {
                    CadastrarGuilda(item.guild_id, item.guild_name);

                    objSiegexGuilda = new SiegeGuilda()
                    {
                        IdSiege = idSiege,
                        IdGuilda = item.guild_id,
                        Posicao = item.pos_id,
                        MatchScore = item.match_score,
                        Members = item.play_member_count,
                        Rating = item.rating_id
                    };
                    bSiege.InserirSiegeGuilda(objSiegexGuilda);

                }

                int cont = 1;

                //Siege x Guilda (rootMatch -- Histórico)

                //pegar a lista de Siege já cadastradas.
                List<Dados.Models.Siege> lSieges = bSiege.ListarSieges();

                foreach (var item in rootMatch.guildsiege_match_log_list)
                {
                    cont = 1;
                    foreach (var item2 in item.guild_info)
                    {
                        //PainelLoadSiege(true, "Gravando Guildas", cont + "/" + item.guild_info.Count.ToString(), false);

                        CadastrarGuilda(item2.guild_id, item2.guild_name);

                        objSiegexGuilda = new SiegeGuilda()
                        {
                            IdSiege = lSieges.FirstOrDefault(y => y.IdSiege == item2.siege_id && y.IdMatch == item2.match_id).Id,
                            IdGuilda = item2.guild_id,
                            Posicao = item2.match_rank,
                            MatchScore = item2.match_score,
                            Members = item2.play_member_count,
                            Rating = item2.rating_id
                        };
                        bSiege.InserirSiegeGuilda(objSiegexGuilda);
                        cont++;
                    }
                }
            }
            catch (Exception ex)
            {

                string log = "Erro ao tentar Inserir Siege x Guilda.";

                log += "Id Guilda: " + objSiegexGuilda.IdGuilda.ToString() + "\r\n";
                log += "Siege Id: " + objSiegexGuilda.IdSiege.ToString() + "\r\n";
                log += "Match Score: " + objSiegexGuilda.MatchScore.ToString() + "\r\n";
                log += "Members: " + objSiegexGuilda.Members.ToString() + "\r\n";
                log += "Posicao: " + objSiegexGuilda.Posicao.ToString() + "\r\n";
                log += "Rating: " + objSiegexGuilda.Rating.ToString() + "\r\n";

                log += "\nErro:" + ex.Message;
                GravarLog(log);
                throw ex;
            }

        }
        #endregion

        #region Siege Players


        private void GravarSiegePlayers(InfoSiege.Root rootSiege, long idSiege, long codGuild)
        {

            try
            {
                Dados.BLO.BLO_Siege bSiege = new Dados.BLO.BLO_Siege();
                int cont = 1;

                //SiegePlayers
                foreach (InfoSiege.WizardInfoList item in rootSiege.wizard_info_list)
                {
                    //PainelLoadSiege(true, "Gravando Players", cont + "/" + rootSiege.wizard_info_list.Count.ToString(), false);

                    //Incluir só se for da Guild
                    if (item.guild_id == codGuild)
                    {
                        bSiege.InserirSiegePlayer
                            (
                                new SiegePlayer()
                                {
                                    IdPlayer = item.wizard_id,
                                    IdSiege = idSiege,
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
                                   IdSiege = idSiege,
                                   Nome = item.wizard_name

                               }
                             );
                    }
                    cont++;
                }
            }
            catch (Exception ex)
            {

                string log = "Erro ao tentar Inserir Players e Player Oponente.";
                log += "\nErro:" + ex.Message;
                GravarLog(log);
                throw ex;
            }

        }
        #endregion

        #region Defense Decks

        private void GravarDefenseDecks(InfoSiege.Root rootSiege, long idSiege, long codGuild)
        {
            try
            {
                Dados.BLO.BLO_Siege bSiege = new Dados.BLO.BLO_Siege();
                int cont = 1;

                //Só insere se for da Guilda
                foreach (InfoSiege.DefenseDeckList item in rootSiege.defense_deck_list)
                {
                    //PainelLoadSiege(true, "Gravando Deck Defesas", cont + "/" + rootSiege.defense_deck_list.Count.ToString(), false);

                    if (rootSiege.wizard_info_list.First(x => x.wizard_id == item.wizard_id).guild_id == codGuild)
                    {
                        bSiege.InsertDefenseDeck
                            (
                            new SiegeDefenseDeck()
                            {
                                IdDeck = item.deck_id,
                                IdSiege = idSiege,
                                IdPlayer = item.wizard_id,
                                IdGuild = rootSiege.wizard_info_list.First(x => x.wizard_id == item.wizard_id).guild_id
                            }
                            );
                    }

                    cont++;
                }
            }
            catch (Exception ex)
            {

                string log = "Erro ao tentar Inserir GravarDefenseDecks.";
                log += "\nErro:" + ex.Message;
                GravarLog(log);
                throw ex;
            }

        }
        #endregion

        #region Ataques\Defesas

        private void GravarAtaquesDefesas(List<InfoSiegeBattleLog.Root> rootBatalhas, InfoSiege.Root rootSiege, long idSiege)
        {
            try
            {
                Dados.BLO.BLO_Siege bSiege = new Dados.BLO.BLO_Siege();
                int cont = 1;

                //Obter os PlayersOponente
                List<SiegePlayerOponente> lstOponentes = bSiege.ListarPlayersOponentesSiege(idSiege);

                bool blnInsere = true;

                foreach (var battleLog in rootBatalhas)
                {
                    foreach (var logList in battleLog.log_list)
                    {
                        cont = 1;
                        foreach (InfoSiegeBattleLog.BattleLogList item in logList.battle_log_list)
                        {
                            //PainelLoadSiege(true, "Gravando Ataques e Defesas", cont + "/" + logList.battle_log_list.Count.ToString(), false);
                            blnInsere = true;

                            //Se nao encontrar o PlayerOPonente Inclui (Existem casos que não estão no MatchInfo, nao sei porque)
                            if (!lstOponentes.Any(x => x.IdGuild == item.opp_guild_id && x.IdPlayer == item.opp_wizard_id && x.Siege.IdSiege == item.siege_id))
                            {
                                // Só insere se for desta Siege!
                                if (lstOponentes.First().Siege.IdSiege == item.siege_id)
                                {
                                    lstOponentes.Add(bSiege.InserirSiegePlayerOponente(new SiegePlayerOponente()
                                    {
                                        IdGuild = item.opp_guild_id,
                                        IdPlayer = item.opp_wizard_id,
                                        IdSiege = idSiege,
                                        Nome = item.opp_wizard_name,
                                        Siege = new Siege() { Id = idSiege, IdSiege = item.siege_id }
                                    }));
                                    blnInsere = true;
                                }
                                else
                                {
                                    blnInsere = false;
                                }

                            }

                            if (blnInsere)
                            {
                                //IF Ataque
                                if (item.log_type == 1)
                                {
                                    bSiege.InsertSiegeAtaque
                                    (new SiegeAtaque()
                                    {
                                        Data = UnixTimeStampToDateTime(Convert.ToDouble(item.log_timestamp)),
                                        IdPlayer = item.wizard_id,
                                        IdPlayerOponente = lstOponentes.First(x => x.IdGuild == item.opp_guild_id && x.IdPlayer == item.opp_wizard_id && x.Siege.IdSiege == item.siege_id).Id,
                                        IdSiege = idSiege,
                                        Vitoria = item.win_lose,
                                        Base = item.base_number,
                                        IdGuildaOpp = item.opp_guild_id

                                    }
                                    );
                                }
                                //DEFESA
                                else
                                {

                                    List<int> listaDecks = new List<int>();
                                    listaDecks = rootSiege.defense_deck_assign_list.Where(x => x.base_number == item.base_number).Select(y => y.base_number).ToList<int>();

                                    //com a lista de Deck preciso saber qual deck é deste player

                                    bSiege.InserirSiegePlayerDefense(new SiegePlayerDefesa()
                                    {
                                        Date = UnixTimeStampToDateTime(Convert.ToDouble(item.log_timestamp)),

                                        IdDeck = 0, //Se gerou o LOG depois que terminou a Siege nao consigo ver se tem  Deck. 
                                                    //tenho que ver se vem no GetGuildSiegeBattleLogByWizardId
                                                    //ID FK
                                        IdPlayerOponente = lstOponentes.First(x => x.IdGuild == item.opp_guild_id && x.IdPlayer == item.opp_wizard_id && x.Siege.IdSiege == item.siege_id).Id,
                                        IdGuildaOpp = item.opp_guild_id,
                                        IdPlayer = item.wizard_id,
                                        Vitoria = item.win_lose,
                                        IdSiege = idSiege,
                                        Base = item.base_number

                                    }

                                   );
                                }
                            }

                            cont++;
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                string log = "Erro ao tentar Inserir Ataques e Defesas.";
                log += "\nErro:" + ex.Message;
                GravarLog(log);
                throw ex;
            }

        }
        #endregion

        #region Time Defesa
        private void GravarTimeDefesa(List<InfoSiegeDefense.Root> rootDefesas, long idSiege)
        {
            string logmonstro1 = string.Empty, logmonstro2 = string.Empty, logmonstro3 = string.Empty;
            try
            {
                Dados.BLO.BLO_Siege bSiege = new Dados.BLO.BLO_Siege();
                int cont = 1;

                //Obter os Decks para deixar na memória para não ficar toda hora indo no banco. 
                List<SiegeDefenseDeck> lstDefenseDecks = new List<SiegeDefenseDeck>();
                lstDefenseDecks = bSiege.ListarDefenseDecks(idSiege);

                //SiegeTimeDefesas
                foreach (var root in rootDefesas)
                {
                    //PainelLoadSiege(true, "Gravando Times Defesa", cont + "/" + rootDefesas.Count.ToString(), false);

                    long monstro1 = 0, monstro2 = 0, monstro3 = 0, idDeck = 0;
                    int basedefesa = 0;


                    foreach (var item in root.defense_unit_list)
                    {
                        //Se o ID do Deck mudar tem que inserer a Def
                        if (idDeck != item.deck_id && idDeck > 0)
                        {
                            //Inserir time defesa

                            logmonstro1 = monstro1.ToString();
                            logmonstro2 = monstro2.ToString();
                            logmonstro3 = monstro3.ToString();

                            if (lstDefenseDecks.Any(x => x.IdGuild == root.wizard_info_list[0].guild_id && x.IdPlayer == root.wizard_info_list[0].wizard_id && x.IdSiege == idSiege && x.IdDeck == idDeck))
                            {
                                bSiege.InserirSiegeTimeDefesa(new SiegeTimeDefesa()
                                {
                                    Base = basedefesa,
                                    Monstro1 = monstro1,
                                    Monstro2 = monstro2,
                                    Monstro3 = monstro3,
                                    //Verificar depois se esse Wizard_iunfo_list vem mais de 1, pq parece q mesmo sendo uma lista só vem 1
                                    IdDeck = lstDefenseDecks.First(x => x.IdGuild == root.wizard_info_list[0].guild_id && x.IdPlayer == root.wizard_info_list[0].wizard_id && x.IdSiege == idSiege && x.IdDeck == idDeck).Id
                                });
                            }

                        }


                        if (item.pos_id == 1)
                        { monstro1 = item.unit_info.unit_master_id; }
                        if (item.pos_id == 2)
                        { monstro2 = item.unit_info.unit_master_id; }
                        if (item.pos_id == 3)
                        { monstro3 = item.unit_info.unit_master_id; }

                        idDeck = item.deck_id;
                        if (root.defense_assign_list.Any(x => x.deck_id == item.deck_id))
                        { basedefesa = root.defense_assign_list.First(x => x.deck_id == item.deck_id).base_number; }
                        else { basedefesa = 0; }

                    }

                    if (idDeck > 0)
                    {
                        //Inserir time defesa
                        logmonstro1 = monstro1.ToString();
                        logmonstro2 = monstro2.ToString();
                        logmonstro3 = monstro3.ToString();

                        if (lstDefenseDecks.Any(x => x.IdGuild == root.wizard_info_list[0].guild_id && x.IdPlayer == root.wizard_info_list[0].wizard_id && x.IdSiege == idSiege && x.IdDeck == idDeck))
                        {
                            bSiege.InserirSiegeTimeDefesa(new SiegeTimeDefesa()
                            {
                                Base = basedefesa,
                                Monstro1 = monstro1,
                                Monstro2 = monstro2,
                                Monstro3 = monstro3,
                                //Verificar depois se esse Wizard_iunfo_list vem mais de 1, pq parece q mesmo sendo uma lista só vem 1
                                IdDeck = lstDefenseDecks.First(x => x.IdGuild == root.wizard_info_list[0].guild_id && x.IdPlayer == root.wizard_info_list[0].wizard_id && x.IdSiege == idSiege && x.IdDeck == idDeck).Id

                            });
                        }

                    }

                    logmonstro1 = string.Empty;
                    logmonstro2 = string.Empty;
                    logmonstro3 = string.Empty;
                    cont++;
                }
            }
            catch (Exception ex)
            {

                string log = "Erro ao tentar Inserir Time Defesa.";
                log += "\nErro:" + ex.Message;
                log += "\nMonstro 1:" + logmonstro1;
                log += "\nMonstro 2:" + logmonstro2;
                log += "\nMonstro 3:" + logmonstro3;
                GravarLog(log);
                throw ex;
            }
        }

        #endregion

        #endregion

        #region DefesasGVG
        private void CarregarTimeDefesas(string caminhoArquivo)
        {
            string[] lines = System.IO.File.ReadAllLines(caminhoArquivo);
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
                        //TODO: Colocar LOG

                    }

                    Texto = string.Empty;
                }
            }
        }

        void GravarDefesas(List<InfoBatalhaPlayer.BattleLogList> List)
        {
            for (int j = 0; j < List.Count; j++)
            {
                //PainelLoad(true, "Cadastrando os oponentes", (j + 1).ToString() + "/" + List.Count.ToString(), false);
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
            //PainelLoad(false, "-", "-", false);
        }
        #endregion

        #region Utils
        private void FazerBackupArquivo()
        {
            //if (!Directory.Exists(txtDiretorio + "//BeckUp"))
            //{
            //    Directory.CreateDirectory(txtDiretorio + "//BeckUp");
            //}

            //System.IO.File.Copy(txtDiretorio + @"//tempDemonOrange.txt", txtDiretorio + @"//BeckUp//" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".txt");

        }
        //TODO: Colocar parametro para a pasta de LOG
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
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        #endregion

    }
    public class PlayerDef
    {
        public string Nome { get; set; }
    }
}