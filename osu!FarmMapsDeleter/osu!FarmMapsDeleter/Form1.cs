using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Osu_BeatmapDataReader;
using OsuSongPathSearcher;
using System.IO;
using System.Threading;
using osu_FarmMapsDeleter;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace osu_FarmMapsDeleter
{
   
    public partial class Form1 : Form
    {

        public ListView lv1 = new ListView();
        public ListView lv2 = new ListView();
        public ListView farmMaps = new ListView();
        public string osuSongPath;
     public   decimal totalFarm;
       OsuSongPathSearcher.Class1 PathSearcher = new OsuSongPathSearcher.Class1();
        Osu_BeatmapDataReader.BeatmapReader bmReader = new Osu_BeatmapDataReader.BeatmapReader();
      
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            PathSearcher.GetOsuSongPath();
            textBox1.Text = PathSearcher.OsuSongPath;
            osuSongPath = PathSearcher.OsuSongPath;
            label2.Text = PathSearcher.Status;
            button1.Enabled = false;
            if (PathSearcher.Status == "Auto detecting osu! song folder failed.")
            {
                button1.Enabled = false;
            }
            else
            {
                button2.Enabled = false;
                GetSongs();
                Thread TH = new Thread(FilterFarmMaps);
                TH.Start();
              
               
            }
       



         
         


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
                }
                catch { label2.Text = "Error: Invailid Path" + Environment.NewLine + "(" + osuSongPath + ")"; label6.Visible = true; }

            
        }

        private void FilterFarmMaps()
        {
            label2.Text = "Searching for Farm Maps (it can take a while)";

            userControl11.Maximum = lv1.Items.Count;
            int i = 0; ;
            foreach (ListViewItem item in this.lv1.Items)
            {
                try
                {
                    bmReader.BeatmapPath = item.Text;
                
                bmReader.GetBeatmapData();
                i++;
                if (bmReader.Creator.Contains("Sotarks")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Log Off Now")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("browiec")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Reform")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Kuki1537")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("monstrata")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("A r M i N")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Nevo")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("fieryrage")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("DeRandom Otaku")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Akitoshi")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("SnowNiNo_")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Azunyan")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Fatfan Kolek")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Yuuma")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("PikAqours")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Taeyang")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Kowari")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Shmiklak")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                if (bmReader.Creator.Contains("Lami")) { farmMaps.Items.Add(bmReader.BeatmapPath); }
                label3.Text = "Farm Maps found: " + farmMaps.Items.Count;
                decimal a = farmMaps.Items.Count;
                decimal b = lv1.Items.Count;
                totalFarm = a/b;
                label5.Text = ((a/b)*100).ToString("0.000") + "% of your osu! maps are Farm";
                userControl11.Value = i;
                label8.Text = i + " / " + lv1.Items.Count;
               
                }
                catch {  }
            }
            if (farmMaps.Items.Count == 0)
            {
                button1.Enabled = false;
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
                textBox1.Text = dialog.FileName;
                osuSongPath = textBox1.Text;
                GetSongs();
               Thread TH = new Thread(FilterFarmMaps);
               TH.Start();
                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread Th2 = new Thread(DeleteAllFarm);
            
            Th2.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
