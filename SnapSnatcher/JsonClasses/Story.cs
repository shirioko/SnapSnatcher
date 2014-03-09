using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSnatcher.JsonClasses
{
    public class Story
    {
        [PrimaryKey, AutoIncrement]
        public int entity_id { get; set; }

        [Indexed(Unique = true)]
        public string id { get; set; }

        public string client_id  { get; set; }

        public long timestamp { get; set; }

        public string media_id { get; set; }

        public string media_key { get; set; }

        public string media_iv { get; set; }

        public string thumbnail_iv { get; set; }

        public int media_type { get; set; }

        public int time { get; set; }

        public string caption_text_display { get; set; }

        public bool zipped { get; set; }

        public long time_left { get; set; }

        public string media_url { get; set; }

        public string thumbnail_url { get; set; }

        public string username { get; set; }
    }
}
