using System.Collections.Generic;

namespace DynamicIPUpdaterForAWS
{
    public class Configs
    {
        public Configs()
        {
            Rules = new List<Rule>();
        }
        public string DeviceName { get; set; }

        public List<Rule> Rules { get; set; }
        public string PublicIp { get; set; }

        public class Rule
        {
            public string SecurityGroupId { get; set; }
            public int Port { get; set; }
        }
    }
}
