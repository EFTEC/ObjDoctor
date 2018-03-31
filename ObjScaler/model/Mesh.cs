using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjScaler.model
{
    public class Mesh
    {
        public string name;
        
        public List<string> comentario = new List<string>();
        public List<string> contenido = new List<string>();


        public List<Group> group=new List<Group>();
        public Vector min=new Vector(float.MaxValue,float.MaxValue,float.MaxValue);
        public Vector max=new Vector(float.MinValue,float.MinValue,float.MinValue);

        public Mesh()
        {
        }

        public Mesh(string name)
        {
            this.name = name;
        }
    }
}
