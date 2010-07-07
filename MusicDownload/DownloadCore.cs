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
    class DownloadCore
    {
        private DownloadManagement downloadManager;

        public DownloadCore(DownloadManagement downloadManager)
        {
            this.downloadManager = downloadManager;
        }

        /// <summary>
        /// 下载指定路径的文件
        /// </summary>
        /// <param name="downloadItem">下载的文件信息</param>
        /// <returns>请求结果</returns>
        public PageRequestResults FileDownload(DownloadMusicTask downloadItem)
        {
            if (!string.IsNullOrEmpty(downloadItem.DownloadUrl))
            {
                // 设置请求信息，使用GET方式获得数据
                HttpWebRequest musicFileReq = (HttpWebRequest)WebRequest.Create(downloadItem.DownloadUrl);
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

                // 判断是否断点续传
                FileStream fs;
                if (!File.Exists(downloadItem.MusicSavePath))
                {
                    //不是断点续传时，创建文件
                    fs = new FileStream(downloadItem.MusicSavePath, FileMode.Create, FileAccess.Write);
                    downloadItem.DownloadSize = 0;
                }
                else
                {
                    //断点续传时读入文件，并且从已下载的位置开始下载
                    fs = System.IO.File.OpenWrite(downloadItem.MusicSavePath);
                    downloadItem.DownloadSize = fs.Length;
                    fs.Seek(downloadItem.DownloadSize, SeekOrigin.Current); //移动文件流中的当前指针
                    musicFileReq.AddRange((int)downloadItem.DownloadSize);  //设置请求头的Range值 
                }

                try
                {
                    // 获取页面响应
                    using (HttpWebResponse musicFileRes = (HttpWebResponse)musicFileReq.GetResponse())
                    {
                        // 如果HTTP为200，获取相应的文件流
                        if (musicFileRes.StatusCode == HttpStatusCode.OK)
                        {
                            // 获取响应的页面流
                            using (Stream remoteStream = musicFileRes.GetResponseStream())
                            {
                                using (fs)
                                {
                                    //设置下载状态和下载大小
                                    downloadItem.DownloadStatus = DownloadStatus.ST_READY_DOWNLOAD;
                                    downloadItem.FileSize = musicFileRes.ContentLength;

                                    //
                                    while (downloadItem.DownloadSize < downloadItem.FileSize)
                                    {
                                        // 检查是否被取消
                                        if (downloadManager.TaskCanStop(downloadItem.DownloadTaskID))
                                        {
                                            break;
                                        }

                                        // 检查是否被暂停
                                        if (downloadManager.TaskCanStop(downloadItem.DownloadTaskID))
                                        {
                                            downloadItem.DownloadStatus = DownloadStatus.ST_STOP_DOWNLOAD;
                                            downloadManager.ReportProgress(downloadItem); //汇报暂停时下载进度 
                                            break;
                                        }

                                        
                                        

                                        //从流中读取到文件流中
                                        byte[] buffer = new byte[1024];
                                        TimeSpan readStart = new TimeSpan(DateTime.Now.Ticks);
                                        int readSize = remoteStream.Read(buffer, 0, buffer.Length);
                                        TimeSpan readEnd = new TimeSpan(DateTime.Now.Ticks);
                                        TimeSpan ts = readEnd.Subtract(readStart).Duration();
                                        fs.Write(buffer, 0, buffer.Length);
                                        downloadItem.DownloadSize += readSize;
                                        downloadItem.DownloadSpeed = ts.Milliseconds;
                                        downloadItem.DownloadStatus = DownloadStatus.ST_IS_DOWNLOAD;
                                        downloadManager.ReportProgress(downloadItem);  //汇报当前下载进度
                                    }

                                    //完成下载，程序退出
                                    return PageRequestResults.Success;
                                }
                            }
                        }
                        else
                        {
                            //汇报下载进度
                            downloadItem.DownloadStatus = DownloadStatus.ST_ERROR_DOWNLOAD;
                            downloadManager.ReportProgress(downloadItem);
                            return PageRequestResults.UnknowException;
                        }
                    }
                }
                catch (WebException webEx)
                {
                    //汇报下载进度
                    downloadItem.DownloadStatus = DownloadStatus.ST_ERROR_DOWNLOAD;
                    downloadManager.ReportProgress(downloadItem);

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
                    else
                    {
                        return PageRequestResults.UnknowException;
                    }
                }
                catch
                {
                    //汇报下载进度
                    downloadItem.DownloadStatus = DownloadStatus.ST_ERROR_DOWNLOAD;
                    downloadManager.ReportProgress(downloadItem);
                    return PageRequestResults.UnknowException;
                }
            }
            return PageRequestResults.UrlIsNull;
        }
    }
}
