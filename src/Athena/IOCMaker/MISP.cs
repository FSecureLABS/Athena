using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Athena
{
    class MISPAttribute
    {
        public string category;
        public string comment;
        public string uuid;
        public string timestamp;
        public bool to_ids;
        public string value;
        public string type;

        public MISPAttribute()
        {
            uuid = Guid.NewGuid().ToString();
        }

    }

    class MISPTag
    {
        public string colour;
        public bool exportable;
        public string name;
    }

    class MISPOrgc
    {
        public string name;
        public string uuid;

        public MISPOrgc()
        {
            uuid = Guid.NewGuid().ToString();
        }
    }

    class MISPEvent
    {
        public string info;
        public string publishtimestamp;
        public string timestamp;
        public string analysis;
        public List<MISPAttribute> Attribute;
        public List<MISPTag> Tag;
        public bool published;
        public string date;
        public MISPOrgc orgc;
        public string threat_level_id;
        public string uuid;

        public MISPEvent()
        {
            uuid = Guid.NewGuid().ToString();
        }

        public void AddAttribute(MISPAttribute att)
        {
            if (Attribute == null) Attribute = new List<MISPAttribute>();
            Attribute.Add(att);
        }

        public void AddTag(MISPTag tag)
        {
            if (Tag == null) Tag = new List<MISPTag>();
            Tag.Add(tag);
        }

    }
}
