
using System.Collections.Generic;


namespace ObjDoctor.model
{
    public class WaveFront
    {
        public List<string> Original= new List<string>();
        public List<string> Com = new List<string>();
        public List<Vector> V=new List<Vector>();
        public List<Vector> VBackup=new List<Vector>();
        public List<Mesh> O = new List<Mesh>();
        public List<string> Vn = new List<string>();
        public List<string> Vt = new List<string>();
        public List<string> S = new List<string>();
        public List<string> Mtllib = new List<string>();
        public List<string> Usemtl = new List<string>();
        public Vector Min=new Vector(float.MaxValue,float.MaxValue,float.MaxValue);
        public Vector Max=new Vector(float.MinValue,float.MinValue,float.MinValue);

    }
}
// Copyright Jorge Castro Castillo March 2018.