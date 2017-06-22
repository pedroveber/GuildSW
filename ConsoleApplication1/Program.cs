using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {


            Dados.DAO.DAO_Lutas.ApagaTudo();
            Dados.DAO.DAO_PlayerOponente.ApagaTudo();
            Dados.DAO.DAO_Player.ApagaTudo();
            Dados.DAO.DAO_Batalha.ApagaTudo();

            string[] lines = System.IO.File.ReadAllLines("full_log.txt");
            //=====================================================================================================================================================================================
            //=====================================================================================================================================================================================
            string Texto = "";
            Boolean Controlador = false;
            foreach (string line in lines)
            {
                if (Controlador)
                {
                    Texto += line;
                    if (line == "")
                    {
                        break;
                    }
                }
                if (line.Contains("Response (GetGuildWarContributeList):"))
                {
                    Controlador = true;
                }
            }
            JavaScriptSerializer Player = new JavaScriptSerializer();
            InfoPlayer.Root ObjPlayer = Player.Deserialize<InfoPlayer.Root>(Texto);
            //=====================================================================================================================================================================================
            //=====================================================================================================================================================================================
            Controlador = false;
            Texto = "";
            foreach (string line in lines)
            {
                if (Controlador)
                {
                    Texto += line;
                    if (line == "")
                    {

                        break;
                    }
                }
                if (line.Contains("Response (GetGuildWarBattleLogByGuildId):"))
                {
                    Controlador = true;
                }
            }
            JavaScriptSerializer Batalhas = new JavaScriptSerializer();
            InfoBatalha ObjBatalha = Batalhas.Deserialize<InfoBatalha>(Texto);
            //=====================================================================================================================================================================================
            //=====================================================================================================================================================================================
            Controlador = false;
            Texto = "";
            foreach (string line in lines)
            {
                if (Controlador)
                {
                    Texto += line;
                    if (line == "")
                    {
                        break;
                    }
                }
                if (line.Contains("Response (GetGuildWarMatchupInfo):"))
                {
                    Controlador = true;
                }
            }
            JavaScriptSerializer Oponente = new JavaScriptSerializer();
            InfoOponente.Root ObjOponente = Oponente.Deserialize<InfoOponente.Root>(Texto);
            //=====================================================================================================================================================================================
            //=====================================================================================================================================================================================
            Controlador = false;
            Texto = "";
            foreach (string line in lines)
            {
                if (Controlador)
                {
                    Texto += line;
                    if (line == "")
                    {
                        break;
                    }
                }
                if (line.Contains("Response (GetGuildInfo):"))
                {
                    Controlador = true;
                }
            }
            JavaScriptSerializer PlayOtherInfo = new JavaScriptSerializer();
            InfoOtherInfoPlayer.Root ObjPlayOtherInfo = PlayOtherInfo.Deserialize<InfoOtherInfoPlayer.Root>(Texto);
            //=====================================================================================================================================================================================
            //=====================================================================================================================================================================================
            Controlador = false;
            Texto = "";
            foreach (string line in lines)
            {
                if (Controlador)
                {
                    Texto += line;
                    if (line == "")
                    {
                        break;
                    }
                }
                if (line.Contains("Response (GetGuildWarParticipationInfo):"))
                {
                    Controlador = true;
                }
            }
            JavaScriptSerializer Paticipante = new JavaScriptSerializer();
            InfoParticipantes.Root ObjPaticipante = Batalhas.Deserialize<InfoParticipantes.Root>(Texto);
            //=====================================================================================================================================================================================
            //=====================================================================================================================================================================================




            // if (ObjPlayer != null && ObjBatalha != null && ObjOponente != null && ObjPlayOtherInfo != null)
            {

                //================================================================
                //================================================================
                //================================================================

                for (int i = 0; i < ObjPlayer.guildwar_contribute_list.Count; i++)
                {
                    new Dados.BLO.BLO_Player().Insert(new Dados.Player()
                    {
                        ID = ObjPlayer.guildwar_contribute_list[i].wizard_id,
                        Nome = ObjPlayer.guildwar_contribute_list[i].wizard_name,
                        Level = ObjPlayer.guildwar_contribute_list[i].wizard_level,
                        PontoArena = ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjPlayer.guildwar_contribute_list[i].wizard_id).FirstOrDefault() != null ?
                        ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjPlayer.guildwar_contribute_list[i].wizard_id).FirstOrDefault().guild_pts : 0,
                        Status = "N"
                    });
                }
                //================================================================
                //================================================================
                //================================================================

                for (int i = 0; i < ObjOponente.my_attack_list.Count; i++)
                {
                    new Dados.BLO.BLO_Player().Insert(new Dados.Player()
                    {
                        ID = ObjOponente.my_attack_list[i].wizard_id,
                        Nome = ObjPlayer.guildwar_contribute_list.Where(w => w.wizard_id == ObjPlayer.guildwar_contribute_list[i].wizard_id).FirstOrDefault().wizard_name,
                        Status = "S"
                    });
                }
                //================================================================
                //================================================================
                //ObjOponente.guildwar_match_info.opp_guild_hp_max
                //ObjOponente.guildwar_match_info.opp_guild_hp_win_cond
                //================================================================           
                Dados.Batalhas Batalha = new Dados.BLO.BLO_Batalha().Insert(new Dados.Batalhas() { ID = ObjBatalha.battle_log_list_group[0].opp_guild_info.guild_id, Guilda = ObjBatalha.battle_log_list_group[0].opp_guild_info.name });
                //================================================================
                //================================================================
                //================================================================           
                for (int i = 0; i < ObjOponente.opp_defense_list.Count; i++)
                {
                    new Dados.BLO.BLO_PlayerOponente().Insert(new Dados.PlayerOponente()
                    {
                        ID = ObjOponente.opp_guild_member_list[i].wizard_id,
                        Nome = ObjOponente.opp_guild_member_list[i].wizard_name,
                        Bonus = ObjOponente.opp_defense_list.Where(w => w.wizard_id == ObjOponente.opp_guild_member_list[i].wizard_id).FirstOrDefault().guild_point_bonus,
                        CodGuilda = Batalha.ID
                    });
                }
                //================================================================
                //================================================================
                //================================================================
                long VidaClan = ObjOponente.guildwar_match_info.opp_guild_hp_max;
                long AuxLife = ObjOponente.opp_defense_list.Count * 10000;
                VidaClan = VidaClan - (VidaClan - AuxLife);
                for (int i = ObjBatalha.battle_log_list_group[0].battle_log_list.Count - 1; i >= 0; i--)
                {
                    int temp = Dados.BLO.BLO_Lutas._SelectAllByBatalha(Batalha).Where(w => w.CodPlayerOponente == ObjBatalha.battle_log_list_group[0].battle_log_list[i].opp_wizard_id).Sum(w => w.Vitoria).Value;
                    int porct = 100;

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
                        if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 2)
                        {
                            VidaClan -= 5100;
                        }
                        else if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 1)
                        {
                            VidaClan -= 3000;
                        }
                    }
                    if (porct == 70)
                    {
                        if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 2)
                        {
                            VidaClan -= 3600;
                        }
                        else if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 1)
                        {
                            VidaClan -= 2100;
                        }
                    }
                    if (porct == 49)
                    {
                        if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 2)
                        {
                            VidaClan -= 2500;
                        }
                        else if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 1)
                        {
                            VidaClan -= 1500;
                        }
                    }
                    if (porct == 34)
                    {
                        if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 2)
                        {
                            VidaClan -= 2000;
                        }
                        else if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 1)
                        {
                            VidaClan -= 1000;
                        }
                    }
                    if (porct == 24)
                    {
                        if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 2)
                        {
                            VidaClan -= 2000;
                        }
                        else if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 1)
                        {
                            VidaClan -= 1000;
                        }
                    }
                    if (porct == 14)
                    {
                        if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 2)
                        {
                            VidaClan -= 1400;
                        }
                        else if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 1)
                        {
                            VidaClan -= 1000;
                        }
                    }
                    if (porct == 4)
                    {
                        if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 2)
                        {
                            VidaClan -= 400;
                        }
                        else if (ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count == 1)
                        {
                            VidaClan -= 400;
                        }
                    }
                    // Unix timestamp is seconds past epoch
                    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds(ObjBatalha.battle_log_list_group[0].battle_log_list[i].battle_end).ToLocalTime();


                    new Dados.BLO.BLO_Lutas().Insert(new Dados.Lutas()
                    {
                        CodBatalhas = Batalha.ID,
                        CodPlayer = ObjBatalha.battle_log_list_group[0].battle_log_list[i].wizard_id,
                        CodPlayerOponente = ObjBatalha.battle_log_list_group[0].battle_log_list[i].opp_wizard_id,
                        Vitoria = ObjBatalha.battle_log_list_group[0].battle_log_list[i].win_count,
                        ValorBarra = porct,
                        DataHora = dtDateTime,
                        MomentoVitoria = VidaClan < ObjOponente.guildwar_match_info.opp_guild_hp_win_cond ? "Win" : "In War"
                    });
                }
                //================================================================
                //================================================================
                //================================================================
            }
        }
    }
}

//SELECT B.id, 
//       B.nome, 
//       Isnull(Sum(A.vitoria), 0) Total_Vitorias, 
//       Count(A.id)               Total_Batalhas, 
//       Sum(CASE 
//             WHEN A.vitoria = 2 THEN C.bonus 
//             ELSE 0 
//           END)                  AS Total_Bonus, 
//       B.pontoarena PontoArena, 
//       B.status NaGVG
//FROM   player B 
//       LEFT JOIN lutas A 
//              ON A.codplayer = B.id 
//       LEFT JOIN playeroponente C 
//              ON A.codplayeroponente = C.id 
//GROUP  BY B.id, 
//          B.nome, 
//          B.pontoarena, 
//          B.status 
//ORDER  BY PontoArena DESC, 
//          total_batalhas DESC; 



//delete FROM [DB_SW_Guild].[dbo].Lutas
//delete FROM [DB_SW_Guild].[dbo].PlayerOponente
//delete FROM [DB_SW_Guild].[dbo].Player
//delete FROM [DB_SW_Guild].[dbo].[Batalhas]




//SELECT b.Nome Atacante, c.Nome Defensor, A.Valorbarra PorcBarra, A.DataHora, MomentoVitoria Status
//  FROM Lutas A
//  inner join Player B on a.CodPlayer = b.ID
//  inner join PlayerOponente c on c.ID=a.CodPlayerOponente