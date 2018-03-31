using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjScaler.model;

namespace ObjScaler.servicio
{
    class ObjSrv
    {
        CultureInfo culture = new CultureInfo("en-US");
        public enum ObjMode
        {
            header,mesh,group
        }
        public string ReadFile(string file)
        {
            var r = "";
            r = File.ReadAllText(file);
            r=r.Replace("\r", ""); 
            return r;
        }

        public WaveFront ParseString(string txt)
        {
            var lines = txt.Split(new[] {"\n"},StringSplitOptions.None);
            
            var r = new WaveFront();
            r.original = lines.ToList();

            var mode = ObjMode.header;
            
            foreach (var line in lines)
            {
                if (line == "")
                {
                    continue;
                }

                var cont = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                var first = cont[0];
                var p = line.IndexOf(" ");
                var second = "";
                second = p < 0 ? "" : line.Substring(p + 1);

                mode=ParseHeader(cont, first,second, r);
            }           
            return r;
        }

        public void ParseSave(WaveFront wave,Rescale rescale,string fileName)
        {
            var linesFinal = new List<string>();
            foreach (var line in wave.original)
            {
                if (line == "")
                {
                    linesFinal.Add(line);
                    continue;
                }
                if (line.Substring(0,1) == "#")
                {
                    // add comments.
                    linesFinal.Add(line);
                    continue;
                }
                var cont = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                var first = cont[0];
                var p = line.IndexOf(" ");
                var second = "";
                second = p < 0 ? "" : line.Substring(p + 1);
                switch (first)
                {
                    case "o":
                    case "g":
                    case "\0":
                    case "mtllib":
                    case "usemtl":
                    case "s":
                    case "f":
                    case "vt":
                        linesFinal.Add(line); // we do nothing
                        break;
                    case "v":
                        linesFinal.Add(AddVector(cont,rescale));
                        //AddVector(waveFront, line);
                        break;
                    case "vn":
                        linesFinal.Add(AddNormal(cont,rescale));
                        //waveFront.vn.Add(second);
                        break;
                    default:
                        throw new Exception("Error in group type :["+first+"]");
                }

            }

            string txt = String.Join("\r\n",linesFinal);
            File.WriteAllText(fileName,txt);
            
        }

        private ObjMode ParseHeader(string[] line,string first,string second, WaveFront waveFront)
        {
            var r = ObjMode.header;
            var curMesh = waveFront.o.LastOrDefault();
            var curGroup =curMesh?.group.LastOrDefault();
            switch (first)
            {
                case "#":
                    waveFront.com.Add(second);
                    break;
                case "o":
                    Mesh mesh=new Mesh();
                    mesh.name = second;
                    waveFront.o.Add(mesh);
                    //r = ObjMode.mesh; // cambiamos el modo
                    break;
                case "g":
                    Group g = new Group();
                    g.name = second;
                    if (curMesh == null)
                    {
                        curMesh = new Mesh("Nonane "+waveFront.o.Count);
                        waveFront.o.Add(curMesh);
                    }
                    curMesh.group.Add(g);
                    //r = ObjMode.group; // cambiamos a modo grupo.
                    break;
                case "vn":
                    waveFront.vn.Add(second);
                    break;
                case "vt":
                    waveFront.vt.Add(second);
                    break;
                case "mtllib":
                    waveFront.mtllib.Add(second);
                    break;
                case "usemtl":
                    waveFront.usemtl.Add(second);
                    break;
                case "s":
                    waveFront.s.Add(second);
                    break;
                case "v":
                    AddVector(waveFront, line);
                    break;
                case "f":
                    if (curMesh == null)
                    {
                        curMesh = new Mesh("Nonane "+waveFront.o.Count);
                        waveFront.o.Add(curMesh);
                    }
                    if (curGroup == null)
                    {
                        curGroup = new Group("Nonane "+curMesh.group.Count);
                        curMesh.group.Add(curGroup);
                    }

                    AddFace(curGroup, line);
                    break;
                case "\0":
                    break;
                default:
                    if (first.Substring(0, 1) == "#")
                    {
                        waveFront.com.Add(first.Substring(1));
                    }
                    else
                    {
                        throw new Exception("Error in group type :["+first+"]");
                    }
                    break;                    
            }

            return r;
        }

        private void AddFace(Group curGroup, string[] line)
        {
            for (var i = 1; i < line.Length; i++)
            {
                var x = line[i];
                var arr = x.Split(new[] {"/"}, StringSplitOptions.None);
                var face = new Face();
                switch (arr.Length)
                {
                    case 1:
                        face.v = Convert.ToInt32(arr[0]);
                        break;
                    case 2:
                        face.v = Convert.ToInt32(arr[0]);
                        face.t =arr[1]==""?-1: Convert.ToInt32(arr[1]);
                        break;
                    case 3:
                        face.v = Convert.ToInt32(arr[0]);
                        face.t = arr[1]==""?-1:Convert.ToInt32(arr[1]);
                        face.n = arr[2]==""?-1:Convert.ToInt32(arr[2]);
                        break;
                    default:
                        throw new Exception("Face with too many values");
                }

                curGroup.f.Add(face);
            }
        }

        private void AddVector(WaveFront waveFront, string[] line)
        {
            if (line.Length < 4)
            {
                //error, vector incorrectly defined.
                throw new Exception("Error in vector");
            }

            var culture = new CultureInfo("en-US");
            Vector vector=new Vector(double.Parse(line[1], culture)
                ,double.Parse(line[2], culture)
                ,double.Parse(line[3], culture));
            waveFront.v.Add(vector);
        }

        private string AddVector( string[] line,Rescale r)
        {
            if (line.Length < 4)
            {
                //error, vector incorrectly defined.
                throw new Exception("Error in vector");
            }

            
            Vector vector=new Vector(double.Parse(line[1], culture)
                ,double.Parse(line[2], culture)
                ,double.Parse(line[3], culture));
            vector=r.Modify(vector);
            return "v "+vector.x.ToString("0.########",culture) + " "
                                              + vector.y.ToString("0.########",culture) + " "
                                              + vector.z.ToString("0.########",culture);

        }
        private string AddNormal( string[] line,Rescale r)
        {
            if (line.Length < 4)
            {
                //error, vector incorrectly defined.
                throw new Exception("Error in vector");
            }

            
            Vector vector=new Vector(double.Parse(line[1], culture)
                ,double.Parse(line[2], culture)
                ,double.Parse(line[3], culture));
            vector = r.ModifyNormal(vector);            
            return "vn "+vector.x.ToString("0.########",culture) + " "
                                              + vector.y.ToString("0.########",culture) + " "
                                              + vector.z.ToString("0.########",culture);

        }

        /// <summary>
        /// Obtenemos el minimo y maximo.
        /// </summary>
        /// <param name="waveFront"></param>
        public void ObjGetStat(WaveFront waveFront)
        {
            // reset
            waveFront.min=new Vector(float.MaxValue,float.MaxValue,float.MaxValue);
            waveFront.max=new Vector(float.MinValue,float.MinValue,float.MinValue);

            foreach (var v in waveFront.v)
            {
                waveFront.min.x = (waveFront.min.x > v.x) ? v.x : waveFront.min.x;
                waveFront.min.y = (waveFront.min.y > v.y) ? v.y : waveFront.min.y;
                waveFront.min.z = (waveFront.min.z > v.z) ? v.z : waveFront.min.z;
                waveFront.max.x = (waveFront.max.x < v.x) ? v.x : waveFront.max.x;
                waveFront.max.y = (waveFront.max.y < v.y) ? v.y : waveFront.max.y;
                waveFront.max.z = (waveFront.max.z < v.z) ? v.z : waveFront.max.z;
            }
     
        }


        public double Scale(Vector max, Vector min,int w,int h, int axis)
        {
            double r = 1;
            var maxX = Math.Abs(max.x) > Math.Abs(min.x)?Math.Abs(max.x):Math.Abs(min.x);
            var maxY = Math.Abs(max.y) > Math.Abs(min.y)?Math.Abs(max.y):Math.Abs(min.y);
            var maxZ = Math.Abs(max.z) > Math.Abs(min.z)?Math.Abs(max.z):Math.Abs(min.z);
            var maxAll=Math.Max(maxX, maxY);
            maxAll=Math.Max(maxAll, maxZ);
            double minArea = Math.Min(w/2, h/2)-10;
            r = minArea / maxAll;

            return r;
        }

        public List<Vector> CloneList(List<Vector> oldList)
        {
            var newList = new List<Vector>();
            newList = oldList.ConvertAll(p => p.Clone()).ToList();

            return newList;
        }
    }
}
