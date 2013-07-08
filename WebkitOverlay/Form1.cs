using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;

namespace WebkitOverlay
{
    public partial class Form1 : Form
    {
        public FilterInfoCollection videoDevices { get; set; }
        public VideoCaptureDevice videoSource { get; set; }
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //initialize position of the window in order to listen what are writen in config file
            int x = 0;
            int y = 0;
            this.Width = Properties.Settings.Default.width;
            this.Height = Properties.Settings.Default.height + 48;
            if (Properties.Settings.Default.top != -1)
            {
                y = Properties.Settings.Default.top;
            }
            if (Properties.Settings.Default.left != -1)
            {
                x = Properties.Settings.Default.left;
            }
            if (Properties.Settings.Default.right != -1)
            {
                x = Screen.PrimaryScreen.Bounds.Width - Properties.Settings.Default.right - this.Width;
            }
            if (Properties.Settings.Default.bottom != -1)
            {
                y = Screen.PrimaryScreen.Bounds.Height - Properties.Settings.Default.bottom - this.Height;
            }
            this.TopMost = true;
            this.SetDesktopLocation(x, y);

            //style button
            if (System.IO.File.Exists("./imgs/close_.png"))
            {
                this.closeBtn.Image = Image.FromFile("./imgs/close_.png");
            }
            else
            {
                MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            if (System.IO.File.Exists("./imgs/extend_.png"))
            {
                this.extendBtn.Image = Image.FromFile("./imgs/extend_.png");
            }
            else
            {
                MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            //list video devices
            ListVideoDevice();
            ListingInComboBox();

            this.videoSource = null;
        }

        private void ListVideoDevice()
        {
            // enumerate video devices
            this.videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        private void ListingInComboBox()
        {
            this.DeviceList.Items.Clear();
            foreach (FilterInfo d in this.videoDevices)
            {
                this.DeviceList.Items.Add(d.Name + " --- " + d.MonikerString);
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeBtn_MouseEnter(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("./imgs/close_hover.png"))
            {
                this.closeBtn.Image = Image.FromFile("./imgs/close_hover.png");
            }
            else
            {
                MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void closeBtn_MouseLeave(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("./imgs/close_.png"))
            {
                this.closeBtn.Image = Image.FromFile("./imgs/close_.png");
            }
            else
            {
                MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void extendBtn_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                if (System.IO.File.Exists("./imgs/extend_.png"))
                {
                    this.extendBtn.Image = Image.FromFile("./imgs/extend_.png");
                }
                else
                {
                    MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                if (System.IO.File.Exists("./imgs/small_.png"))
                {
                    this.extendBtn.Image = Image.FromFile("./imgs/small_.png");
                }
                else
                {
                    MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private void extendBtn_MouseEnter(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (System.IO.File.Exists("./imgs/small_hover.png"))
                {
                    this.extendBtn.Image = Image.FromFile("./imgs/small_hover.png");
                }
                else
                {
                    MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                if (System.IO.File.Exists("./imgs/extend_hover.png"))
                {
                    this.extendBtn.Image = Image.FromFile("./imgs/extend_hover.png");
                }
                else
                {
                    MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private void extendBtn_MouseLeave(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (System.IO.File.Exists("./imgs/small_.png"))
                {
                    this.extendBtn.Image = Image.FromFile("./imgs/small_.png");
                }
                else
                {
                    MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                if (System.IO.File.Exists("./imgs/extend_.png"))
                {
                    this.extendBtn.Image = Image.FromFile("./imgs/extend_.png");
                }
                else
                {
                    MessageBox.Show("Veuillez réinstaller l'application", "Erreur Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private void DeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(this.videoSource == null))
                if (this.videoSource.IsRunning)
                {
                    this.videoSource.SignalToStop();
                    this.videoSource = null;
                }

            FilterInfo s = this.videoDevices[this.DeviceList.SelectedIndex];
            // create video source
            this.videoSource = new VideoCaptureDevice(s.MonikerString);
            //this.videoSource.DesiredFrameRate = 30;
            this.videoSource.DesiredFrameSize = new Size(2*this.Width, 2*this.Height);
            // set NewFrame event handler
            this.videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            // start the video source
            this.videoSource.Start();
            this.pictureBox1.Focus();
        }

        private void video_NewFrame( object sender, NewFrameEventArgs eventArgs )
        {
            // get new frame
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            // process the frame
            this.pictureBox1.Image = bitmap;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!(this.videoSource == null))
                if (this.videoSource.IsRunning)
                {
                    this.videoSource.SignalToStop();
                    this.videoSource = null;
                }
        }

    }
}
