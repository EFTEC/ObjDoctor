using System;


namespace ObjDoctor.model
{
    public class Rescale
    {
        public double XScale=1.0d;
        public double YScale=1.0d;
        public double ZScale=1.0d;

        public double XTranslate = 0d;
        public double YTranslate = 0d;
        public double ZTranslate = 0d;
        public bool AnchorMaxX = false;
        public bool AnchorMaxY = false;
        public bool AnchorMaxZ = false;
        public bool AnchorMinX = false;
        public bool AnchorMinY = false;
        public bool AnchorMinZ = false;

        public bool KeepRatio = true;
    

        public Vector Modify(Vector v)
        {
            var re = new Vector();
            re.X = (v.X + XTranslate) * XScale; 
            re.Y = (v.Y + YTranslate) * YScale; 
            re.Z = (v.Z + ZTranslate) * ZScale; 
            return re;
        }
        public Vector ModifyNormal(Vector v)
        {
            var re = new Vector(v.X  * XScale,v.Y  * YScale,v.Z * ZScale);
            double magnitude =Math.Sqrt(re.X * re.X + re.Y * re.Y + re.Z * re.Z);
            // normalizing
            re.X = re.X / magnitude;
            re.Y = re.Y / magnitude;
            re.Z = re.Z / magnitude;
            return re;
        }
    }
}
// Copyright Jorge Castro Castillo March 2018.