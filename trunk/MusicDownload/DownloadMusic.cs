using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using CMusicSearch.MusicCommon;
using CMusicSearch.MusicCommon.Entity;

namespace CMusicSearch.MusicDownload
{
    /// <summary>
    /// 下载音乐文件
    /// </summary>
    class DownloadMusic
    {
        private Queue<DownloadMusicInfo> waitDownloadQueue;
        private Queue<DownloadMusicInfo> downloadQueue;
        private AsyncOperation asyncOperation;


        //定义开始，进行，结束相关的委托对象
        //定义这些委托和相关事件的触发方法相绑定是为了工作线程和UI线程间通信
        private delegate void WorkerThreadStartDelegate(object argument);
        private readonly WorkerThreadStartDelegate threadStart;
        private readonly SendOrPostCallback progressReporter;
        private readonly SendOrPostCallback progressWait;
        private readonly SendOrPostCallback operationCompleted;


        /// <summary>
        /// 构造函数
        /// </summary>
        public DownloadMusic()
        {
            waitDownloadQueue = new Queue<DownloadMusicInfo>();
            downloadQueue = new Queue<DownloadMusicInfo>();

            //绑定委托事件
            this.threadStart = new WorkerThreadStartDelegate(this.DownloadThreadStart);
            this.progressReporter = new SendOrPostCallback(this.ProgressReporter);
            this.operationCompleted = new SendOrPostCallback(this.AsyncOperationCompleted);
            this.progressWait = new SendOrPostCallback(this.progressWait);
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
        /// 等待下载事件
        /// </summary>
        public event ProgressChangedEventHandler ProgressWaitting;

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
        /// 引发下载暂停事件的方法
        /// </summary>
        protected virtual void OnProgressWait(ProgressChangedEventArgs e)
        {
            if (ProgressWaitting != null)
            {
                ProgressWaitting(this, e);
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
            this.asyncOperation.PostOperationCompleted(this.operationCompleted, arg);
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
        /// 等待下载时调用，触发ProgressChanged事件
        /// </summary>
        /// <param name="arg"></param>
        private void ProgressWait(object arg)
        {
            this.OnProgressWait((ProgressChangedEventArgs)arg);
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
        public void DownloadAsync(DownloadMusicInfo downloadItem)
        {
            //如果目前下载数量小于5条，则通过委托启动新的下载任务
            if (downloadQueue.Count < 5)
            {
                //设置下载任务的GUID，进行管理
                downloadItem.DownloadGUID = Guid.NewGuid();
                this.asyncOperation = AsyncOperationManager.CreateOperation(downloadItem.DownloadGUID);

                //新启一个线程进行操作
                this.threadStart.BeginInvoke(downloadItem, null, null); 
            }
            else  //如果目前下载数量大于5则进入等待下载队列
            {
                waitDownloadQueue.Enqueue(downloadItem);
                ProgressChangedEventArgs arg = new ProgressChangedEventArgs(0, DownloadStatus.DOWNLOAD_ST_WAIT);
                if (this.asyncOperation != null)
                {
                    this.asyncOperation.Post(this.progressWait, arg);
                }
                else
                {
                    this.progressReporter(arg);
                }
            }
        }


        /// <summary>
        /// 开始回报下载进度
        /// </summary>
        /// <param name="percentProgress"></param>
        /// <param name="userState"></param>
        public void ReportProgress(int percentProgress, object userState)
        {
            ProgressChangedEventArgs arg = new ProgressChangedEventArgs(percentProgress, userState);
            if (this.asyncOperation != null)
            {
                this.asyncOperation.Post(this.progressReporter, arg);
            }
            else
            {
                this.progressReporter(arg);
            }
        }

    }
}
