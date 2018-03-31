using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjScaler.model
{
    public class Vector 
    {
        public double x;
        public double y;
        public double z;

        public Vector()
        {
        }

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector Clone()
        {
            return new Vector(x, y, z);
        }
    }
}
