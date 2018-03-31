using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjScaler.model
{
    public class Rescale
    {
        public double xScale=1.0d;
        public double yScale=1.0d;
        public double zScale=1.0d;

        public double xTranslate = 0d;
        public double yTranslate = 0d;
        public double zTranslate = 0d;
        public bool anchorMaxX = false;
        public bool anchorMaxY = false;
        public bool anchorMaxZ = false;
        public bool anchorMinX = false;
        public bool anchorMinY = false;
        public bool anchorMinZ = false;

        public bool keepRatio = true;
    

        public Vector Modify(Vector v)
        {
            var re = new Vector();
            re.x = (v.x + xTranslate) * xScale; 
            re.y = (v.y + yTranslate) * yScale; 
            re.z = (v.z + zTranslate) * zScale; 
            return re;
        }
        public Vector ModifyNormal(Vector v)
        {
            var re = new Vector(v.x  * xScale,v.y  * yScale,v.z * zScale);
            double magnitude =Math.Sqrt(re.x * re.x + re.y * re.y + re.z * re.z);
            // normalizing
            re.x = re.x / magnitude;
            re.y = re.y / magnitude;
            re.z = re.z / magnitude;
            return re;
        }
    }
}
