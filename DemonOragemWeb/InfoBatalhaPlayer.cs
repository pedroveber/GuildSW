using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemonOrange
{
    public class InfoBatalhaPlayer
    {

        public class BattleLogList
        {
            public int opp_guild_id { get; set; }
            public int guild_point_var { get; set; }
            public int match_id { get; set; }
            public int wizard_level { get; set; }
            public List<int> result { get; set; }
            public int rid { get; set; }
            public int league_type { get; set; }
            public int opp_channel_uid { get; set; }
            public int opp_wizard_level { get; set; }
            public string opp_guild_name { get; set; }
            public int opp_wizard_id { get; set; }
            public int guild_id { get; set; }
            public string opp_wizard_name { get; set; }
            public int battle_time { get; set; }
            public int wizard_id { get; set; }
            public string guild_name { get; set; }
            public int draw_count { get; set; }
            public string wizard_name { get; set; }
            public int channel_uid { get; set; }
            public int win_count { get; set; }
            public int log_type { get; set; }
            public int battle_end { get; set; }
            public int lose_count { get; set; }
        }

        public class Root
        {
            public string tzone { get; set; }
            public List<BattleLogList> battle_log_list { get; set; }
            public int ts_val { get; set; }
            public int ret_code { get; set; }
            public int tvalue { get; set; }
            public int tvaluelocal { get; set; }
            public string command { get; set; }
            public int log_type { get; set; }
        }

    }
}
