using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ObjScaler.model;
using ObjScaler.servicio;

namespace ObjScaler
{
    public partial class Form1 : Form
    {
        private ObjSrv objSrv = new ObjSrv();
        private ObjDraw objDraw = new ObjDraw();
        private WaveFront _waveFront =null;
        private Bitmap bitmap = null;
        private bool _refresh=true;
        private Rescale rescale=new Rescale();
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void DrawStat()
        {
            listBox1.Items.Clear();
            foreach (var mesh in _waveFront.o)
            {
                foreach (var group in mesh.@group)
                {
                    if (group.f.Count > 0)
                    {
                        listBox1.Items.Add(new {id = group.name, txt = group.name + " (" + group.f.Count + ")"});
                    }
                }
            }

            var culture = new CultureInfo("en-US");
            txtMax.Text =  string.Format(culture,"x: {0:0.00000000}, y:{1:0.00000000}, z:{2:0.00000000}"
                ,_waveFront.max.x
                ,_waveFront.max.y
                ,_waveFront.max.z);
            txtMin.Text =  string.Format(culture,"x: {0:0.00000000}, y:{1:0.00000000}, z:{2:0.00000000}"
                ,_waveFront.min.x
                ,_waveFront.min.y
                ,_waveFront.min.z);
            double cx = (_waveFront.max.x + _waveFront.min.x) / 2;
            double cy = (_waveFront.max.y + _waveFront.min.y) / 2;
            double cz = (_waveFront.max.z + _waveFront.min.z) / 2;

            txtCenter.Text =  string.Format(culture,"x: {0:0.00000000}, y:{1:0.00000000}, z:{2:0.00000000}"
                ,cx
                ,cy
                ,cz);

            txtDimension.Text = string.Format(culture,"x: {0}, y:{1}, z:{2}"
                ,(_waveFront.max.x-_waveFront.min.x).ToString(culture)
                ,(_waveFront.max.y-_waveFront.min.y).ToString(culture)
                ,(_waveFront.max.z-_waveFront.min.z).ToString(culture));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            /*if (!_refresh)
            {
                return;
            }*/

            if (_waveFront == null)
            {
                return; //  nothing to draw.
            }

            if (bitmap == null)
            {
                return;
            }



            // e.Graphics=Graphics.FromImage(bitmap);
            e.Graphics.DrawImageUnscaled(bitmap, Point.Empty);

            //_refresh = false;

            /*
                e.Graphics.FillEllipse(Brushes.Yellow,0,0,150,150);

                e.Graphics.FillEllipse(Brushes.Black,30,70,20,20);

                e.Graphics.FillEllipse(Brushes.Black,100,70,20,20);

                e.Graphics.FillEllipse(Brushes.Black,30,100,90,20);

                e.Graphics.FillEllipse(Brushes.Yellow,30,92,90,20);
            */
        }

        private void panel1_Resize(object sender, EventArgs e)
        {

        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            if (_waveFront == null || bitmap == null)
            {
                return;
            }
            var scale = objSrv.Scale(_waveFront.max, _waveFront.min, panel1.Width, panel1.Height,0);
            int axis = comboBox1.SelectedIndex;
            bitmap = objDraw.Draw(_waveFront,panel1.Width,panel1.Height,scale,axis);
            panel1.Refresh();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objSrv == null || _waveFront==null)
            {
                return;
            }
            var scale = objSrv.Scale(_waveFront.max, _waveFront.min, panel1.Width, panel1.Height,0);
            int axis = comboBox1.SelectedIndex;
            bitmap = objDraw.Draw(_waveFront,panel1.Width,panel1.Height,scale,axis);
            panel1.Refresh();
        }

        private void buttonResize_Click(object sender, EventArgs e)
        {
            FormResize fr=new FormResize();
            
            // recover backup
            _waveFront.v =objSrv.CloneList(_waveFront.vBackup); 
            objSrv.ObjGetStat(_waveFront);
            
            fr.WaveFront = _waveFront;
            fr.rescale = rescale;
            fr.ShowDialog();

            if (!fr.ok)
            {
                rescale=new Rescale(); // reset rescale
                return; // operation cancelled.
            }

            ShowMsg("Calculating");

            // rescale
            var newList = new List<Vector>();
            for (var i = 0; i < _waveFront.v.Count; i++)
            {
                var v = _waveFront.v[i];
                newList.Add(rescale.Modify(v));
            }

            _waveFront.v = newList; // changed the list
            ShowMsg("Drawing...");
            objSrv.ObjGetStat(_waveFront);
            DrawStat();
            var scale = objSrv.Scale(_waveFront.max, _waveFront.min, panel1.Width, panel1.Height,0);
            int axis = comboBox1.SelectedIndex;
            bitmap = objDraw.Draw(_waveFront,panel1.Width,panel1.Height,scale,axis);
            panel1.Refresh();
            HideMsg();
            fr.Close();
        }

  

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about=new AboutBox1();
            about.ShowDialog();

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
            Application.DoEvents();

            DialogResult r=openFileDialog1.ShowDialog();
            

            if (r != DialogResult.OK)
            {
                return;
            }

            if (String.IsNullOrEmpty(openFileDialog1.FileName))
            {
                return;
            }
            ShowMsg("Loading...");
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
            saveFileDialog1.FileName = openFileDialog1.SafeFileName;
       
            this.Text = "ObjScaler - " +Path.GetFileNameWithoutExtension(openFileDialog1.FileName);

            //var txt = objSrv.ReadFile(@"C:\Users\jorge\Documents\modo\triszbrush.OBJ");
            //var txt = objSrv.ReadFile(@"C:\Users\jorge\Documents\modo\caja2.WaveFront");
            var txt = objSrv.ReadFile(openFileDialog1.FileName);
            
            
            _waveFront = objSrv.ParseString(txt);
            objSrv.ObjGetStat(_waveFront);
            ShowMsg("Drawing...");
            DrawStat();
            var scale = objSrv.Scale(_waveFront.max, _waveFront.min, panel1.Width, panel1.Height,0);
            int axis = comboBox1.SelectedIndex;
            bitmap = objDraw.Draw(_waveFront,panel1.Width,panel1.Height,scale,axis);
            _waveFront.vBackup =objSrv.CloneList(_waveFront.v); // backup previous vectors.
            panel1.Refresh();
            HideMsg();
        
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(saveFileDialog1.FileName)) return;
            ShowMsg("Saving...");
            this.Text = "ObjScaler - " +Path.GetFileNameWithoutExtension( saveFileDialog1.FileName);
            objSrv.ParseSave(_waveFront,rescale, saveFileDialog1.FileName);
            HideMsg();
            
        }

        public void ShowMsg(string txt)
        {            
            labelMsg.Text = txt;
            panelMsg.Visible = true;
            Application.DoEvents();
        }

        public void HideMsg()
        {
            panelMsg.Visible =false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var v1 = saveFileDialog1.ShowDialog();
            if (v1 == DialogResult.OK)
            {
                saveToolStripMenuItem_Click(sender, e);
            }

        }
    }
}
