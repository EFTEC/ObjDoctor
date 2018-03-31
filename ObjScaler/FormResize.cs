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
using ObjScaler.model;

namespace ObjScaler
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
            var deltaX = WaveFront.max.x - WaveFront.min.x;
            var deltaY = WaveFront.max.y - WaveFront.min.y;
            var deltaZ = WaveFront.max.z - WaveFront.min.z;

            noEvent = true;

            chkRatio.Checked = rescale.keepRatio;
     
            chkAnchorMaxX.Checked = rescale.anchorMaxX;
            chkAnchorMaxY.Checked = rescale.anchorMaxY;
            chkAnchorMaxZ.Checked = rescale.anchorMaxZ;
            chkAnchorMinX.Checked = rescale.anchorMinX;
            chkAnchorMinY.Checked = rescale.anchorMinY;
            chkAnchorMinZ.Checked = rescale.anchorMinZ;

            double transX = 0,transY = 0,transZ = 0;


            


            txtDimX.Text = (deltaX * rescale.xScale).ToString(culture);
            txtDimY.Text = (deltaY * rescale.yScale).ToString(culture);
            txtDimZ.Text = (deltaZ * rescale.zScale).ToString(culture);

            txtPerX.Text=(rescale.xScale*100).ToString(culture);
            txtPerY.Text=(rescale.yScale*100).ToString(culture);
            txtPerZ.Text=(rescale.zScale*100).ToString(culture);

            if (rescale.anchorMaxY)
            {
                transY =  ConvertTxt(txtMaxY, 0);
                rescale.yTranslate =  transY-WaveFront.max.y;
                txtMinY.Text = (transY-(deltaY*rescale.yScale)).ToString(culture);
                //txtMaxY.Text = ((WaveFront.max.y * rescale.yScale)-rescale.yTranslate).ToString(culture);
            }
            if (rescale.anchorMinY)
            {
                transY =  ConvertTxt(txtMinY, 0);
                rescale.yTranslate = transY-WaveFront.min.y;
                txtMaxY.Text = (transY+(deltaY*rescale.yScale)).ToString(culture);
                //txtMaxY.Text = ((WaveFront.max.y * rescale.yScale)-rescale.yTranslate).ToString(culture);
            }

            if (!rescale.anchorMaxY && !rescale.anchorMinY)
            {
                rescale.yTranslate = 0;                
                // quizas la escala es recalculada.
                txtMaxY.Text = (WaveFront.max.y * rescale.yScale).ToString(culture);
                txtMinY.Text = (WaveFront.min.y * rescale.yScale).ToString(culture);  
                
            }
            if (rescale.anchorMaxX)
            {
                transX =  ConvertTxt(txtMaxX, 0);
                rescale.xTranslate = transX-WaveFront.max.x ;
                
                txtMinX.Text = (transX-(deltaX*rescale.xScale)).ToString(culture);
                //txtMaxX.Text = ((WaveFront.max.x * rescale.xScale)-rescale.xTranslate).ToString(culture);
            }
            if (rescale.anchorMinX)
            {
                transX =  ConvertTxt(txtMinX, 0);
                rescale.xTranslate = transX-WaveFront.min.x;
                
                txtMaxX.Text = (transX+(deltaX*rescale.xScale)).ToString(culture);
                //txtMaxX.Text = ((WaveFront.max.x * rescale.xScale)-rescale.xTranslate).ToString(culture);
            }

            if (!rescale.anchorMaxX && !rescale.anchorMinX)
            {
                rescale.xTranslate = 0;                
                // quizas la escala es recalculada.
                txtMaxX.Text = (WaveFront.max.x * rescale.xScale).ToString(culture);
                txtMinX.Text = (WaveFront.min.x * rescale.xScale).ToString(culture);  
                
            }
            if (rescale.anchorMaxZ)
            {
                transZ =  ConvertTxt(txtMaxZ, 0);
                rescale.zTranslate =   transZ-WaveFront.max.z;
                txtMinZ.Text = (transZ-(deltaZ*rescale.zScale)).ToString(culture);
                //txtMaxZ.Text = ((WaveFront.max.z * rescale.zScale)-rescale.zTranslate).ToString(culture);
            }
            if (rescale.anchorMinZ)
            {
                transZ =  ConvertTxt(txtMinZ, 0);
                rescale.zTranslate = transZ-WaveFront.min.z;
                txtMaxZ.Text = (transZ+(deltaZ*rescale.zScale)).ToString(culture);
                //txtMaxZ.Text = ((WaveFront.max.z * rescale.zScale)-rescale.zTranslate).ToString(culture);
            }

            if (!rescale.anchorMaxZ && !rescale.anchorMinZ)
            {
                rescale.zTranslate = 0;                
                // quizas la escala es recalculada.
                txtMaxZ.Text = (WaveFront.max.z * rescale.zScale).ToString(culture);
                txtMinZ.Text = (WaveFront.min.z * rescale.zScale).ToString(culture);  
                
            }
            /*
            txtMaxX.Text = ((WaveFront.max.x+rescale.xTranslate) * rescale.xScale).ToString(culture);
            txtMaxY.Text = ((WaveFront.max.y * rescale.yScale)-rescale.yTranslate).ToString(culture);
            txtMaxZ.Text = ((WaveFront.max.z+rescale.zTranslate) * rescale.zScale).ToString(culture);

            txtMinX.Text = (WaveFront.min.x * rescale.xScale).ToString(culture);
            txtMinY.Text = (WaveFront.min.y * rescale.yScale).ToString(culture);
            txtMinZ.Text = (WaveFront.min.z * rescale.zScale).ToString(culture);
            */

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
            rescale.keepRatio = chkRatio.Checked;
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
                    rescale.xScale = d / 100;
                    if (rescale.keepRatio)
                    {
                        rescale.yScale = rescale.xScale;
                        rescale.zScale = rescale.xScale;
                    }
                    break;
                case "y":
                    rescale.yScale = d / 100;
                    if (rescale.keepRatio)
                    {
                        rescale.xScale = rescale.yScale;
                        rescale.zScale = rescale.yScale;
                    }
                    break;
                case "z":
                    rescale.zScale = d / 100;
                    if (rescale.keepRatio)
                    {
                        rescale.xScale = rescale.zScale;
                        rescale.yScale = rescale.zScale;
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
            var deltaX = WaveFront.max.x - WaveFront.min.x;
            var deltaY = WaveFront.max.y - WaveFront.min.y;
            var deltaZ = WaveFront.max.z - WaveFront.min.z;
            switch (type)
            {

                case "x":
                    rescale.xScale = d / deltaX;
                    if (rescale.keepRatio)
                    {
                        rescale.yScale = rescale.xScale;
                        rescale.zScale = rescale.xScale;
                    }
                    break;
                case "y":
                    rescale.yScale = d / deltaY;
                    if (rescale.keepRatio)
                    {
                        rescale.xScale = rescale.yScale;
                        rescale.zScale = rescale.yScale;
                    }
                    break;
                case "z":
                    rescale.zScale = d / deltaZ;
                    if (rescale.keepRatio)
                    {
                        rescale.xScale = rescale.zScale;
                        rescale.yScale = rescale.zScale;
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
            rescale.anchorMaxX = chk.Checked;
            if (chk.Checked)
            {
                rescale.anchorMinX = false;
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
            rescale.anchorMaxY = chk.Checked;
            if (chk.Checked)
            {
                rescale.anchorMinY = false;
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
            var deltaX = WaveFront.max.x - WaveFront.min.x;
            var deltaY = WaveFront.max.y - WaveFront.min.y;
            var deltaZ = WaveFront.max.z - WaveFront.min.z;
            switch (type)
            {

                case "maxx":
                    rescale.xScale = d / deltaX;
                    if (rescale.keepRatio)
                    {
                        rescale.yScale = rescale.xScale;
                        rescale.zScale = rescale.xScale;
                    }
                    break;
                case "y":
                    rescale.yScale = d / deltaY;
                    if (rescale.keepRatio)
                    {
                        rescale.xScale = rescale.yScale;
                        rescale.zScale = rescale.yScale;
                    }
                    break;
                case "z":
                    rescale.zScale = d / deltaZ;
                    if (rescale.keepRatio)
                    {
                        rescale.xScale = rescale.zScale;
                        rescale.yScale = rescale.zScale;
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
                    original = WaveFront.max.x;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"x");
                    //DimChanged("");
                    break;
                case "minx":
                    original = WaveFront.min.x;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"x");
                    //DimChanged("");
                    break;
                case "maxy":                    
                    original = WaveFront.max.y;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"y");
                    //DimChanged("");
                    break;
                case "miny":
                    original = WaveFront.min.y;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"y");
                    //DimChanged("");
                    break;
                case "maxz":                    
                    original = WaveFront.max.z;
                    proportion =  this.ConvertTxt(txtBox, 0)/original;
                    txtTmp.Text = (proportion*100).ToString(this.culture);
                    PerChanged(txtTmp,"z");
                    //DimChanged("");
                    break;
                case "minz":
                    original = WaveFront.min.z;
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
            rescale.anchorMinY = chk.Checked;
            if (chk.Checked)
            {
                rescale.anchorMaxY = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void chkAnchorMinZ_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.anchorMinZ = chk.Checked;
            if (chk.Checked)
            {
                rescale.anchorMaxZ = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void chkAnchorMaxZ_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.anchorMaxZ = chk.Checked;
            if (chk.Checked)
            {
                rescale.anchorMinZ = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void chkAnchorMinX_CheckedChanged(object sender, EventArgs e)
        {
            if (noEvent) return;
            noEvent = true;
            var chk = (CheckBox) sender;
            rescale.anchorMinX = chk.Checked;
            if (chk.Checked)
            {
                rescale.anchorMaxX = false;
            }       
            noEvent = false;
            RefreshTxt();
        }

        private void btnCenterX_Click(object sender, EventArgs e)
        {
            noEvent = true;
            Button btn = (Button) sender;

            var deltaX = WaveFront.max.x - WaveFront.min.x;
            var deltaY = WaveFront.max.y - WaveFront.min.y;
            var deltaZ = WaveFront.max.z - WaveFront.min.z;

            switch (btn.Tag.ToString())
            {
                case "x":
                    rescale.anchorMaxX = true;
                    rescale.anchorMinX = false;
                                        
                    txtMaxX.Text=(deltaX * rescale.xScale / 2).ToString(culture);
                    
                    break;
                case "y":
                    rescale.anchorMaxY = true;
                    rescale.anchorMinY = false;
                                        
                    txtMaxY.Text=(deltaY * rescale.yScale / 2).ToString(culture);
                    
                    break;
                case "z":
                    rescale.anchorMaxZ = true;
                    rescale.anchorMinZ = false;
                                        
                    txtMaxZ.Text=(deltaZ * rescale.zScale / 2).ToString(culture);
                    
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
    }
}
