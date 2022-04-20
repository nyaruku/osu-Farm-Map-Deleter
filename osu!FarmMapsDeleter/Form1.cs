using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace osu_FarmMapsDeleter
{
    public partial class Form1 : Form
    {
        public ListView lv1 = new ListView(); // all maps
        public ListView lv2 = new ListView();
        public ListView farmMaps = new ListView();
        public string osuSongPath;
        public decimal totalFarm;

        public List<string> Title;
        public List<Int32> BeatmapID,BeatmapSetID;
    

        private OsuBeatmapReader.FasterBeatmapReader obm = new OsuBeatmapReader.FasterBeatmapReader();

        public Form1()
        {
            InitializeComponent();
            Title = new List<String>();
            BeatmapID = new List<Int32>();
            BeatmapSetID = new List<Int32>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            button1.Enabled = false;
            button3.Enabled = false;
        }

        private void GetSongs()
        {
            try
            {
                string[] filelist;

                filelist = Directory.GetDirectories(osuSongPath);

                foreach (string s in filelist)
                {
                    string[] TempPath;

                    TempPath = Directory.GetFiles(s);
                    lv2.Items.Add(s);

                    foreach (String a in TempPath)
                    {
                        if (a.Contains(".osu"))
                        {
                            lv1.Items.Add(a);
                            label4.Text = "Total osu! maps: " + lv1.Items.Count;
                        }
                    }
                }
                button3.Enabled = true;
            }
            catch { label2.Text = "Error: Invailid Path" + Environment.NewLine + "(" + osuSongPath + ")"; label6.Visible = true; }
            Cursor = Cursors.Default;
        }

        private void FilterFarmMaps()
        {
            #region Clear List
            if (Title == null)
            {
            }
            else
            {
                Title.Clear();
            }      
            if (BeatmapID == null)
            {
            }
            else
            {
                BeatmapID.Clear();
            }
            if (BeatmapSetID == null)
            {
            }
            else
            {
                BeatmapSetID.Clear();
            }
            #endregion


            int MapTime;
            if (textBox2.Text == "")
            {
                MapTime = 90000;
            }
            else
            {
                MapTime = Convert.ToInt32(textBox2.Text);
            }

            label2.Text = "Searching for Farm Maps (it can take a while)";

            userControl11.Maximum = lv1.Items.Count;
            int i = 0; ;
            foreach (ListViewItem item in this.lv1.Items)
            {
                try
                {
                    i++;
                    obm.GetBeatmapData(item.Text);
                    if (obm.MapLength <= MapTime)
                    {
                        farmMaps.Items.Add(item.Text);
                        if(obm.Title.Length > 50)
                        {
                            string newTitle = obm.Title.PadRight(50) + "... - " + obm.Artist;
                            Title.Add(newTitle + "|" + obm.Version + " by " + obm.Creator);
                        }
                        else
                        {
                            string newTitle = obm.Title + " - " + obm.Artist;
                            Title.Add(newTitle + " | " + obm.Version + " by " + obm.Creator);
                        }
                                           
                        BeatmapID.Add(obm.BeatmapID);
                        BeatmapSetID.Add(obm.BeatmapSetID);
                    }

                    label3.Text = "Farm Maps found: " + farmMaps.Items.Count;
                    decimal a = farmMaps.Items.Count;
                    decimal b = lv1.Items.Count;
                    totalFarm = a / b;
                    label5.Text = ((a / b) * 100).ToString("0.000") + "% of your osu! maps are Farm";
                    userControl11.Value = i;
                    label8.Text = i + " / " + lv1.Items.Count;
                }
                catch { }
            }
            if (farmMaps.Items.Count == 0)
            {
                button1.Enabled = false;
                button3.Enabled = true;
                label2.Text = "No Farm Maps Found";
            }
            else
            {
                button1.Enabled = true;

                label2.Text = "You can now click 'DELETE ALL FARM'.";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            Application.Exit();
        }

        public void DeleteAllFarm()
        {
            userControl11.ProgressBarColor = Color.Red;
            label2.Text = "All found Farms will be now deleted...";
            userControl11.Value = 0;
            userControl11.Maximum = farmMaps.Items.Count;
            foreach (ListViewItem item in this.farmMaps.Items)
            {
                try
                {
                    File.Delete(item.Text);

                    farmMaps.Items.Remove(item);

                    decimal a = farmMaps.Items.Count;
                    decimal b = lv1.Items.Count;
                    totalFarm = a / b;
                    label5.Text = ((a / b) * 100).ToString("0.000") + "% of your osu! maps are Farm";
                    userControl11.Value++;
                    label8.Text = farmMaps.Items.Count + " / " + lv1.Items.Count;
                    Thread.Sleep(1);
                }
                catch { }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Cursor = Cursors.WaitCursor;
                textBox1.Text = dialog.FileName;
                osuSongPath = textBox1.Text;
                GetSongs();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread Th2 = new Thread(DeleteAllFarm);

            Th2.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            osuSongPath = textBox1.Text;
            button3.Enabled = false;

            // GetSongs();
            Thread TH = new Thread(FilterFarmMaps);
            TH.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}