using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using CMusicSearch.MusicCommon;

namespace CMusicSearch.MusicDownload
{
    /// <summary>
    /// 下载音乐文件
    /// </summary>
    class DownloadManagement
    {
        /// <summary>
        /// 等待下载的任务列表
        /// </summary>
        private List<DownloadMusicTask> waitDownloadList;

        /// <summary>
        /// 下载中的任务列表
        /// </summary>
        private Dictionary<Guid, DownloadMusicTask> downloadTable;

        /// <summary>
        /// 管理任务同步的列表
        /// </summary>
        private Dictionary<Guid ,AsyncOperation> asyncOperationtTable;

        //同步锁
        private static object waitLock;
        private static object downloadLock;
        private static object asyncLock;


        //定义开始，进行，结束相关的委托对象
        //定义这些委托和相关事件的触发方法相绑定是为了工作线程和UI线程间通信
        private delegate void WorkerThreadStartDelegate(object argument);
        private readonly WorkerThreadStartDelegate threadStart;
        private readonly SendOrPostCallback progressReporter;
        private readonly SendOrPostCallback operationCompleted;


        /// <summary>
        /// 构造函数
        /// </summary>
        public DownloadManagement()
        {
            //初始化管理下载的数据结构
            waitDownloadList = new List<DownloadMusicTask>();
            downloadTable = new Dictionary<Guid, DownloadMusicTask>();
            asyncOperationtTable = new Dictionary<Guid, AsyncOperation>();

            //绑定委托事件
            this.threadStart = new WorkerThreadStartDelegate(this.DownloadThreadStart);
            this.progressReporter = new SendOrPostCallback(this.ProgressReporter);
            this.operationCompleted = new SendOrPostCallback(this.AsyncOperationCompleted);
        }

        static DownloadManagement()
        {
            waitLock = new object();
            downloadLock = new object();
            asyncLock = new object();
        }

        #region 下载事件处理
        
        //使用自己实现事件
        //private static readonly object doWorkKey;
        //private static readonly object progressChangedKey;
        //private static readonly object runWorkerCompletedKey;

        /// <summary>
        /// 开始下载事件
        /// </summary>
        public event DoWorkEventHandler DoDownload;


        /// <summary>
        /// 下载中事件
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;


        /// <summary>
        /// 下载完成事件
        /// </summary>
        public event RunWorkerCompletedEventHandler DoDownloadrCompleted;


        /// <summary>
        /// 引发开始下载事件的方法
        /// </summary>
        protected virtual void OnDoDownload(DoWorkEventArgs e)
        {    
            if (DoDownload != null)
            {
                DoDownload(this, e);
            }
        }


        /// <summary>
        /// 引发下载进度变化事件的方法
        /// </summary>
        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }


        /// <summary>
        /// 引发下载结束事件的方法
        /// </summary>
        protected virtual void OnDoDownloadCompleted(RunWorkerCompletedEventArgs e)
        {
            if (DoDownloadrCompleted != null)
            {
                DoDownloadrCompleted(this, e);
            }
        }


        /// <summary>
        /// 开始下载时调用，触发DoDownload事件
        /// </summary>
        /// <param name="argument"></param>
        private void DownloadThreadStart(object argument)
        {
            object result = null;
            bool cancelled = false;
            Exception error = null;
            try
            {
                DoWorkEventArgs e = new DoWorkEventArgs(argument);
                this.OnDoDownload(e);
                if (e.Cancel)
                {
                    cancelled = true;
                }
                else
                {
                    result = e.Result;
                }
            }
            catch (Exception exception2)
            {
                error = exception2;
            }
            RunWorkerCompletedEventArgs arg = new RunWorkerCompletedEventArgs(result, error, cancelled);

            DownloadMusicTask item = argument as DownloadMusicTask;
            this.asyncOperationtTable[item.DownloadGUID].PostOperationCompleted(this.operationCompleted, arg);

            //从下载队列中移除已经下载完的文件,
            lock(downloadLock)
            {
                downloadTable.Remove(item.DownloadGUID);
            }

            // 从同步管理表中删除
            lock(asyncLock)
            {
                asyncOperationtTable.Remove(item.DownloadGUID);
            }

            // 寻找等待队列中等待的任务，移动到下载列表
            lock (waitLock)
            {
                if (waitDownloadList.Count > 0 && downloadTable.Count <5 )
                {
                    foreach (DownloadMusicTask task in waitDownloadList)
                    {
                        if (task.DownloadStatus == DownloadStatus.ST_WAIT_DOWNLOAD)
                        {
                            DownloadAsync(task);
                            waitDownloadList.Remove(task);
                            break;
                        }
                    }
                    
                }
            }
        }


        /// <summary>
        /// 下载进行时调用，触发ProgressChanged事件
        /// </summary>
        /// <param name="arg"></param>
        private void ProgressReporter(object arg)
        {
            this.OnProgressChanged((ProgressChangedEventArgs)arg);
        }


        /// <summary>
        /// 下载完成时调用，触发DoDownloadrCompleted事件
        /// </summary>
        /// <param name="arg"></param>
        private void AsyncOperationCompleted(object arg)
        {
            this.OnDoDownloadCompleted((RunWorkerCompletedEventArgs)arg);
        }        

        #endregion


        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="argument">要下载的音乐信息</param>
        public void DownloadAsync(DownloadMusicTask downloadItem)
        {
            //如果目前下载数量小于5条，则通过委托启动新的下载任务
            if (downloadTable.Count < 5)
            {
                //设置下载任务,进行管理
                AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(downloadItem.DownloadGUID);

                lock (asyncLock)
                {
                    if (!asyncOperationtTable.ContainsKey(downloadItem.DownloadGUID))
                        asyncOperationtTable.Add(downloadItem.DownloadGUID, asyncOperation);
                }

                lock(downloadLock)
                {
                     downloadTable.Add(downloadItem.DownloadGUID, downloadItem); //加入到下载列表
                }
                //新启一个线程进行操作
                this.threadStart.BeginInvoke(downloadItem, null, null);
                
               
            }
            else  //如果目前下载数量大于5则进入等待下载队列
            {
                lock(waitLock)
                {
                    waitDownloadList.Add(downloadItem);
                }
                downloadItem.DownloadSize = 0;
                downloadItem.DownloadStatus = DownloadStatus.ST_WAIT_DOWNLOAD;
                ReportProgress(downloadItem); //报道下载进度
            }
        }


        /// <summary>
        /// 开始回报下载进度
        /// </summary>
        /// <param name="percentProgress"></param>
        /// <param name="userState"></param>
        public void ReportProgress(object userState)
        {
            //如果是暂停状态,把当前任务放入等待队列，而从等待队列中选一个开始下载
            DownloadMusicTask item = userState as DownloadMusicTask;
            if(item.DownloadStatus == DownloadStatus.ST_STOP_DOWNLOAD)
            {
                // 放入等待队列
                lock (downloadLock)
                {
                    downloadTable.Remove(item.DownloadGUID);
                }

                // 寻找等待队列中等待的任务，移动到下载列表
                lock (waitLock)
                {
                    if (waitDownloadList.Count > 0 && downloadTable.Count < 5)
                    {
                        foreach (DownloadMusicTask task in waitDownloadList)
                        {
                            if (task.DownloadStatus == DownloadStatus.ST_WAIT_DOWNLOAD)
                            {
                                DownloadAsync(task);
                                waitDownloadList.Remove(task);
                                break;
                            }
                        }

                    }
                }
            }

            //通知UI，进入变化
            ProgressChangedEventArgs arg = new ProgressChangedEventArgs(0, userState);
            if (this.asyncOperationtTable[item.DownloadGUID] != null)
            {
                this.asyncOperationtTable[item.DownloadGUID].Post(this.progressReporter, arg);
            }
            else
            {
                this.progressReporter(arg); //不对
            }
        }


    }
}
