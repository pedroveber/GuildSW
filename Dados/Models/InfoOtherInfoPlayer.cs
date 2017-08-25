using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InfoOtherInfoPlayer
    {
        public class CacheExpireTime
        {
            public string date { get; set; }
            public long? timezone_type { get; set; }
            public string timezone { get; set; }
        }

        public class CreateTime
        {
            public string date { get; set; }
            public long? timezone_type { get; set; }
            public string timezone { get; set; }
        }

        public class GuildInfo
        {
            public string comment { get; set; }
            public string notice { get; set; }
            public string master_wizard_name { get; set; }
            public string name { get; set; }
            public long? level { get; set; }
            public long? master_wizard_level { get; set; }
            public CacheExpireTime cache_expire_time { get; set; }
            public long? experience { get; set; }
            public long? member_now { get; set; }
            public CreateTime create_time { get; set; }
            public long? master_wizard_id { get; set; }
            public long? guild_id { get; set; }
            public long? member_max { get; set; }
        }

        public class GuildSkill
        {
            public long? skill_type { get; set; }
            public long? rid { get; set; }
            public long? guild_id { get; set; }
            public long? total_guild_polong { get; set; }
            public long? skill_level { get; set; }
        }

        public class GuildMember
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

        public class Guild
        {
            public GuildInfo guild_info { get; set; }
            public List<GuildSkill> guild_skills { get; set; }
            public List<GuildMember> guild_members { get; set; }
            public long? price { get; set; }
            public List<object> popup_msgs { get; set; }
            public long? dc_rate { get; set; }
        }

        public class GuildwarRankingInfo
        {
            public long? rating_id { get; set; }
            public long? league_type { get; set; }
            public long? match_score { get; set; }
            public long? total_guild_count { get; set; }
            public long? rank { get; set; }
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
            public long match_score { get; set; }
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

        public class Root
        {
            public Guild guild { get; set; }
            public long? ret_code { get; set; }
            public long? ts_val { get; set; }
            public string tzone { get; set; }
            public long? tvalue { get; set; }
            public long? tvaluelocal { get; set; }
            public GuildwarRankingInfo guildwar_ranking_info { get; set; }
            public string command { get; set; }
            public GuildwarRankingStat guildwar_ranking_stat { get; set; }
        }
    }
}
