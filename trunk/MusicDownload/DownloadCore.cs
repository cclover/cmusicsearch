using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using CMusicSearch.MusicCommon;


namespace CMusicSearch.MusicDownload
{
    public class DownloadCore
    {
        /// <summary>
        /// 下载管理对象（检查暂停和取消下载用）
        /// </summary>
        private DownloadManagement downloadManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="downloadManager">下载管理对象</param>
        public DownloadCore(DownloadManagement downloadManager)
        {
            this.downloadManager = downloadManager;
        }

        /// <summary>
        /// 下载指定路径的文件
        /// </summary>
        /// <param name="downloadItem">下载任务信息</param>
        /// <returns>下载结果</returns>
        public PageRequestResults FileDownload(DownloadMusicTask downloadTask)
        {
            if (!string.IsNullOrEmpty(downloadTask.DownloadUrl))
            {
                // 设置请求信息，使用GET方式获得数据
                HttpWebRequest musicFileReq = (HttpWebRequest)WebRequest.Create(downloadTask.DownloadUrl);
                musicFileReq.AllowAutoRedirect = true;
                musicFileReq.Method = "GET";
                //设置超时时间
                musicFileReq.Timeout = SearchConfig.TIME_OUT;
                //获取代理
                IWebProxy proxy = SearchConfig.GetConfiguredWebProxy();
                //判断代理是否有效
                if (proxy != null)
                {
                    //代理有效时设置代理
                    musicFileReq.Proxy = proxy;
                }

                // 判断是否断点续传()
                FileStream downloadStream,configStream;
                if (!File.Exists(downloadTask.MusicSavePath))
                {
                    //不是断点续传时，创建下载文件和配置文件
                    downloadStream = new FileStream(downloadTask.MusicSavePath, FileMode.Create, FileAccess.Write);
                    //configStream = new FileStream(downloadTask.MusicConfigPath, FileMode.Create, FileAccess.Write);
                    downloadTask.DownloadSize = 0;
                }
                else
                {
                    //断点续传时读入文件，并且从已下载的位置开始下载
                    downloadStream = System.IO.File.OpenWrite(downloadTask.MusicSavePath);
                    downloadTask.DownloadSize = downloadStream.Length;
                    downloadStream.Seek(downloadTask.DownloadSize, SeekOrigin.Current); //移动文件流中的当前指针
                    musicFileReq.AddRange((int)downloadTask.DownloadSize);  //设置请求头的Range值 
                }

                try
                {
                    // 获取HTTP请求响应
                    using (HttpWebResponse musicFileRes = (HttpWebResponse)musicFileReq.GetResponse())
                    {
                        // 如果HTTP返回200--正常，返回206表示是断点续传
                        // 
                        if (musicFileRes.StatusCode == HttpStatusCode.OK
                            || musicFileRes.StatusCode == HttpStatusCode.PartialContent
                            || musicFileRes.StatusCode == HttpStatusCode.Moved
                            || musicFileRes.StatusCode == HttpStatusCode.MovedPermanently)
                        {
                            // 获取响应的页面流
                            using (Stream remoteStream = musicFileRes.GetResponseStream())
                            {
                                using (downloadStream)
                                {
                                    //设置下载状态和下载大小，状态为准备下载，并汇报进度
                                    downloadTask.DownloadStatus = DownloadStatus.ST_READY_DOWNLOAD;

                                    //此处要加上已下载的大小，因为断线续传时返回的ContentLength是从Range出到结束的大小
                                    downloadTask.FileSize = musicFileRes.ContentLength + downloadTask.DownloadSize;
                                    downloadManager.ReportProgress(downloadTask);  //汇报当前下载进度


                                    //开始下载数据，检查是否下载完成
                                    while (downloadTask.DownloadSize < downloadTask.FileSize)
                                    {
                                        // 检查是否被取消
                                        if (downloadManager.TaskCanCancle(downloadTask.DownloadTaskID))
                                        {
                                            //如果任务被取消退出
                                            downloadTask.DownloadStatus = DownloadStatus.ST_CANCEL_DOWNLOAD;
                                            break;
                                        }

                                        // 检查是否被暂停
                                        if (downloadManager.TaskCanStop(downloadTask.DownloadTaskID))
                                        {
                                            //汇报暂停时下载进度
                                            downloadTask.DownloadStatus = DownloadStatus.ST_STOP_DOWNLOAD;
                                            downloadManager.ReportProgress(downloadTask);
                                            break;
                                        }

                                        // 正常下载，从流中读取到文件流中
                                        byte[] buffer = new byte[1024];
                                        TimeSpan readStart = new TimeSpan(DateTime.Now.Ticks);
                                        int readSize = remoteStream.Read(buffer, 0, buffer.Length);
                                        TimeSpan readEnd = new TimeSpan(DateTime.Now.Ticks);
                                        TimeSpan ts = readEnd.Subtract(readStart).Duration();

                                        //写入文件
                                        downloadStream.Write(buffer, 0, readSize);
                                        downloadTask.DownloadSize += readSize;

                                        //计算速度
                                        if (ts.Milliseconds != 0)
                                            downloadTask.DownloadSpeed = (readSize / ts.Milliseconds) * 1000;
                                        else
                                            downloadTask.DownloadSpeed = 0;

                                        //汇报当前下载进度
                                        downloadTask.DownloadStatus = DownloadStatus.ST_IS_DOWNLOAD;
                                        downloadManager.ReportProgress(downloadTask);
                                    }

                                    // 如果完成，设置状态
                                    if (downloadTask.DownloadSize == downloadTask.FileSize)
                                    {
                                        downloadTask.DownloadStatus = DownloadStatus.ST_OVER_DOWNLOAD;
                                    }
                                    //完成下载，程序退出
                                    return PageRequestResults.Success;
                                }
                            }
                        }
                        else
                        {
                            //汇报下载进度
                            downloadTask.DownloadStatus = DownloadStatus.ST_ERROR_DOWNLOAD;
                            downloadManager.ReportProgress(downloadTask);
                            return PageRequestResults.UnknowException;
                        }
                    }
                }
                catch (WebException webEx)
                {
                    //出错时汇报下载进度和错误信息
                    downloadTask.Error = webEx;
                    downloadTask.DownloadStatus = DownloadStatus.ST_ERROR_DOWNLOAD;
                    downloadManager.ReportProgress(downloadTask);

                    // 异常时返回异常的原因
                    if (webEx.Status == WebExceptionStatus.Timeout)
                    {
                        return PageRequestResults.TimeOut;
                    }
                    else if (webEx.Status == WebExceptionStatus.SendFailure)
                    {
                        return PageRequestResults.SendFailure;
                    }
                    else if (webEx.Status == WebExceptionStatus.ConnectFailure)
                    {
                        return PageRequestResults.ConnectFailure;
                    }
                    else if (webEx.Status == WebExceptionStatus.ReceiveFailure)
                    {
                        return PageRequestResults.ReceiveFailure;
                    }
                    else if (webEx.Status == WebExceptionStatus.NameResolutionFailure)
                    {
                        return PageRequestResults.DNSFailure;
                    }
                    else if (webEx.Status == WebExceptionStatus.RequestProhibitedByProxy)
                    {
                        return PageRequestResults.ProxyFailure;
                    }
                    else
                    {
                        return PageRequestResults.UnknowException;
                    }
                }
                catch (Exception ex)
                {
                    //出错时汇报下载进度和错误信息
                    downloadTask.Error = ex;
                    downloadTask.DownloadStatus = DownloadStatus.ST_ERROR_DOWNLOAD;
                    downloadManager.ReportProgress(downloadTask);
                    return PageRequestResults.UnknowException;
                }
                finally
                {
                    downloadStream.Close();
                    //configStream.Close();
                }
            }
            return PageRequestResults.UrlIsNull;
        }
    }
}
