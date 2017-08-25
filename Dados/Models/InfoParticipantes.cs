using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
 public   class InfoParticipantes
    {
        public class GuildwarMemberList
        {
            public long? last_login_time { get; set; }
            public long? wizard_level { get; set; }
            public long? grade { get; set; }
            public long? arena_score { get; set; }
            public long? wizard_id { get; set; }
            public long? rating_id { get; set; }
            public string wizard_name { get; set; }
            public long? channel_uid { get; set; }
            public long? guild_id { get; set; }
        }

        public class GuildwarRankingInfo
        {
            public long? rating_id { get; set; }
            public long? league_type { get; set; }
            public long? match_score { get; set; }
            public long? total_guild_count { get; set; }
            public long? rank { get; set; }
        }

        public class GuildwarParticipationInfo
        {
            public long? league_type { get; set; }
            public long? match_lose { get; set; }
            public long? member_count { get; set; }
            public long? energy { get; set; }
            public long? match_score { get; set; }
            public long? energy_max { get; set; }
            public long? energy_regen_remained { get; set; }
            public long? guild_id { get; set; }
            public long? participated { get; set; }
            public long? match_win { get; set; }
        }

        public class GuildwarReserve
        {
            public long? status { get; set; }
            public long? league_type { get; set; }
            public long? guild_id { get; set; }
            public string last_update { get; set; }
        }

        public class Current
        {
            public long? league_type { get; set; }
            public double ranking_rate { get; set; }
            public long? participation_id { get; set; }
            public double defense_win_ratio { get; set; }
            public double attack_win_ratio { get; set; }
            public long? rating_id { get; set; }
            public long? attack_lose_count { get; set; }
            public long? defense_lose_count { get; set; }
            public long? match_score { get; set; }
            public long? rank { get; set; }
            public long? total_guild_count { get; set; }
            public long? defense_win_count { get; set; }
            public long? attack_win_count { get; set; }
        }

        public class Prev
        {
            public long? league_type { get; set; }
            public double ranking_rate { get; set; }
            public long? participation_id { get; set; }
            public double defense_win_ratio { get; set; }
            public double attack_win_ratio { get; set; }
            public long? rating_id { get; set; }
            public long? attack_lose_count { get; set; }
            public long? defense_lose_count { get; set; }
            public long? match_score { get; set; }
            public long? rank { get; set; }
            public long? total_guild_count { get; set; }
            public long? defense_win_count { get; set; }
            public long? attack_win_count { get; set; }
        }

        public class Best
        {
            public long? league_type { get; set; }
            public double ranking_rate { get; set; }
            public long? participation_id { get; set; }
            public double defense_win_ratio { get; set; }
            public double attack_win_ratio { get; set; }
            public long? rating_id { get; set; }
            public long? attack_lose_count { get; set; }
            public long? defense_lose_count { get; set; }
            public long? match_score { get; set; }
            public long? rank { get; set; }
            public long? total_guild_count { get; set; }
            public long? defense_win_count { get; set; }
            public long? attack_win_count { get; set; }
        }

        public class GuildwarRankingStat
        {
            public Current current { get; set; }
            public Prev prev { get; set; }
            public Best best { get; set; }
        }

        public class GuildMemberDefenseList
        {
            public List<object> unit_list { get; set; }
            public long? wizard_id { get; set; }
        }

        public class Root
        {
            public List<GuildwarMemberList> guildwar_member_list { get; set; }
            public long? ret_code { get; set; }
            public long? ts_val { get; set; }
            public string tzone { get; set; }
            public long? tvalue { get; set; }
            public long? tvaluelocal { get; set; }
            public GuildwarRankingInfo guildwar_ranking_info { get; set; }
            public GuildwarParticipationInfo guildwar_participation_info { get; set; }
            public GuildwarReserve guildwar_reserve { get; set; }
            public string command { get; set; }
            public GuildwarRankingStat guildwar_ranking_stat { get; set; }
            public List<GuildMemberDefenseList> guild_member_defense_list { get; set; }
        }
    }
}
