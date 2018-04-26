
namespace ObjDoctor.model
{
    public class Vector 
    {
        public double X;
        public double Y;
        public double Z;

        public Vector()
        {
        }

        public Vector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector Clone()
        {
            return new Vector(X, Y, Z);
        }
    }
}
// Copyright Jorge Castro Castillo March 2018.