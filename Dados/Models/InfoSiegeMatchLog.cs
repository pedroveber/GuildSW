using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados.Models
{
    public class InfoSiegeMatchLog
    {
        public class Root
        {
            public long ret_code { get; set; }
            public long ts_val { get; set; }
            public string tzone { get; set; }
            public long tvalue { get; set; }
            public long tvaluelocal { get; set; }
            public string command { get; set; }
            public List<GuildsiegeMatchLogList> guildsiege_match_log_list { get; set; }
        }

        public class GuildsiegeMatchLogList
        {
            public long siege_id { get; set; }
            public long guild_id { get; set; }
            public int pos_id { get; set; }

            public List<GuildInfo> guild_info { get; set; }

        }
        public class GuildInfo
        {
            public long siege_id { get; set; }
            public long guild_id { get; set; }
            public int pos_id { get; set; }
            public int match_rank { get; set; }
            public string guild_name { get; set; }
        }
    }
}
