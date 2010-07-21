﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using CMusicSearch.MusicCommon;
using CMusicSearch.MusicRunner;
using CMusicSearch.MusicDownload;


namespace CMusicSearch.Test
{
    public partial class MainForm : Form
    {
        MSLRCRunner Finder = new MSLRCRunner();
        NetworkHelper networkHelp = NetworkHelper.GetNetworkHelperInstance();
        DownloadManagement downloadManager = new DownloadManagement();
        DownloadMusicTask task = null;

        public MainForm()
        {
            InitializeComponent();

            //添加网络监视
            networkHelp.NetworkStatusChanged += new NetworkHelper.NetworkChangedEventHandler(networkHelp_NetworkStatusChanged);
            networkHelp.ListenNetworkStatus(SynchronizationContext.Current);

            //添加下载事件
            downloadManager.DoWork += new DoWorkEventHandler(downloadManager_DoWork);
            downloadManager.ProgressChanged += new ProgressChangedEventHandler(downloadManager_ProgressChanged);
            downloadManager.RunWorkerCompleted += new RunWorkerCompletedEventHandler(downloadManager_RunWorkerCompleted);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SearchMusicInfo info = new SearchMusicInfo() { MusicName = EncodeConverter.UrlEncode(textBox1.Text.Trim()), MusicFormat = SearchMusicFormat.MP3 };
                var list = Finder.SearchM(info);
                info.MusicName = textBox1.Text;
                var lstlrc = Finder.SearchL(info);

                dataGridView1.DataSource = list;
                dataGridView2.DataSource = lstlrc;
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



        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (task != null)
            {
                task.DownloadTaskID = Guid.NewGuid();
                downloadManager.RunWorkerAsync(task);
            }
        }


        void downloadManager_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {


                DownloadCore core = new DownloadCore(downloadManager);
                core.FileDownload((e.Argument as DownloadMusicTask));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void downloadManager_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                DownloadMusicTask task = e.UserState as DownloadMusicTask;
                float totalSize = task.FileSize;
                float downSize = task.DownloadSize;
                float process = 0;
                if (totalSize != 0)
                    process = (downSize / totalSize) * 100;
                if (task.DownloadSpeed != 0)
                    speedlab.Text = String.Format("{0}k/s", task.DownloadSpeed / 1024);
                progressDownload.Value = (int)process;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        void downloadManager_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Exception ex = e.Error;
                DownloadMusicTask result = e.Result as DownloadMusicTask;
                if (ex != null)
                    MessageBox.Show(ex.Message + result.DownloadStatus.ToString());
                else
                    MessageBox.Show(result.DownloadStatus.ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        void networkHelp_NetworkStatusChanged(object sender, NetworkHelper.NetworkChangedEventArgs e)
        {
            if (e.IsNetworkAlive)
                labNetStatus.Text = "网络状态正常";
            else
                labNetStatus.Text = "网络连接失败";
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                task = new DownloadMusicTask();
                task.DownloadUrl = dataGridView1["MusicUrl", e.RowIndex].Value.ToString();
                task.MusicName = dataGridView1["MusicName", e.RowIndex].Value.ToString();

                task.MusicSavePath = string.Format(@"c:\{0}.{1}", task.MusicName, dataGridView1["MusicFormat", e.RowIndex].Value.ToString());
            }
        }

    }
}
