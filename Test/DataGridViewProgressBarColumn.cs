using System;
using System.Drawing;
using System.Windows.Forms;


namespace CMusicSearch.Test
{
    /// <summary>
    /// DataGridViewProgressBarCell对象的列
    /// </summary>
    public class DataGridViewProgressBarColumn : DataGridViewTextBoxColumn
    {
        /// <summary>
        /// Constructer
        /// </summary>
        public DataGridViewProgressBarColumn()
        {
            this.CellTemplate = new DataGridViewProgressBarCell();
        }

        /// <summary>
        /// CellTemplate的取得和设定
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {

                if (!(value is DataGridViewProgressBarCell))
                {
                    throw new InvalidCastException(
                        "请指定DataGridViewProgressBarCell对象。");
                }
                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// ProgressBar的最大值
        /// </summary>
        public int Maximum
        {
            get
            {
                return ((DataGridViewProgressBarCell)this.CellTemplate).Maximum;
            }
            set
            {
                if (this.Maximum == value)
                    return;
                //变更单元格模板的值
                ((DataGridViewProgressBarCell)this.CellTemplate).Maximum =
                    value;
                //变更已经向DataGridView单元格追加的值
                if (this.DataGridView == null)
                    return;
                int rowCount = this.DataGridView.RowCount;
                for (int i = 0; i < rowCount; i++)
                {
                    DataGridViewRow r = this.DataGridView.Rows.SharedRow(i);
                    ((DataGridViewProgressBarCell)r.Cells[this.Index]).Maximum =
                        value;
                }
            }
        }

        /// <summary>
        /// ProgressBar的最小值
        /// </summary>
        public int Mimimum
        {
            get
            {
                return ((DataGridViewProgressBarCell)this.CellTemplate).Mimimum;
            }
            set
            {
                if (this.Mimimum == value)
                    return;
                //变更单元格模板的值
                ((DataGridViewProgressBarCell)this.CellTemplate).Mimimum =
                    value;
                //变更已经向DataGridView单元格追加的值
                if (this.DataGridView == null)
                    return;
                int rowCount = this.DataGridView.RowCount;
                for (int i = 0; i < rowCount; i++)
                {
                    DataGridViewRow r = this.DataGridView.Rows.SharedRow(i);
                    ((DataGridViewProgressBarCell)r.Cells[this.Index]).Mimimum =
                        value;
                }
            }
        }
    }

    /// <summary>
    /// 在DataGridView中表示ProgressBar
    /// </summary>
    public class DataGridViewProgressBarCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// Constructer
        /// </summary>
        public DataGridViewProgressBarCell()
        {
            this.Maximum = 100;
            this.Mimimum = 0;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public int Maximum
        {
            get;
            set;
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public int Mimimum
        {
            get;
            set;
        }

        /// <summary>
        /// 指定单元格的值的数据类型,在这里指定为整数
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(int);
            }
        }

        /// <summary>
        /// 指定新行单元格的值
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                return 0;
            }
        }


        /// <summary>
        /// 为了追加新属性，必需重载Clone方法
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DataGridViewProgressBarCell cell = (DataGridViewProgressBarCell)base.Clone();
            cell.Maximum = this.Maximum;
            cell.Mimimum = this.Mimimum;
            return cell;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                                      DataGridViewElementStates cellState, object value, object formattedValue,
                                      string errorText, DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {

            //// 缓冲区上下文
            //BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            //// 自定义缓冲区
            //BufferedGraphics myBuffer = currentContext.Allocate(graphics, cellBounds);
            //// 绘图graphics
            //Graphics g = myBuffer.Graphics;
            //g.Clear(Color.Black);
            ////g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;   // 反锯齿

            //设定值
            int intValue = 0;
            if (value is int)
                intValue = (int)value;
            if (intValue < this.Mimimum)
                intValue = this.Mimimum;
            if (intValue > this.Maximum)
                intValue = this.Maximum;
            //除法计算
            double rate = (double)(intValue - this.Mimimum) / (this.Maximum - this.Mimimum);

            //描绘单元格的边框
            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            {
                this.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            }

            //取得边框的范围
            Rectangle borderRect = this.BorderWidths(advancedBorderStyle);
            Rectangle paintRect = new Rectangle(
                cellBounds.Left + borderRect.Left,
                cellBounds.Top + borderRect.Top,
                cellBounds.Width - borderRect.Right,
                cellBounds.Height - borderRect.Bottom);

            //设定背景色
            //被选择时和没有被选择时，颜色变换
            bool isSelected = (cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected;
            Color bkColor;
            if (isSelected &&
                (paintParts & DataGridViewPaintParts.SelectionBackground) == DataGridViewPaintParts.SelectionBackground)
            {
                bkColor = cellStyle.SelectionBackColor;
            }
            else
            {
                bkColor = cellStyle.BackColor;
            }
            //描绘背景
            if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            {
                using (SolidBrush backBrush = new SolidBrush(bkColor))
                {
                    graphics.FillRectangle(backBrush, paintRect);
                }
            }

            //减去Padding
            paintRect.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
            paintRect.Width -= cellStyle.Padding.Horizontal;
            paintRect.Height -= cellStyle.Padding.Vertical;

            //描绘ProgressBar
            if ((paintParts & DataGridViewPaintParts.ContentForeground) == DataGridViewPaintParts.ContentForeground)
            {
                if (ProgressBarRenderer.IsSupported)
                {
                    //使用VisualStyle描绘
                    //描绘ProgressBar的框架
                    ProgressBarRenderer.DrawHorizontalBar(graphics, paintRect);
                    //描绘ProgressBar的进度条
                    Rectangle barBounds = new Rectangle(
                        paintRect.Left + 3, paintRect.Top + 3,
                        paintRect.Width - 4, paintRect.Height - 6);
                    barBounds.Width = (int)Math.Round(barBounds.Width * rate);
                    ProgressBarRenderer.DrawHorizontalChunks(graphics, barBounds);
                }
                else
                {
                    //不能使用VisualStyle描绘时
                    graphics.FillRectangle(Brushes.White, paintRect);
                    graphics.DrawRectangle(Pens.Black, paintRect);
                    Rectangle barBounds = new Rectangle(
                        paintRect.Left + 1, paintRect.Top + 1,
                        paintRect.Width - 1, paintRect.Height - 1);
                    barBounds.Width = (int)Math.Round(barBounds.Width * rate);
                    graphics.FillRectangle(Brushes.Blue, barBounds);
                }
            }

            //表示光标移动的框
            if (this.DataGridView.CurrentCellAddress.X == this.ColumnIndex &&
                this.DataGridView.CurrentCellAddress.Y == this.RowIndex &&
                (paintParts & DataGridViewPaintParts.Focus) == DataGridViewPaintParts.Focus &&
                this.DataGridView.Focused)
            {
                //设定大小适当的框
                Rectangle focusRect = paintRect;
                focusRect.Inflate(-3, -3);
                ControlPaint.DrawFocusRectangle(graphics, focusRect);
                //指定背景色描绘移动框时
                //ControlPaint.DrawFocusRectangle(
                //    graphics, focusRect, Color.Empty, bkColor);
            }

            //表示文字列
            if ((paintParts & DataGridViewPaintParts.ContentForeground) ==
                DataGridViewPaintParts.ContentForeground)
            {
                //设定表示的文字列
                string txt = string.Format("{0}%", Math.Round(rate * 100));
                //string txt = formattedValue.ToString();

                TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping;
                //设定颜色
                Color fColor = cellStyle.ForeColor;
                //if (isSelected)
                //    fColor = cellStyle.SelectionForeColor;
                //else
                //    fColor = cellStyle.ForeColor;
                //描绘文字列
                paintRect.Inflate(-2, -2);
                TextRenderer.DrawText(graphics, txt, cellStyle.Font,paintRect, fColor, flags);
                //g.DrawString(txt, cellStyle.Font, Brushes.Black, paintRect);
            }

            //表示错误图标
            if ((paintParts & DataGridViewPaintParts.ErrorIcon) ==
                DataGridViewPaintParts.ErrorIcon &&
                this.DataGridView.ShowCellErrors &&
                !string.IsNullOrEmpty(errorText))
            {
                //取得错误图标表示的位置
                Rectangle iconBounds = this.GetErrorIconBounds(
                    graphics, cellStyle, rowIndex);
                iconBounds.Offset(cellBounds.X, cellBounds.Y);
                //描绘错误图标
                this.PaintErrorIcon(graphics, iconBounds, cellBounds, errorText);
            }

            //// 自定义缓冲中的图形渲染在屏幕上
            //myBuffer.Render();
            //// 释放资源
            //myBuffer.Dispose();

            //g.Dispose();

        }
    }
}