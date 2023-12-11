using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleObjectLoader
{
    internal class ObjectConfig
    {
        public string ModId { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }

        public List<string> Categories { get; set; }
        public string TexturePath { get; set; }

    }
}
