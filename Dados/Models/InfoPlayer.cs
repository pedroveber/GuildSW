using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InfoPlayer
    {
        public class GuildwarContributeList
        {
            public long wizard_level { get; set; }
            public double contribute { get; set; }
            public string wizard_name { get; set; }
            public long rank { get; set; }
            public long guild_pts { get; set; }
            public long wizard_id { get; set; }
            public long channel_uid { get; set; }
        }

        public class Root
        {
            public long ret_code { get; set; }
            public long ts_val { get; set; }
            public string tzone { get; set; }
            public List<GuildwarContributeList> guildwar_contribute_list { get; set; }
            public long tvalue { get; set; }
            public long tvaluelocal { get; set; }
            public string command { get; set; }
        }
    }
}
