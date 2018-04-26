using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ObjDoctor.model;

namespace ObjDoctor
{
    public partial class FormResize : Form
    {
        public WaveFront WaveFront;
        public Rescale rescale=new Rescale();
        public bool noEvent = false;
        public bool ok = false;
        CultureInfo culture = new CultureInfo("en-US");
        public FormResize()
        {
            InitializeComponent();
            
            
        }

       


        private void FormResize_Load(object sender, EventArgs e)
        {
            RefreshTxt();
        }
        #region dimevent
        private void txtDimX_TextChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            TextBox txt = ((TextBox) sender); // current textbox
            DimChanged(txt,"x");
        }
        
        private void txtDimY_TextChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            
            TextBox txt = ((TextBox) sender); // current textbox
            DimChanged(txt,"y");
        }

        private void txtDimZ_TextChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            
            TextBox txt = ((TextBox) sender); // current textbox
            DimChanged(txt,"z");
        }
        #endregion
        private void RefreshTxt()
        {
            var deltaX = WaveFront.Max.X - WaveFront.Min.X;
            var deltaY = WaveFront.Max.Y - WaveFront.Min.Y;
            var deltaZ = WaveFront.Max.Z - WaveFront.Min.Z;

            noEvent = true;

            chkRatio.Checked = rescale.KeepRatio;
     
            chkAnchorMaxX.Checked = rescale.AnchorMaxX;
            chkAnchorMaxY.Checked = rescale.AnchorMaxY;
            chkAnchorMaxZ.Checked = rescale.AnchorMaxZ;
            chkAnchorMinX.Checked = rescale.AnchorMinX;
            chkAnchorMinY.Checked = rescale.AnchorMinY;
            chkAnchorMinZ.Checked = rescale.AnchorMinZ;

            double transX = 0,transY = 0,transZ = 0;


            


            txtDimX.Text = (deltaX * rescale.XScale).ToString(culture);
            txtDimY.Text = (deltaY * rescale.YScale).ToString(culture);
            txtDimZ.Text = (deltaZ * rescale.ZScale).ToString(culture);

            txtPerX.Text=(rescale.XScale*100).ToString(culture);
            txtPerY.Text=(rescale.YScale*100).ToString(culture);
            txtPerZ.Text=(rescale.ZScale*100).ToString(culture);

            if (rescale.AnchorMaxY)
            {
                transY =  ConvertTxt(txtMaxY, 0);
                rescale.YTranslate =  transY-WaveFront.Max.Y;
                txtMinY.Text = (transY-(deltaY*rescale.YScale)).ToString(culture);
                //txtMaxY.Text = ((WaveFront.max.y * rescale.yScale)-rescale.yTranslate).ToString(culture);
            }
            if (rescale.AnchorMinY)
            {
                transY =  ConvertTxt(txtMinY, 0);
                rescale.YTranslate = transY-WaveFront.Min.Y;
                txtMaxY.Text = (transY+(deltaY*rescale.YScale)).ToString(culture);
                //txtMaxY.Text = ((WaveFront.max.y * rescale.yScale)-rescale.yTranslate).ToString(culture);
            }

            if (!rescale.AnchorMaxY && !rescale.AnchorMinY)
            {
                rescale.YTranslate = 0;                
                // quizas la escala es recalculada.
                txtMaxY.Text = (WaveFront.Max.Y * rescale.YScale).ToString(culture);
                txtMinY.Text = (WaveFront.Min.Y * rescale.YScale).ToString(culture);  
                
            }
            if (rescale.AnchorMaxX)
            {
                transX =  ConvertTxt(txtMaxX, 0);
                rescale.XTranslate = transX-WaveFront.Max.X ;
                
                txtMinX.Text = (transX-(deltaX*rescale.XScale)).ToString(culture);
                //txtMaxX.Text = ((WaveFront.max.x * rescale.xScale)-rescale.xTranslate).ToString(culture);
            }
            if (rescale.AnchorMinX)
            {
                transX =  ConvertTxt(txtMinX, 0);
                rescale.XTranslate = transX-WaveFront.Min.X;
                
                txtMaxX.Text = (transX+(deltaX*rescale.XScale)).ToString(culture);
                //txtMaxX.Text = ((WaveFront.max.x * rescale.xScale)-rescale.xTranslate).ToString(culture);
            }

            if (!rescale.AnchorMaxX && !rescale.AnchorMinX)
            {
                rescale.XTranslate = 0;                
                // quizas la escala es recalculada.
                txtMaxX.Text = (WaveFront.Max.X * rescale.XScale).ToString(culture);
                txtMinX.Text = (WaveFront.Min.X * rescale.XScale).ToString(culture);  
                
            }
            if (rescale.AnchorMaxZ)
            {
                transZ =  ConvertTxt(txtMaxZ, 0);
                rescale.ZTranslate =   transZ-WaveFront.Max.Z;
                txtMinZ.Text = (transZ-(deltaZ*rescale.ZScale)).ToString(culture);
                //txtMaxZ.Text = ((WaveFront.max.z * rescale.zScale)-rescale.zTranslate).ToString(culture);
            }
            if (rescale.AnchorMinZ)
            {
                transZ =  ConvertTxt(txtMinZ, 0);
                rescale.ZTranslate = transZ-WaveFront.Min.Z;
                txtMaxZ.Text = (transZ+(deltaZ*rescale.ZScale)).ToString(culture);
                //txtMaxZ.Text = ((WaveFront.max.z * rescale.zScale)-rescale.zTranslate).ToString(culture);
            }

            if (!rescale.AnchorMaxZ && !rescale.AnchorMinZ)
            {
                rescale.ZTranslate = 0;                
                // quizas la escala es recalculada.
                txtMaxZ.Text = (WaveFront.Max.Z * rescale.ZScale).ToString(culture);
                txtMinZ.Text = (WaveFront.Min.Z * rescale.ZScale).ToString(culture);  
                
            }
            /*
            txtMaxX.Text = ((WaveFront.max.x+rescale.xTranslate) * rescale.xScale).ToString(culture);
            txtMaxY.Text = ((WaveFront.max.y * rescale.yScale)-rescale.yTranslate).ToString(culture);
            txtMaxZ.Text = ((WaveFront.max.z+rescale.zTranslate) * rescale.zScale).ToString(culture);

            txtMinX.Text = (WaveFront.min.x * rescale.xScale).ToString(culture);
            txtMinY.Text = (WaveFront.min.y * rescale.yScale).ToString(culture);
            txtMinZ.Text = (WaveFront.min.z * rescale.zScale).ToString(culture);
            */
            txtDispX.Text=rescale.XTranslate.ToString(culture);
            txtDispY.Text=rescale.YTranslate.ToString(culture);
            txtDispZ.Text=rescale.ZTranslate.ToString(culture);
            noEvent = false;
        }

        private double ConvertTxt(TextBox txt, double def)
        {
            double r = def;
            try
            {
                var str=txt.Text.Replace(",", ".");
                r = double.Parse(str, this.culture);
            }
            catch (Exception )
            {
                return r;
            }
            return r;
        }

        private void chkRatio_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            rescale.KeepRatio = chkRatio.Checked;
            RefreshTxt();
        }

        private void txtDimX_ModifiedChanged(object sender, EventArgs e)
        {

        }

        private void txtPerX_TextChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            
            TextBox txt = ((TextBox) sender); // current textbox
            PerChanged(txt,"x");
        }

        private void PerChanged(TextBox txt, string type)
        {
            double d;
            try
            {
                d = ConvertTxt(txt,0);
            }
            catch (Exception )
            {
                return;
            }
            switch (type)
            {
                case "x":
                    rescale.XScale = d / 100;
                    if (rescale.KeepRatio)
                    {
                        rescale.YScale = rescale.XScale;
                        rescale.ZScale = rescale.XScale;
                    }
                    break;
                case "y":
                    rescale.YScale = d / 100;
                    if (rescale.KeepRatio)
                    {
                        rescale.XScale = rescale.YScale;
                        rescale.ZScale = rescale.YScale;
                    }
                    break;
                case "z":
                    rescale.ZScale = d / 100;
                    if (rescale.KeepRatio)
                    {
                        rescale.XScale = rescale.ZScale;
                        rescale.YScale = rescale.ZScale;
                    }
                    break;
            }

            RefreshTxt();
        }


        private void DimChanged(TextBox txt, string type)
        {
            double d;
            try
            {
                d = Convert.ToDouble(txt.Text);
            }
            catch (Exception )
            {
                return;
            }
            var deltaX = WaveFront.Max.X - WaveFront.Min.X;
            var deltaY = WaveFront.Max.Y - WaveFront.Min.Y;
            var deltaZ = WaveFront.Max.Z - WaveFront.Min.Z;
            switch (type)
            {

                case "x":
                    rescale.XScale = d / deltaX;
                    if (rescale.KeepRatio)
                    {
                        rescale.YScale = rescale.XScale;
                        rescale.ZScale = rescale.XScale;
                    }
                    break;
                case "y":
                    rescale.YScale = d / deltaY;
                    if (rescale.KeepRatio)
                    {
                        rescale.XScale = rescale.YScale;
                        rescale.ZScale = rescale.YScale;
                    }
                    break;
                case "z":
                    rescale.ZScale = d / deltaZ;
                    if (rescale.KeepRatio)
                    {
                        rescale.XScale = rescale.ZScale;
                        rescale.YScale = rescale.ZScale;
                    }
                    break;
            }

            RefreshTxt();
        }




        private void txtPerY_TextChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            
            TextBox txt = ((TextBox) sender); // current textbox
            PerChanged(txt,"y");
        }

        private void txtPerZ_TextChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            
            TextBox txt = ((TextBox) sender); // current textbox
            PerChanged(txt,"z");
        }

        private void chkAnchorMaxX_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.AnchorMaxX = chk.Checked;
            if (chk.Checked)
            {
                rescale.AnchorMinX = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void RefreshAnchor(CheckBox chk)
        {
            noEvent = true;

            if (chk.Checked)
            {
                // desabilitamos todos
                /*
                chkAnchorMaxX.Checked = false;
                chkAnchorMaxY.Checked = false;
                chkAnchorMaxZ.Checked = false;
                chkAnchorMinX.Checked = false;
                chkAnchorMinY.Checked = false;
                chkAnchorMinZ.Checked = false;
                rescale.anchorMaxX = false;
                rescale.anchorMaxY = false;
                rescale.anchorMaxZ = false;
                rescale.anchorMinX = false;
                rescale.anchorMinY = false;
                rescale.anchorMinZ = false;
                */
                // habilitamos el que se habilito
                chk.Checked = true;
    
  
            }
            else
            {
                chk.Checked = false;
            }

            noEvent = false;
        }

        private void chkAnchorMaxY_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.AnchorMaxY = chk.Checked;
            if (chk.Checked)
            {
                rescale.AnchorMinY = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void txtMaxX_TextChanged(object sender, EventArgs e)
        {
            //if (noEvent) return;
            //TextBox txt = ((TextBox) sender); // current textbox
            //PosChanged(txt,"maxx");
            if (noEvent) return;
            RefreshTxt();
        }

        private void PosChanged(TextBox txt, string type)
        {
            double d;
            try
            {
                d = Convert.ToDouble(txt.Text);
            }
            catch (Exception )
            {
                return;
            }
            var deltaX = WaveFront.Max.X - WaveFront.Min.X;
            var deltaY = WaveFront.Max.Y - WaveFront.Min.Y;
            var deltaZ = WaveFront.Max.Z - WaveFront.Min.Z;
            switch (type)
            {

                case "maxx":
                    rescale.XScale = d / deltaX;
                    if (rescale.KeepRatio)
                    {
                        rescale.YScale = rescale.XScale;
                        rescale.ZScale = rescale.XScale;
                    }
                    break;
                case "y":
                    rescale.YScale = d / deltaY;
                    if (rescale.KeepRatio)
                    {
                        rescale.XScale = rescale.YScale;
                        rescale.ZScale = rescale.YScale;
                    }
                    break;
                case "z":
                    rescale.ZScale = d / deltaZ;
                    if (rescale.KeepRatio)
                    {
                        rescale.XScale = rescale.ZScale;
                        rescale.YScale = rescale.ZScale;
                    }
                    break;
            }

            RefreshTxt();
        }

        private void txtMaxAny_TextChanged(object sender, EventArgs e)
        {
            if (noEvent) return;

            CheckBox chk=null;
            var txt = (TextBox) sender;
            switch (txt.Tag.ToString())
            {
                case "maxx":
                    chk = chkAnchorMaxX;
                    break;
                case "minx":
                    chk = chkAnchorMinX;
                    break;
                case "maxy":
                    chk = chkAnchorMaxY;
                    break;
                case "miny":
                    chk = chkAnchorMinY;
                    break;
                case "maxz":
                    chk = chkAnchorMaxZ;
                    break;
                case "minz":
                    chk = chkAnchorMinZ;
                    break;
            }

            if (!chk.Checked)
            {
                
                var va=ConvertTxt(txt, 102030405060);
                if (Math.Abs(va - 102030405060) < 0.001 || txt.Text.Substring(txt.Text.Length-1,1)==".")
                {
                    return; // no hay valor, no hacer nada todavia.
                }
                pretxt(txt.Tag.ToString(),txt);
            }

            RefreshTxt();
        }

        private void pretxt(string txt, TextBox txtBox)
        {
            double original = 0;
            double proportion = 0;
            TextBox txtTmp=new TextBox();

            switch (txt) 
            {
                case "maxx":                    
                    original = WaveFront.Max.X;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"x");
                    //DimChanged("");
                    break;
                case "minx":
                    original = WaveFront.Min.X;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"x");
                    //DimChanged("");
                    break;
                case "maxy":                    
                    original = WaveFront.Max.Y;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"y");
                    //DimChanged("");
                    break;
                case "miny":
                    original = WaveFront.Min.Y;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"y");
                    //DimChanged("");
                    break;
                case "maxz":                    
                    original = WaveFront.Max.Z;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"z");
                    //DimChanged("");
                    break;
                case "minz":
                    original = WaveFront.Min.Z;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"z");
                    //DimChanged("");
                    break;
                default:
                    throw new Exception("No defined");
            }
        }


      
        private void chkAnchorMinY_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.AnchorMinY = chk.Checked;
            if (chk.Checked)
            {
                rescale.AnchorMaxY = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void chkAnchorMinZ_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.AnchorMinZ = chk.Checked;
            if (chk.Checked)
            {
                rescale.AnchorMaxZ = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void chkAnchorMaxZ_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.AnchorMaxZ = chk.Checked;
            if (chk.Checked)
            {
                rescale.AnchorMinZ = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void chkAnchorMinX_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.AnchorMinX = chk.Checked;
            if (chk.Checked)
            {
                rescale.AnchorMaxX = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void btnCenterX_Click(object sender, EventArgs e)
        {
            noEvent = true;
            Button btn = (Button) sender;

            var deltaX = WaveFront.Max.X - WaveFront.Min.X;
            var deltaY = WaveFront.Max.Y - WaveFront.Min.Y;
            var deltaZ = WaveFront.Max.Z - WaveFront.Min.Z;

            switch (btn.Tag.ToString())
            {
                case "x":
                    rescale.AnchorMaxX = true;
                    rescale.AnchorMinX = false;
                                        
                    txtMaxX.Text=(deltaX * rescale.XScale / 2).ToString(culture);
                    
                    break;
                case "y":
                    rescale.AnchorMaxY = true;
                    rescale.AnchorMinY = false;
                                        
                    txtMaxY.Text=(deltaY * rescale.YScale / 2).ToString(culture);
                    
                    break;
                case "z":
                    rescale.AnchorMaxZ = true;
                    rescale.AnchorMinZ = false;
                                        
                    txtMaxZ.Text=(deltaZ * rescale.ZScale / 2).ToString(culture);
                    
                    break;
            }

            noEvent = false;
            RefreshTxt();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ok = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ok = false;
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtDispX_TextChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            double val = ConvertTxt((TextBox) sender, 0);
            if (chkAnchorMaxX.Checked)
            {
                rescale.XTranslate = val;
    
            }
            else
            {
                rescale.XTranslate = val;
          
            }
            double exp = WaveFront.Max.X * rescale.XScale + rescale.XTranslate;
            txtMaxX.Text=(exp).ToString(culture);
            exp = WaveFront.Min.X * rescale.XScale + rescale.XTranslate;
            txtMinX.Text=(exp).ToString(culture);
            noEvent = false;
        }
    }
}
