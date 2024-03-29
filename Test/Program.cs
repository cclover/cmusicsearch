﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CMusicSearch.Test
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!CMusicSearch.MusicRunner.MSLRCRunner.Initialize())
            {
                MessageBox.Show("没有音乐插件!");
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
