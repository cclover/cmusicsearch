using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CMusicSearch.MusicCommon;

namespace CMusicSearch.MusicDownload
{
    public class FileManager
    {
        const string DOWNLOAD_DIR = @"D:\MusicDownload";
        const string TMP_FILE_EXT = "mus";
        const string CONFIG_FILE_EXT = "cfg";


        /// <summary>
        /// 返回下载文件的路径（临时文件）
        /// </summary>
        /// <param name="musicName">下载的文件名</param>
        /// <param name="musicFormat">下载文件格式</param>
        /// <returns>下载路径</returns>
        public static string GetSavaPath(string musicName, string musicFormat)
        {
            //创建下载目录
            if (!Directory.Exists(DOWNLOAD_DIR))
            {
                try
                {
                    Directory.CreateDirectory(DOWNLOAD_DIR);
                }
                catch
                {
                    return null;
                }
            }

            int i = 1;
            string name = musicName;
            //判断要下载的文件是否存在同名的音乐文件或是mus文件
            while (File.Exists(string.Format("{0}\\{1}.{2}", DOWNLOAD_DIR, musicName, musicFormat))
                || File.Exists(string.Format("{0}\\{1}.{2}.{3}", DOWNLOAD_DIR, musicName, musicFormat,TMP_FILE_EXT)))
            {
                musicName = string.Format("{0}({1})", name, i); //存在同名则更改名字
                i++;
            }

            //返回下载的路径
            return string.Format("{0}\\{1}.{2}.{3}", DOWNLOAD_DIR, musicName, musicFormat, TMP_FILE_EXT);
        }


        /// <summary>
        /// 下载结束时操作文件
        /// </summary>
        /// <param name="savePath">下载文件路径</param>
        /// <param name="configPath">配置文件路径</param>
        /// <param name="status">下载状态</param>
        public static void FileDownloadOver(string savePath, string configPath, DownloadStatus status)
        {
            //下载完，删除配置文件，并修改下载文件名
            if (status == DownloadStatus.ST_OVER_DOWNLOAD)
            {
                if (File.Exists(savePath))
                {
                    File.Move(savePath, savePath.Remove(savePath.LastIndexOf(CommonSymbol.SYMBOL_DOT))); //重命名
                }
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
            }
            else if(status == DownloadStatus.ST_ERROR_DOWNLOAD) //如果下载错误,并且文件为0，则删除文件和配置文件
            {
                if (File.Exists(savePath)) 
                {
                    FileInfo file = new FileInfo(savePath);
                    if (file.Length == 0)
                    {
                        file.Delete();
                        if (File.Exists(configPath))
                        {
                            File.Delete(configPath);
                        }
                    }

                }

            }
            else if (status == DownloadStatus.ST_CANCEL_DOWNLOAD) //如果取消下载，删除文件和配置文件
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }
                if (File.Exists(configPath))
                {
                    File.Delete(configPath);
                }
            }
        }
    }
}
