﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CMusicSearch.MusicCommon;


namespace CMusicSearch.MusicDownload
{
    /// <summary>
    /// 负责管理下载任务的调度，不负责具体的文件下载。
    /// 基于事件驱动模型的多任务多线程后台下载管理器。功能类似BackGroundWork控件，支持多任务，UI操作最终被封送到UI线程。
    /// </summary>
    public class DownloadManagement
    {

        #region 私有变量
        /// <summary>
        /// 等待下载的任务列表
        /// </summary>
        private Dictionary<Guid, DownloadMusicTask> waitDownloadTable;

        /// <summary>
        /// 下载中的任务列表
        /// </summary>
        private Dictionary<Guid, DownloadMusicTask> downloadTable;

        /// <summary>
        /// 管理任务同步的列表
        /// </summary>
        private Dictionary<Guid, AsyncOperation> asyncOperationtTable;

        //同步锁
        private object waitTableLock;
        private object downloadTableLock;
        private object asyncOperationLock;


        //定义开始，进行，结束相关的委托对象
        //定义这些委托和相关事件的触发方法相绑定是为了工作线程和UI线程间通信
        private delegate void WorkerThreadStartDelegate(object argument);
        private readonly WorkerThreadStartDelegate threadStart;
        private readonly SendOrPostCallback progressReporter;
        private readonly SendOrPostCallback operationCompleted;

        #endregion


        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DownloadManagement()
        {
            //初始化管理下载的数据结构
            waitDownloadTable = new Dictionary<Guid, DownloadMusicTask>();
            downloadTable = new Dictionary<Guid, DownloadMusicTask>();
            asyncOperationtTable = new Dictionary<Guid, AsyncOperation>();

            //绑定委托事件
            this.threadStart = new WorkerThreadStartDelegate(this.WorkerThreadStart);
            this.progressReporter = new SendOrPostCallback(this.ProgressReporter);
            this.operationCompleted = new SendOrPostCallback(this.AsyncOperationCompleted);

            //初始化锁
            waitTableLock = new object();
            downloadTableLock = new object();
            asyncOperationLock = new object();
        }
        #endregion


        #region 下载事件处理

        //使用自己实现事件
        //private static readonly object doWorkKey;
        //private static readonly object progressChangedKey;
        //private static readonly object runWorkerCompletedKey;

        /// <summary>
        /// 开始下载事件
        /// </summary>
        public event DoWorkEventHandler DoWork;


        /// <summary>
        /// 下载中事件
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;


        /// <summary>
        /// 下载完成事件
        /// </summary>
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;


        /// <summary>
        /// 引发开始下载事件的方法
        /// </summary>
        protected virtual void OnDoWork(DoWorkEventArgs e)
        {
            if (DoWork != null)
            {
                DoWork(this, e);
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
        protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            if (RunWorkerCompleted != null)
            {
                RunWorkerCompleted(this, e);
            }
        }


        /// <summary>
        /// 开始下载时调用，触发DoWork事件
        /// </summary>
        /// <param name="argument"></param>
        private void WorkerThreadStart(object argument)
        {
            //执行DoWork委托绑定的事件
            try
            {
                DoWorkEventArgs e = new DoWorkEventArgs(argument);
                this.OnDoWork(e);
            }
            catch
            {
                //因为OnDoWork是FileDownload方法，里面已经处理了异常
                //所以这里实际捕获不到下载引发的异常
            }

            //执行完获得下载任务的实体类
            Guid taskID = (argument as DownloadMusicTask).DownloadTaskID;
            DownloadMusicTask item = downloadTable[taskID];

            //如果是暂停或错误状态,把当前任务放入等待队列，而从等待队列中选一个开始下载
            if (item.DownloadStatus == DownloadStatus.ST_STOP_DOWNLOAD || item.DownloadStatus == DownloadStatus.ST_ERROR_DOWNLOAD)
            {
                // 从下载队列删除
                lock (downloadTableLock)
                {
                    downloadTable.Remove(item.DownloadTaskID);
                }

                // 加入到等待队列，并寻找等待队列中等待的任务，移动到下载列表
                lock (waitTableLock)
                {
                    waitDownloadTable.Add(item.DownloadTaskID, item);
                    if (waitDownloadTable.Count > 0 && downloadTable.Count < 5)
                    {
                        foreach (DownloadMusicTask task in waitDownloadTable.Values)
                        {
                            if (task.DownloadStatus == DownloadStatus.ST_WAIT_DOWNLOAD)
                            {
                                RunWorkerAsync(task); //开始等待的任务
                                waitDownloadTable.Remove(task.DownloadTaskID); //把这个任务从等待列表移除
                                break;
                            }
                        }
                    }
                }
                return;
            }

            // 触发RunWorkerCompleted事件,发送到UI线程操作(cancle设为false，true会抛出异常，item.Cancle可以查看)
            RunWorkerCompletedEventArgs arg = new RunWorkerCompletedEventArgs(item, item.Error, false);
            if (asyncOperationtTable[item.DownloadTaskID] != null)
            {
                asyncOperationtTable[item.DownloadTaskID].PostOperationCompleted(this.operationCompleted, arg);
            }

            // 从同步管理表中删除已经完成（或取消）下载的任务
            lock (asyncOperationLock)
            {
                asyncOperationtTable[item.DownloadTaskID] = null;
                asyncOperationtTable.Remove(item.DownloadTaskID);
            }

            //从下载队列中移除已经下载完（或取消）的文件,
            lock (downloadTableLock)
            {
                downloadTable.Remove(item.DownloadTaskID);
            }

            // 寻找等待队列中等待的任务，移动到下载列表
            if (waitDownloadTable.Count > 0 && downloadTable.Count < 5)
            {
                foreach (DownloadMusicTask waitTask in waitDownloadTable.Values)
                {
                    if (waitTask.DownloadStatus == DownloadStatus.ST_WAIT_DOWNLOAD)
                    {
                        RunWorkerAsync(waitTask); //开始等待的任务
                        lock (waitTableLock)
                        {
                            waitDownloadTable.Remove(waitTask.DownloadTaskID); //把这个任务从等待列表移除
                        }
                        break;
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
        /// 下载完成时调用，触发RunWorkerCompleted事件
        /// </summary>
        /// <param name="arg"></param>
        private void AsyncOperationCompleted(object arg)
        {
            this.OnRunWorkerCompleted((RunWorkerCompletedEventArgs)arg);
        }

        #endregion


        #region 外部调用方法

        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="argument">要下载的音乐信息</param>
        public void RunWorkerAsync(DownloadMusicTask downloadItem)
        {
            //如果目前下载数量小于5条，则通过委托启动新的下载任务

            //设置下载状态
            downloadItem.IsStop = false;
            downloadItem.DownloadStatus = DownloadStatus.ST_READY_DOWNLOAD;

            // 对于新增的任务，添加到同步管理表
            if (!asyncOperationtTable.ContainsKey(downloadItem.DownloadTaskID))
            {
                AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(downloadItem.DownloadTaskID);
                lock (asyncOperationLock)
                {
                    asyncOperationtTable.Add(downloadItem.DownloadTaskID, asyncOperation);
                }
            }

            if (downloadTable.Count < 5)
            {
                // 如果开始的是等待任务，则从等待列表删除
                if (waitDownloadTable.ContainsKey(downloadItem.DownloadTaskID))
                {
                    lock (waitTableLock)
                    {
                        waitDownloadTable.Remove(downloadItem.DownloadTaskID);
                    }
                }

                //加入到下载列表
                lock (downloadTableLock)
                {
                    downloadTable.Add(downloadItem.DownloadTaskID, downloadItem);
                }

                //新启一个线程开始下载
                this.threadStart.BeginInvoke(downloadItem, null, null);
            }
            else  //如果目前下载数量大于5则进入等待下载队列
            {
                //设置下载状态
                downloadItem.IsStop = true;
                downloadItem.DownloadStatus = DownloadStatus.ST_WAIT_DOWNLOAD;

                //添加到等待列表
                lock (waitTableLock)
                {
                    waitDownloadTable.Add(downloadItem.DownloadTaskID, downloadItem);
                }

                ReportProgress(downloadItem); //报道下载进度
            }
        }


        /// <summary>
        /// 开始回报下载进度
        /// </summary>
        /// <param name="userState">下载任务信息</param>
        public void ReportProgress(object taskState)
        {
            DownloadMusicTask item = taskState as DownloadMusicTask;
            //通知UI,进度和状态发生变化（进度百分比客户端从taskState取得自己计算，这里返回0）
            ProgressChangedEventArgs arg = new ProgressChangedEventArgs(0, taskState);
            if (this.asyncOperationtTable[item.DownloadTaskID] != null)
            {
                this.asyncOperationtTable[item.DownloadTaskID].Post(this.progressReporter, arg);
            }
            else
            {
                //this.progressReporter(arg); //非UI线程修改UI控件会报错
            }
        }


        /// <summary>
        /// 检查任务是否被停止
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>是否停止</returns>
        internal bool TaskCanStop(Guid taskID)
        {
            //如果在下载队列中，检查是否停止
            if (downloadTable.ContainsKey(taskID))
                return downloadTable[taskID].IsStop;
            else //如果不在下载队列中，表示已经停止
                return true;
        }


        /// <summary>
        /// 暂停正在进行的任务
        /// </summary>
        /// <param name="taskID">任务ID</param>
        public void StopAsync(Guid taskID)
        {
            // 设置下载的任务为停止
            if (downloadTable.ContainsKey(taskID))
                downloadTable[taskID].IsStop = true;
        }


        /// <summary>
        /// 暂停所有正在下载的任务
        /// </summary>
        public void StopAll()
        {
            foreach (DownloadMusicTask task in downloadTable.Values)
            {
                task.IsStop = true;  //设置所有下载任务为停止
            }
        }


        /// <summary>
        /// 检查任务是否被取消
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns>是否取消</returns>
        internal bool TaskCanCancle(Guid taskID)
        {
            //如果在下载队列中，检查是否取消
            if (downloadTable.ContainsKey(taskID))
                return downloadTable[taskID].IsCancle;
            else //如果不在下载队列中，表示已经停止
                return true;
        }


        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="taskID">任务ID</param>
        public void CancleAsync(Guid taskID)
        {
            //如果正在下载，设置为取消状态
            if (downloadTable.ContainsKey(taskID))
            {
                downloadTable[taskID].IsCancle = true;
            }
            else //如果没有在下载
            {
                // 获得要取消的任务对象
                DownloadMusicTask cancelTask = null;
                foreach (DownloadMusicTask waitTask in waitDownloadTable.Values)
                {
                    if (waitTask.DownloadTaskID == taskID)
                    {
                        cancelTask = waitTask;
                        break;
                    }
                }

                // 触发RunWorkerCompleted事件,发送到UI线程操作
                if (cancelTask != null)
                {
                    cancelTask.DownloadStatus = DownloadStatus.ST_CANCEL_DOWNLOAD; //状态设置为取消
                    RunWorkerCompletedEventArgs arg = new RunWorkerCompletedEventArgs(cancelTask, null, false);
                    if (asyncOperationtTable[taskID] != null)
                    {
                        asyncOperationtTable[taskID].PostOperationCompleted(this.operationCompleted, arg);
                    }
                }

                // 从同步管理表中删除
                lock (asyncOperationLock)
                {
                    asyncOperationtTable[taskID] = null;
                    asyncOperationtTable.Remove(taskID);
                }

                //如果存在则从等待队列中删除
                if (cancelTask != null)
                {
                    lock (waitTableLock)
                    {
                        waitDownloadTable.Remove(cancelTask.DownloadTaskID);
                    }
                }
            }
        }


        /// <summary>
        /// 取消所有下载
        /// </summary>
        public void CancleAll()
        {
            //设置所有下载任务为取消
            foreach (DownloadMusicTask task in downloadTable.Values)
            {
                task.IsCancle = true;
            }

            //删除等待任务的同步对象
            foreach (DownloadMusicTask waitTask in waitDownloadTable.Values)
            {
                //通知客户端
                waitTask.DownloadStatus = DownloadStatus.ST_CANCEL_DOWNLOAD; //状态设置为取消
                RunWorkerCompletedEventArgs arg = new RunWorkerCompletedEventArgs(waitTask, null, false);
                if (asyncOperationtTable[waitTask.DownloadTaskID] != null)
                {
                    asyncOperationtTable[waitTask.DownloadTaskID].PostOperationCompleted(this.operationCompleted, arg);
                }

                //从同步管理表中删除
                asyncOperationtTable[waitTask.DownloadTaskID] = null;
                lock (asyncOperationLock)
                {
                    asyncOperationtTable.Remove(waitTask.DownloadTaskID);
                }
            }

            //删除所有在等待的任务
            waitDownloadTable.Clear();
        }


        #endregion

    }

}
