using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjScaler.model
{
    public class WaveFront
    {
        public List<string> original= new List<string>();
        public List<string> com = new List<string>();
        public List<Vector> v=new List<Vector>();
        public List<Vector> vBackup=new List<Vector>();
        public List<Mesh> o = new List<Mesh>();
        public List<string> vn = new List<string>();
        public List<string> vt = new List<string>();
        public List<string> s = new List<string>();
        public List<string> mtllib = new List<string>();
        public List<string> usemtl = new List<string>();
        public Vector min=new Vector(float.MaxValue,float.MaxValue,float.MaxValue);
        public Vector max=new Vector(float.MinValue,float.MinValue,float.MinValue);

    }
}
