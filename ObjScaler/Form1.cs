using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ObjDoctor.model;
using ObjDoctor.servicio;
using Timer = System.Timers.Timer;


namespace ObjDoctor
{
    public partial class Form1 : Form
    {
        private ObjSrv objSrv = new ObjSrv();
        private ObjDraw objDraw = new ObjDraw();
        private WaveFront _waveFront =null;
        private Bitmap _bitmap = null;
        private Rescale _rescale=new Rescale();
        private Timer ti;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void DrawStat()
        {
            listBox1.Items.Clear();
            foreach (var mesh in _waveFront.O)
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
                ,_waveFront.Max.X
                ,_waveFront.Max.Y
                ,_waveFront.Max.Z);
            txtMin.Text =  string.Format(culture,"x: {0:0.00000000}, y:{1:0.00000000}, z:{2:0.00000000}"
                ,_waveFront.Min.X
                ,_waveFront.Min.Y
                ,_waveFront.Min.Z);
            double cx = (_waveFront.Max.X + _waveFront.Min.X) / 2;
            double cy = (_waveFront.Max.Y + _waveFront.Min.Y) / 2;
            double cz = (_waveFront.Max.Z + _waveFront.Min.Z) / 2;

            txtCenter.Text =  string.Format(culture,"x: {0:0.00000000}, y:{1:0.00000000}, z:{2:0.00000000}"
                ,cx
                ,cy
                ,cz);

            txtDimension.Text = string.Format(culture,"x: {0}, y:{1}, z:{2}"
                ,(_waveFront.Max.X-_waveFront.Min.X).ToString(culture)
                ,(_waveFront.Max.Y-_waveFront.Min.Y).ToString(culture)
                ,(_waveFront.Max.Z-_waveFront.Min.Z).ToString(culture));
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

            if (_bitmap == null)
            {
                return;
            }



            // e.Graphics=Graphics.FromImage(bitmap);
            e.Graphics.DrawImageUnscaled(_bitmap, Point.Empty);

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
            if (_waveFront == null || _bitmap == null)
            {
                return;
            }
            var scale = objSrv.Scale(_waveFront.Max, _waveFront.Min, panel1.Width, panel1.Height,0);
            int axis = comboBox1.SelectedIndex;
            _bitmap = objDraw.Draw(_waveFront,panel1.Width,panel1.Height,scale,axis);
            panel1.Refresh();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objSrv == null || _waveFront==null)
            {
                return;
            }
            var scale = objSrv.Scale(_waveFront.Max, _waveFront.Min, panel1.Width, panel1.Height,0);
            int axis = comboBox1.SelectedIndex;
            _bitmap = objDraw.Draw(_waveFront,panel1.Width,panel1.Height,scale,axis);
            panel1.Refresh();
        }

        private void buttonResize_Click(object sender, EventArgs e)
        {
            if (_waveFront == null)
            {
                MessageBox.Show("You must load a OBJ first");
                return;
            }
            FormResize fr=new FormResize();
            
            // recover backup
            _waveFront.V =objSrv.CloneList(_waveFront.VBackup); 
            objSrv.ObjGetStat(_waveFront);
            
            fr.WaveFront = _waveFront;
            fr.rescale = _rescale;
            fr.ShowDialog();

            if (!fr.ok)
            {
                _rescale=new Rescale(); // reset rescale
                return; // operation cancelled.
            }

            ShowMsg("Calculating");

            // rescale
            var newList = new List<Vector>();
            for (var i = 0; i < _waveFront.V.Count; i++)
            {
                var v = _waveFront.V[i];
                newList.Add(_rescale.Modify(v));
            }

            _waveFront.V = newList; // changed the list
            ShowMsg("Drawing...");
            objSrv.ObjGetStat(_waveFront);
            DrawStat();
            var scale = objSrv.Scale(_waveFront.Max, _waveFront.Min, panel1.Width, panel1.Height,0);
            int axis = comboBox1.SelectedIndex;
            _bitmap = objDraw.Draw(_waveFront,panel1.Width,panel1.Height,scale,axis);
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

            LoadingAsync();
            /*
            ti =  new System.Timers.Timer(1000);
         

            Thread thread = new Thread(LoadingAsync);
            thread.Start();
            thread.
            */
            

        }

        private void Callback(object state)
        {
            throw new NotImplementedException();
        }

        private void LoadingAsync()
        {
            
            ShowMsg("Loading...");
           // Thread.Sleep(5000);
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
            saveFileDialog1.FileName = openFileDialog1.SafeFileName;
       
            this.Text = "Obj Doctor - " +Path.GetFileNameWithoutExtension(openFileDialog1.FileName);

            //var txt = objSrv.ReadFile(@"C:\Users\jorge\Documents\modo\triszbrush.OBJ");
            //var txt = objSrv.ReadFile(@"C:\Users\jorge\Documents\modo\caja2.WaveFront");
            var txt = objSrv.ReadFile(openFileDialog1.FileName);
            
            
            _waveFront = objSrv.ParseString(txt);
            objSrv.ObjGetStat(_waveFront);
            ShowMsg("Drawing...");
            DrawStat();
            var scale = objSrv.Scale(_waveFront.Max, _waveFront.Min, panel1.Width, panel1.Height,0);
            int axis = comboBox1.SelectedIndex;
            _bitmap = objDraw.Draw(_waveFront,panel1.Width,panel1.Height,scale,axis);
            _waveFront.VBackup =objSrv.CloneList(_waveFront.V); // backup previous vectors.
            panel1.Refresh();
            HideMsg();
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(saveFileDialog1.FileName)) return;
            ShowMsg("Saving...");
            this.Text = "Obj Doctor - " +Path.GetFileNameWithoutExtension( saveFileDialog1.FileName);
            objSrv.ParseSave(_waveFront,_rescale, saveFileDialog1.FileName);
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
