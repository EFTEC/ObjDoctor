using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjScaler.model;

namespace ObjScaler.servicio
{
    class ObjDraw
    {
        public Bitmap Draw(WaveFront waveFront,int w,int h,double scale,int axis=0)
        {
            if (w < 0 && h < 0)
            {
                w = 100;
                h = 100;
            }
            var bitmap=new Bitmap(w,h);
            
            var palette =new[] {Color.Blue,Color.Red, Color.DarkBlue,Color.Green,Color.SaddleBrown,Color.DarkGoldenrod};

            int halfh = h / 2;
            int halfw = w / 2;
            int x = 0, y = 0;
            int x2 = 0, y2 = 0;
            FillBitmap(bitmap);
            double scale2 = scale / (double) 1.5;
            DrawLineInt(bitmap, halfw, 0, halfw, h,Color.Black);
            DrawLineInt(bitmap,0, halfh, w, halfh,Color.Black);

            //var scale = Scale(WaveFront.max,WaveFront.min,w,h,axis);
            int numCol = -1;
            
            /*
             var color = palette[0];
            foreach (var v in waveFront.v)
            {
                switch (axis)
                {
                    case 0:
                        x = halfw+ Convert.ToInt32(v.x * scale);
                        y = halfh - Convert.ToInt32(v.y * scale);
                        break;
                    case 1:
                        x = halfw + Convert.ToInt32(v.x * scale);
                        y = halfh - Convert.ToInt32(v.z * scale);
                        break;
                    case 2:
                        x = halfw + Convert.ToInt32(v.y * scale);
                        y = halfh - Convert.ToInt32(v.z * scale);
                        break;
                    case 3:
                        x = halfw + Convert.ToInt32(v.x+(v.z/2) * scale2);
                        y = halfh - Convert.ToInt32(v.y+(v.z/2) * scale2);
                        break;
                }

                bitmap.SetPixel(x,y,color);
            }
            */
      
            foreach (var mesh in waveFront.o)
            {


                foreach (var grupo in mesh.group)
                {
                    numCol++;
                    if (numCol >= palette.Length)
                    {
                        numCol = 0;
                    }

                    var color2 = palette[numCol];
                    int fCount = grupo.f.Count;
                    fCount = (fCount > 50000) ? 50000 : fCount; // hard limit of 50k vertexs.
                    for (var i = 0; i < fCount; i++)
                    {
                        int e =(i<grupo.f.Count-1)?i + 1:0; // we close the face
                        var idxV1 = grupo.f[i];
                        var idxV2 = grupo.f[e];
                        var vector = waveFront.v[idxV1.v - 1];
                        var vector2 = waveFront.v[idxV2.v - 1];
                        switch (axis)
                        {
                            case 0:
                                x =  Convert.ToInt32(vector.x * scale)+halfw;
                                y = halfh - Convert.ToInt32(vector.y * scale);
                                x2 =  Convert.ToInt32(vector2.x * scale)+halfw ;
                                y2 = halfh - Convert.ToInt32(vector2.y * scale);
                                break;
                            case 1:
                                x =  Convert.ToInt32(vector.x * scale)+halfw ;
                                y = halfh - Convert.ToInt32(vector.z * scale);
                                x2 =  Convert.ToInt32(vector2.x * scale)+halfw;
                                y2 = halfh - Convert.ToInt32(vector2.z * scale);
                                break;
                            case 2:
                                x =  Convert.ToInt32(vector.y * scale)+halfw;
                                y = halfh - Convert.ToInt32(vector.z * scale);
                                x2 =  Convert.ToInt32(vector2.y * scale)+halfw;
                                y2 = halfh - Convert.ToInt32(vector2.z * scale);
                                break;
                            case 3:
                                
                                x =  Convert.ToInt32(vector.x+(vector.z/2) * scale2)+halfw;
                                y = halfh - Convert.ToInt32(vector.y+(vector.z/2) * scale2);
                                x2 =  Convert.ToInt32(vector2.x+(vector2.z/2) * scale2)+halfw;
                                y2 = halfh - Convert.ToInt32(vector2.y+(vector2.z/2) * scale2);
                                break;
                        }

                        DrawLineInt(bitmap,x,y,x2,y2,color2);
                    }
                }
            }
       

          
            return bitmap;
        }

        public void FillBitmap(Bitmap bmp)
        {
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255,255,255)))
            {
                gfx.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
            }
        }
        public void DrawLineInt(Bitmap bmp,int x1,int y1,int x2,int y2,Color color)
        {
            
            Pen blackPen = new Pen(color, 1);


            // Draw line to screen.
            using(var graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawLine(blackPen, x1, y1, x2, y2);
            }
        }

    }
}
