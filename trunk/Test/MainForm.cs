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

        #region 字段
        MSLRCRunner Finder = new MSLRCRunner(); //搜索对象
        NetworkHelper networkHelp = NetworkHelper.GetNetworkHelperInstance();//网络辅助对象
        DownloadManagement downloadManager = new DownloadManagement(); //下载管理器
        DownloadMusicTask task = null; //新下载任务
        DataTable downListTable = new DataTable(); //下载列表信息
        #endregion

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

        #region 网络状态

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

        #endregion

        #region 搜索结果

        /// <summary>
        /// 搜索歌曲
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Thread searchThread = new Thread(
                    delegate()
                    {
                        SearchMusicInfo info = new SearchMusicInfo() { MusicName = EncodeConverter.UrlEncode(textBox1.Text.Trim()), MusicFormat = SearchMusicFormat.MP3 };
                        var list = Finder.SearchM(info);
                        info.MusicName = textBox1.Text;
                        var lstlrc = Finder.SearchL(info);

                        //多线程使用UI控件
                        this.Invoke(new Action<List<MusicInfo>, List<MusicLrcInfo>>(DataBind), new object[] { list, lstlrc });
                    });
                searchThread.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="list"></param>
        /// <param name="lstlrc"></param>
        private void DataBind(List<MusicInfo> list, List<MusicLrcInfo> lstlrc)
        {
            dataGridView1.DataSource = list;
            dataGridView2.DataSource = lstlrc;
        }


        /// <summary>
        /// 搜索歌词
        /// </summary>
        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            MusicLrcInfo info = ((List<MusicLrcInfo>)dataGridView2.DataSource)[dataGridView2.CurrentRow.Index];
            if (info == null)
            {
                return;
            }

            textBox2.Text = Finder.GetLyricContent(info);
        }

        #endregion

        #region 下载状态
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
                //if (task.DownloadSpeed != -1)
                //String.Format("{0}k/s", task.DownloadSpeed / 1024);

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
                DataRow[] updateRow = downListTable.Select("GUID='" + result.DownloadTaskID.ToString() + "'");
                if (result.DownloadStatus == DownloadStatus.ST_OVER_DOWNLOAD)
                {
                    updateRow[0]["status"] = "下载完成";
                }
                else if (result.DownloadStatus == DownloadStatus.ST_CANCEL_DOWNLOAD)
                {
                    downListTable.Rows.Remove(updateRow[0]); //删除table数据
                }
                else if (result.DownloadStatus == DownloadStatus.ST_NONE)
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

        #endregion

        #region 下载的菜单操作

        int downloadIndex = -1;
        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            downloadIndex = e.RowIndex;
            pauseAllToolStripMenuItem.Enabled = true;
            deleteAllToolStripMenuItem.Enabled = true;
            deleteToolStripMenuItem.Enabled = true;
            pauseToolStripMenuItem.Enabled = true;
            if (downListTable.Rows[downloadIndex]["status"].ToString() == "下载完成")
                startToolStripMenuItem.Enabled = true;
            if (downListTable.Rows[downloadIndex]["status"].ToString() == "下载中")
            {
                pauseToolStripMenuItem.Text = "暂停";
            }
            if (downListTable.Rows[downloadIndex]["status"].ToString() == "暂停下载")
            {
                pauseToolStripMenuItem.Text = "继续下载";
            }
            contextMenuDownload.Show(MousePosition);
        }


        /// <summary>
        /// 重新下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 暂停，继续
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Guid taskID = (Guid)downListTable.Rows[downloadIndex]["GUID"];
            if (downListTable.Rows[downloadIndex]["status"].ToString() == "下载中")
            {
                downloadManager.StopAsync(taskID);
            }
            if (downListTable.Rows[downloadIndex]["status"].ToString() == "暂停下载")
            {
                //downloadManager.RunWorkerAsync(taskID);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Guid taskID = (Guid)downListTable.Rows[downloadIndex]["GUID"];
            downloadManager.CancleAsync(taskID);
        }

        /// <summary>
        /// 全部停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            downloadManager.StopAll();
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            downloadManager.CancleAll();
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", FileManager.DOWNLOAD_DIR); 
        }

    }
}
