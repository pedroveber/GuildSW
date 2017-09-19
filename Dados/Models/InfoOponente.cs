using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados.Models
{
    public class InfoOponente
    {
        public class OppGuildMemberList
        {
            public long last_login_time { get; set; }
            public long wizard_level { get; set; }
            public long grade { get; set; }
            public long arena_score { get; set; }
            public long wizard_id { get; set; }
            public long rating_id { get; set; }
            public string wizard_name { get; set; }
            public long channel_uid { get; set; }
            public long guild_id { get; set; }
        }

        public class MyAttackList
        {
            public long guild_point_var { get; set; }
            public long energy { get; set; }
            public long guild_id { get; set; }
            public long match_id { get; set; }
            public long wizard_id { get; set; }
        }

        public class GuildwarMatchInfo
        {
            public long member_count_current { get; set; }
            public long league_type { get; set; }
            public long win_wait_remained { get; set; }
            public long match_id { get; set; }
            public long opp_guild_id { get; set; }
            public long def_num { get; set; }
            public long atk_num { get; set; }
            public long guild_rating_id { get; set; }
            public string create_time { get; set; }
            public long status { get; set; }
            public long opp_guild_hp_win_cond { get; set; }
            public long guild_id { get; set; }
            public long opp_guild_hp_current { get; set; }
            public long end_remained { get; set; }
            public long opp_guild_hp_max { get; set; }
        }

        public class CacheExpireTime
        {
            public string date { get; set; }
            public long timezone_type { get; set; }
            public string timezone { get; set; }
        }

        public class CreateTime
        {
            public string date { get; set; }
            public long timezone_type { get; set; }
            public string timezone { get; set; }
        }

        public class OppGuildInfo
        {
            public string comment { get; set; }
            public string notice { get; set; }
            public string master_wizard_name { get; set; }
            public string name { get; set; }
            public long level { get; set; }
            public long master_wizard_level { get; set; }
            public CacheExpireTime cache_expire_time { get; set; }
            public long experience { get; set; }
            public long member_now { get; set; }
            public CreateTime create_time { get; set; }
            public long master_wizard_id { get; set; }
            public long guild_id { get; set; }
            public long member_max { get; set; }
        }

        public class MyAtkdefList
        {
            public long def_wizard_id { get; set; }
            public long match_id { get; set; }
            public long atk_wizard_id { get; set; }
        }

        public class OppDefenseList
        {
            public int guild_point_bonus { get; set; }
            public long match_id { get; set; }
            public long guild_polong_bonus { get; set; }
            public long wizard_id { get; set; }
            public long hp_current { get; set; }
            public long rank { get; set; }
            public long guild_id { get; set; }
            public long last_arena_score { get; set; }
            public long last_rating_id { get; set; }
            public long hp_max { get; set; }
        }

        public class OppParticipationInfo
        {
            public long league_type { get; set; }
            public long match_lose { get; set; }
            public long member_count { get; set; }
            public long energy { get; set; }
            public long match_score { get; set; }
            public long energy_max { get; set; }
            public long energy_regen_remained { get; set; }
            public long guild_id { get; set; }
            public long participated { get; set; }
            public long match_win { get; set; }
        }

        public class Root
        {
            public List<object> guildwar_my_dead_unit_id_list { get; set; }
            public long ret_code { get; set; }
            public long ts_val { get; set; }
            public List<OppGuildMemberList> opp_guild_member_list { get; set; }
            public string tzone { get; set; }
            public List<MyAttackList> my_attack_list { get; set; }
            public long tvalue { get; set; }
            public long tvaluelocal { get; set; }
            public string command { get; set; }
            public GuildwarMatchInfo guildwar_match_info { get; set; }
            public OppGuildInfo opp_guild_info { get; set; }
            public List<MyAtkdefList> my_atkdef_list { get; set; }
            public List<OppDefenseList> opp_defense_list { get; set; }
            public OppParticipationInfo opp_participation_info { get; set; }
        }
    }
}