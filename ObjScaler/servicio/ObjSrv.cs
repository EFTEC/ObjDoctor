using System;
using System.Collections.Generic;

using System.Globalization;
using System.IO;
using System.Linq;

using ObjDoctor.model;

namespace ObjDoctor.servicio
{
    class ObjSrv
    {
        CultureInfo culture = new CultureInfo("en-US");

        public string ReadFile(string file)
        {
            var r =File.ReadAllText(file).Replace("\r", "");
            return r;
        }

        public WaveFront ParseString(string txt)
        {
            var lines = txt.Split(new[] {"\n"},StringSplitOptions.None);
            
            var r = new WaveFront();
            r.Original = lines.ToList();
            
            foreach (var line in lines)
            {
                if (line == "")
                {
                    continue;
                }

                var cont = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                var first = cont[0];
                var p = line.IndexOf(" ", StringComparison.Ordinal);
                string second= p < 0 ? "" : line.Substring(p + 1);

                ParseHeader(cont, first,second, r);
            }           
            return r;
        }

        public void ParseSave(WaveFront wave,Rescale rescale,string fileName)
        {
            var linesFinal = new List<string>();
            foreach (var line in wave.Original)
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

        private void ParseHeader(string[] line,string first,string second, WaveFront waveFront)
        {
 
            var curMesh = waveFront.O.LastOrDefault();
            var curGroup =curMesh?.group.LastOrDefault();
            switch (first)
            {
                case "#":
                    waveFront.Com.Add(second);
                    break;
                case "o":
                    Mesh mesh=new Mesh();
                    mesh.name = second;
                    waveFront.O.Add(mesh);
                    //r = ObjMode.mesh; // cambiamos el modo
                    break;
                case "g":
                    Group g = new Group();
                    g.name = second;
                    if (curMesh == null)
                    {
                        curMesh = new Mesh("Nonane "+waveFront.O.Count);
                        waveFront.O.Add(curMesh);
                    }
                    curMesh.group.Add(g);
                    //r = ObjMode.group; // cambiamos a modo grupo.
                    break;
                case "vn":
                    waveFront.Vn.Add(second);
                    break;
                case "vt":
                    waveFront.Vt.Add(second);
                    break;
                case "mtllib":
                    waveFront.Mtllib.Add(second);
                    break;
                case "usemtl":
                    waveFront.Usemtl.Add(second);
                    break;
                case "s":
                    waveFront.S.Add(second);
                    break;
                case "v":
                    AddVector(waveFront, line);
                    break;
                case "f":
                    if (curMesh == null)
                    {
                        curMesh = new Mesh("Nonane "+waveFront.O.Count);
                        waveFront.O.Add(curMesh);
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
                        waveFront.Com.Add(first.Substring(1));
                    }
                    else
                    {
                        throw new Exception("Error in group type :["+first+"]");
                    }
                    break;                    
            }

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
            Vector vector=new Vector(double.Parse(line[1], culture)
                ,double.Parse(line[2], culture)
                ,double.Parse(line[3], culture));
            waveFront.V.Add(vector);
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
            return "v "+vector.X.ToString("0.########",culture) + " "
                                              + vector.Y.ToString("0.########",culture) + " "
                                              + vector.Z.ToString("0.########",culture);

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
            return "vn "+vector.X.ToString("0.########",culture) + " "
                                              + vector.Y.ToString("0.########",culture) + " "
                                              + vector.Z.ToString("0.########",culture);

        }

        /// <summary>
        /// Obtenemos el minimo y maximo.
        /// </summary>
        /// <param name="waveFront"></param>
        public void ObjGetStat(WaveFront waveFront)
        {
            // reset
            waveFront.Min=new Vector(float.MaxValue,float.MaxValue,float.MaxValue);
            waveFront.Max=new Vector(float.MinValue,float.MinValue,float.MinValue);

            foreach (var v in waveFront.V)
            {
                waveFront.Min.X = (waveFront.Min.X > v.X) ? v.X : waveFront.Min.X;
                waveFront.Min.Y = (waveFront.Min.Y > v.Y) ? v.Y : waveFront.Min.Y;
                waveFront.Min.Z = (waveFront.Min.Z > v.Z) ? v.Z : waveFront.Min.Z;
                waveFront.Max.X = (waveFront.Max.X < v.X) ? v.X : waveFront.Max.X;
                waveFront.Max.Y = (waveFront.Max.Y < v.Y) ? v.Y : waveFront.Max.Y;
                waveFront.Max.Z = (waveFront.Max.Z < v.Z) ? v.Z : waveFront.Max.Z;
            }
     
        }


        public double Scale(Vector max, Vector min,int w,int h, int axis)
        {
            var maxX = Math.Abs(max.X) > Math.Abs(min.X)?Math.Abs(max.X):Math.Abs(min.X);
            var maxY = Math.Abs(max.Y) > Math.Abs(min.Y)?Math.Abs(max.Y):Math.Abs(min.Y);
            var maxZ = Math.Abs(max.Z) > Math.Abs(min.Z)?Math.Abs(max.Z):Math.Abs(min.Z);
            var maxAll=Math.Max(maxX, maxY);
            maxAll=Math.Max(maxAll, maxZ);
            double minArea = Math.Min(w/2, h/2)-10;
            var r = minArea / maxAll;

            return r;
        }

        public List<Vector> CloneList(List<Vector> oldList)
        {
            var newList  = oldList.ConvertAll(p => p.Clone()).ToList();

            return newList;
        }
    }
}
// Copyright Jorge Castro Castillo March 2018.
