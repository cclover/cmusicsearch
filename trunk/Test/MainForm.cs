using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MusicSearch.MusicCommon;
using System.Threading;
using MusicSearch.MusicRunner;


namespace Test
{
    public partial class MainForm : Form
    {
        MSLRCRunner Finder = new MSLRCRunner();
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Finder.Initialize();
                SearchMusicInfo info = new SearchMusicInfo() { MusicName = EncodeConverter.UrlEncode(textBox1.Text.Trim()), MusicFormat = SearchMusicFormat.MP3 };
                var list = Finder.SearchM(info);
                info.MusicName = textBox1.Text;
                var lstlrc = Finder.SearchL(info);

                dataGridView1.DataSource = list;
                dataGridView2.DataSource= lstlrc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            
           MusicLrcInfo info = ((List<MusicLrcInfo>)dataGridView2.DataSource)[dataGridView2.CurrentRow.Index];
           if (info == null)
           {
               return;
           }

           textBox2.Text = Finder.GetLyricContent(info);
        }
    }
}
