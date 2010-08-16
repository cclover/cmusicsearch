using System;
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

        DataTable downListTable = new DataTable(); //下载列表信息

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

            //设置下载信息列表
            DataColumn nameCol = new DataColumn("name", typeof(string));
            DataColumn statusCol = new DataColumn("status", typeof(string));
            DataColumn processCol = new DataColumn("process", typeof(int));
            DataColumn linkCol = new DataColumn("link", typeof(string));
            DataColumn guidCol = new DataColumn("GUID", typeof(Guid));
            downListTable.Columns.Add(nameCol);
            downListTable.Columns.Add(statusCol);
            downListTable.Columns.Add(processCol);
            downListTable.Columns.Add(linkCol);
            downListTable.Columns.Add(guidCol);
            dataGridView3.Columns[0].DataPropertyName = "name";
            dataGridView3.Columns[1].DataPropertyName = "status";
            dataGridView3.Columns[2].DataPropertyName = "process";
            dataGridView3.Columns[3].DataPropertyName = "link";
            dataGridView3.DataSource = downListTable;
        }


        /// <summary>
        /// 搜索歌曲
        /// </summary>
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


        /// <summary>
        /// 搜索歌词
        /// </summary>
        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {

            MusicLrcInfo info = ((List<MusicLrcInfo>)dataGridView2.DataSource)[dataGridView2.CurrentRow.Index];
            if (info == null)
            {
                return;
            }

            textBox2.Text = Finder.GetLyricContent(info);
        }


        /// <summary>
        /// 开始下载
        /// </summary>
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


        /// <summary>
        /// 下载过程更新进度
        /// </summary>
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
                if (task.DownloadSpeed != -1)
                    speedlab.Text = String.Format("{0}k/s", task.DownloadSpeed / 1024);

                //更新下载信息
                DataRow[] updateRow = downListTable.Select("GUID='" + task.DownloadTaskID.ToString() + "'");
                if (updateRow != null && updateRow.Count() > 0)
                {
                    updateRow[0]["process"] = (int)process;
                }

                if (task.DownloadStatus == DownloadStatus.ST_READY_DOWNLOAD)
                {
                    updateRow[0]["status"] = "准备下载";
                }
                else if (task.DownloadStatus == DownloadStatus.ST_IS_DOWNLOAD)
                {
                    updateRow[0]["status"] = "下载中";
                }
                else if (task.DownloadStatus == DownloadStatus.ST_WAIT_DOWNLOAD)
                {
                    updateRow[0]["status"] = "等待下载";
                }
                else if (task.DownloadStatus == DownloadStatus.ST_ERROR_DOWNLOAD)
                {
                    updateRow[0]["status"] = "下载失败";
                }
                else if (task.DownloadStatus == DownloadStatus.ST_STOP_DOWNLOAD)
                {
                    updateRow[0]["status"] = "暂停下载";
                }
                else if (task.DownloadStatus == DownloadStatus.ST_NONE)
                {
                    updateRow[0]["status"] = string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        /// <summary>
        /// 下载结束时提示
        /// </summary>
        void downloadManager_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Exception ex = e.Error;
                DownloadMusicTask result = e.Result as DownloadMusicTask;
                if (ex != null)
                    MessageBox.Show(ex.Message + result.DownloadStatus.ToString());

                //跟新下载状态
                DataRow[] updateRow = downListTable.Select("GUID='" + task.DownloadTaskID.ToString() + "'");
                if (task.DownloadStatus == DownloadStatus.ST_OVER_DOWNLOAD)
                {
                    updateRow[0]["status"] = "下载完成";
                }
                else if (task.DownloadStatus == DownloadStatus.ST_CANCEL_DOWNLOAD)
                {
                    downListTable.Rows.Remove(updateRow[0]);
                }
                else if (task.DownloadStatus == DownloadStatus.ST_NONE)
                {
                    updateRow[0]["status"] = string.Empty;
                }

                //对下载的文件进行操作
                FileManager.FileDownloadOver(result.MusicSavePath, result.MusicConfigPath, result.DownloadStatus);

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 显示网络状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void networkHelp_NetworkStatusChanged(object sender, NetworkHelper.NetworkChangedEventArgs e)
        {
            if (e.IsNetworkAlive)
                labNetStatus.Text = "网络状态正常";
            else
                labNetStatus.Text = "网络连接失败";
        }


        /// <summary>
        /// 生成下载任务
        /// </summary>
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                task = new DownloadMusicTask();
                task.DownloadUrl = dataGridView1["MusicUrl", e.RowIndex].Value.ToString();
                task.MusicName = dataGridView1["MusicName", e.RowIndex].Value.ToString();
                task.MusicSavePath = FileManager.GetSavaPath(task.MusicName, dataGridView1["MusicFormat", e.RowIndex].Value.ToString());
                if (string.IsNullOrEmpty(task.MusicSavePath))
                {
                    task = null;
                    MessageBox.Show("无法保存文件");
                }
            }
        }


        /// <summary>
        /// 提交下载歌曲任务
        /// </summary>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (task != null)
            {
                //设置下载列表
                task.DownloadTaskID = Guid.NewGuid();


                DataRow row = downListTable.NewRow();
                row[0] = task.MusicName;
                row[1] = string.Empty;
                row[2] = 0;
                row[3] = task.DownloadUrl;
                row[4] = task.DownloadTaskID;
                downListTable.Rows.Add(row);


                downloadManager.RunWorkerAsync(task);

            }
        }


        /// <summary>
        /// 暂停和继续
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "停止")
            {
                downloadManager.StopAsync(task.DownloadTaskID);
                button2.Text = "继续";
            }
            else
            {
                if (task != null)
                {
                    downloadManager.RunWorkerAsync(task);
                }
                button2.Text = "停止";
            }
        }


        /// <summary>
        /// 取消下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (task != null)
                downloadManager.CancleAsync(task.DownloadTaskID);
        }

    }
}
