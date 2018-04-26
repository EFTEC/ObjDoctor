using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjDoctor.model
{
    public class Mesh
    {
        public string name;

        public List<Group> group=new List<Group>();
        public Mesh()
        {
        }

        public Mesh(string name)
        {
            this.name = name;
        }
    }
}
// Copyright Jorge Castro Castillo March 2018.