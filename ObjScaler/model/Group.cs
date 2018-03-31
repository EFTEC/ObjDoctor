using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjScaler.model
{
    public class Group
    {
        public string name;
        public List<string> comentario = new List<string>();
        public List<Face> f = new List<Face>();

        public Group()
        {
        }

        public Group(string name)
        {
            this.name = name;
        }
    }
}
