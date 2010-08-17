namespace CMusicSearch.Test
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.musicPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.button2 = new System.Windows.Forms.Button();
            this.labNetStatus = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.MusicName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MusicSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Progress = new CMusicSearch.Test.DataGridViewProgressBarColumn();
            this.Speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.URL = new System.Windows.Forms.DataGridViewLinkColumn();
            this.GUID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuDownload = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewProgressBarColumn1 = new CMusicSearch.Test.DataGridViewProgressBarColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.musicPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.contextMenuDownload.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(275, 22);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "搜索";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(465, 447);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.musicPlayer);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.labNetStatus);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 447);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(922, 72);
            this.panel1.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(494, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "播放";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // musicPlayer
            // 
            this.musicPlayer.Enabled = true;
            this.musicPlayer.Location = new System.Drawing.Point(450, 13);
            this.musicPlayer.Name = "musicPlayer";
            this.musicPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("musicPlayer.OcxState")));
            this.musicPlayer.Size = new System.Drawing.Size(38, 36);
            this.musicPlayer.TabIndex = 6;
            this.musicPlayer.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(728, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 25);
            this.button2.TabIndex = 5;
            this.button2.Text = "打开下载目录";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // labNetStatus
            // 
            this.labNetStatus.AutoSize = true;
            this.labNetStatus.Location = new System.Drawing.Point(845, 28);
            this.labNetStatus.Name = "labNetStatus";
            this.labNetStatus.Size = new System.Drawing.Size(35, 13);
            this.labNetStatus.TabIndex = 4;
            this.labNetStatus.Text = "label1";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(614, 21);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 25);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "下载";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(12, 24);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(242, 20);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox2.Font = new System.Drawing.Font("SimSun", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(3, 207);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(437, 211);
            this.textBox2.TabIndex = 4;
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(437, 198);
            this.dataGridView2.TabIndex = 6;
            this.dataGridView2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView2_MouseClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(471, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(451, 447);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView2);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(443, 421);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "歌词";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(443, 421);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "下载";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.AllowUserToResizeRows = false;
            this.dataGridView3.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MusicName,
            this.Size,
            this.MusicSize,
            this.Progress,
            this.Speed,
            this.URL,
            this.GUID,
            this.Task});
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView3.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridView3.Location = new System.Drawing.Point(3, 3);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersVisible = false;
            this.dataGridView3.RowTemplate.Height = 23;
            this.dataGridView3.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.Size = new System.Drawing.Size(437, 415);
            this.dataGridView3.TabIndex = 0;
            this.dataGridView3.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView3_CellMouseClick);
            // 
            // MusicName
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MusicName.DefaultCellStyle = dataGridViewCellStyle15;
            this.MusicName.HeaderText = "歌曲名称";
            this.MusicName.Name = "MusicName";
            this.MusicName.ReadOnly = true;
            this.MusicName.Width = 80;
            // 
            // Size
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Size.DefaultCellStyle = dataGridViewCellStyle16;
            this.Size.HeaderText = "文件大小";
            this.Size.Name = "Size";
            this.Size.Width = 80;
            // 
            // MusicSize
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MusicSize.DefaultCellStyle = dataGridViewCellStyle17;
            this.MusicSize.HeaderText = "下载状态";
            this.MusicSize.Name = "MusicSize";
            this.MusicSize.ReadOnly = true;
            this.MusicSize.Width = 80;
            // 
            // Progress
            // 
            this.Progress.HeaderText = "进度";
            this.Progress.Maximum = 100;
            this.Progress.Mimimum = 0;
            this.Progress.Name = "Progress";
            this.Progress.ReadOnly = true;
            // 
            // Speed
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Speed.DefaultCellStyle = dataGridViewCellStyle18;
            this.Speed.HeaderText = "下载速度";
            this.Speed.Name = "Speed";
            this.Speed.ReadOnly = true;
            this.Speed.Width = 80;
            // 
            // URL
            // 
            this.URL.HeaderText = "下载地址";
            this.URL.Name = "URL";
            this.URL.ReadOnly = true;
            this.URL.VisitedLinkColor = System.Drawing.Color.Red;
            // 
            // GUID
            // 
            this.GUID.HeaderText = "GUID";
            this.GUID.Name = "GUID";
            this.GUID.ReadOnly = true;
            this.GUID.Visible = false;
            // 
            // Task
            // 
            this.Task.HeaderText = "Task";
            this.Task.Name = "Task";
            this.Task.ReadOnly = true;
            this.Task.Visible = false;
            // 
            // contextMenuDownload
            // 
            this.contextMenuDownload.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.pauseAllToolStripMenuItem,
            this.deleteAllToolStripMenuItem});
            this.contextMenuDownload.Name = "contextMenuDownload";
            this.contextMenuDownload.Size = new System.Drawing.Size(123, 114);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Enabled = false;
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.startToolStripMenuItem.Text = "开始";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Enabled = false;
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.pauseToolStripMenuItem.Text = "暂停";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.deleteToolStripMenuItem.Text = "删除";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // pauseAllToolStripMenuItem
            // 
            this.pauseAllToolStripMenuItem.Enabled = false;
            this.pauseAllToolStripMenuItem.Name = "pauseAllToolStripMenuItem";
            this.pauseAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.pauseAllToolStripMenuItem.Text = "全部暂停";
            this.pauseAllToolStripMenuItem.Click += new System.EventHandler(this.pauseAllToolStripMenuItem_Click);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Enabled = false;
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.deleteAllToolStripMenuItem.Text = "全部删除";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "歌曲名称";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "文件大小";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewProgressBarColumn1
            // 
            this.dataGridViewProgressBarColumn1.HeaderText = "进度";
            this.dataGridViewProgressBarColumn1.Maximum = 100;
            this.dataGridViewProgressBarColumn1.Mimimum = 0;
            this.dataGridViewProgressBarColumn1.Name = "dataGridViewProgressBarColumn1";
            this.dataGridViewProgressBarColumn1.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 519);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "MusicSearchDemo";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.musicPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.contextMenuDownload.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label labNetStatus;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewProgressBarColumn dataGridViewProgressBarColumn1;
        private System.Windows.Forms.ContextMenuStrip contextMenuDownload;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn MusicName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn MusicSize;
        private DataGridViewProgressBarColumn Progress;
        private System.Windows.Forms.DataGridViewTextBoxColumn Speed;
        private System.Windows.Forms.DataGridViewLinkColumn URL;
        private System.Windows.Forms.DataGridViewTextBoxColumn GUID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Task;
        private AxWMPLib.AxWindowsMediaPlayer musicPlayer;
        private System.Windows.Forms.Button button3;
    }
}

