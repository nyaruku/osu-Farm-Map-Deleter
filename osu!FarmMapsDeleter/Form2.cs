using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using osu_FarmMapsDeleter;

namespace osu_FarmMapsDeleter
{
    public partial class Form2 : Form
    {
        osu_FarmMapsDeleter.Form1 form1 = new osu_FarmMapsDeleter.Form1();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (string aa in form1.Title)
            {
                listBox1.Items.Add(aa); 
            }

            
        }
    }
}
