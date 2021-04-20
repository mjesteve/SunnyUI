﻿/******************************************************************************
 * SunnyUI 开源控件库、工具类库、扩展类库、多页面开发框架。
 * CopyRight (C) 2012-2021 ShenYongHua(沈永华).
 * QQ群：56829229 QQ：17612584 EMail：SunnyUI@QQ.Com
 *
 * Blog:   https://www.cnblogs.com/yhuse
 * Gitee:  https://gitee.com/yhuse/SunnyUI
 * GitHub: https://github.com/yhuse/SunnyUI
 *
 * SunnyUI.dll can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * 如果您使用此代码，请保留此说明。
 ******************************************************************************
 * 文件名称: UIDataGridViewFooter
 * 文件说明: DataGridView页脚，可做统计显示
 * 当前版本: V3.0
 * 创建日期: 2021-04-20
 *
 * 2021-04-20: V3.0.3 增加文件说明
******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sunny.UI
{
    [ToolboxItem(true)]
    public class UIDataGridViewFooter : UIControl
    {
        public UIDataGridViewFooter()
        {
            SetStyleFlags(true, false, true);
            Height = 35;
            RadiusSides = UICornerRadiusSides.None;
            RectSides = ToolStripStatusLabelBorderSides.None;
        }

        private UIDataGridView dgv;
        public UIDataGridView DataGridView
        {
            get => dgv;
            set
            {
                dgv = value;
                if (dgv != null)
                {
                    dgv.ColumnWidthChanged += Dgv_ColumnWidthChanged;
                    dgv.HorizontalScrollBarChanged += Dgv_HorizontalScrollBarChanged;
                }
            }
        }

        public void Clear()
        {
            dictionary.Clear();
            Invalidate();
        }

        private readonly Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public string this[string name]
        {
            get => dictionary.ContainsKey(name) ? dictionary[name] : "";
            set
            {
                if (dictionary.NotContainsKey(name))
                    dictionary.Add(name, value);
                else
                    dictionary[name] = value;

                Invalidate();
            }
        }

        private void Dgv_HorizontalScrollBarChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Dgv_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            Invalidate();
        }

        protected override void OnPaintFore(Graphics g, GraphicsPath path)
        {
            if (dgv.ColumnCount > 0 && dgv.RowCount > 0)
            {
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    Rectangle rect = dgv.GetCellDisplayRectangle(column.Index, 0, false);
                    if (rect.Left == 0 && rect.Width == 0) continue;

                    string str = this[column.Name];
                    if (str.IsNullOrEmpty()) continue;

                    SizeF sf = g.MeasureString(str, Font);
                    if (rect.Left == 0 && rect.Width < column.Width)
                    {
                        g.DrawString(str, Font, Color.White, rect.Width - column.Width + (column.Width - sf.Width) / 2.0f, (Height - sf.Height) / 2.0f);
                    }
                    else
                    {
                        g.DrawString(str, Font, Color.White, rect.Left + (column.Width - sf.Width) / 2.0f, (Height - sf.Height) / 2.0f);
                    }
                }
            }
        }

        public override void SetStyleColor(UIBaseStyle uiColor)
        {
            base.SetStyleColor(uiColor);
            if (uiColor.IsCustom()) return;

            fillColor = uiColor.PlainColor;

            Invalidate();
        }

        /// <summary>
        /// 填充颜色，当值为背景色或透明色或空值则不填充
        /// </summary>
        [Description("填充颜色"), Category("SunnyUI")]
        [DefaultValue(typeof(Color), "80, 160, 255")]
        public Color FillColor
        {
            get => fillColor;
            set => SetFillColor(value);
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Description("边框颜色"), Category("SunnyUI")]
        [DefaultValue(typeof(Color), "80, 160, 255")]
        public Color RectColor
        {
            get => rectColor;
            set => SetRectColor(value);
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        [Description("字体颜色"), Category("SunnyUI")]
        [DefaultValue(typeof(Color), "White")]
        public override Color ForeColor
        {
            get => foreColor;
            set => SetForeColor(value);
        }
    }
}
