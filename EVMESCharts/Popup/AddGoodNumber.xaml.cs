using EVMESCharts.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace EVMESCharts.Popup
{
    /// <summary>
    /// AddGoodNumber.xaml 的交互逻辑
    /// </summary>
    public partial class AddGoodNumber : Window
    {
        /// <summary>
        /// 良品数输入页面
        /// </summary>
        /// <param name="Date">日期</param>
        public AddGoodNumber(string Date)
        {
            // 使弹框位于页面正中间
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            // 表格标题
            AddGoodTitle = $"{Date}  生产数据";

            // 存储传入的时间信息
            Daytime = Date;

            // 添加表格数据
            AddTableData(Date);

            FontColor = MainWindow.WindowFontColor;
            BgColor = MainWindow.WindowBgColor;

            DataContext = this;
        }
        
        /// <summary>
        /// 定义字体颜色
        /// </summary>
        public string FontColor
        {
            get;
            set;
        }

        /// <summary>
        /// 定义控件背景颜色
        /// </summary>
        public string BgColor
        {
            get;
            set;
        }
        /// <summary>
        /// 用于存储时间信息
        /// </summary>
        public string Daytime
        {
            get;
            set;
        }

        /// <summary>
        /// 生产数据信息
        /// </summary>
        public ObservableCollection<GoodDataFormat> AddGoodList
        {
            get;
            set;
        } = new ObservableCollection<GoodDataFormat>();

        /// <summary>
        /// 添加表格数据
        /// </summary>
        /// <param name="Date">时间日期</param>
        public void AddTableData(string Date)
        {
            string sql = $"SELECT Time,Produce,GoodProduct FROM DayDeviceData WHERE Day = '{Date}' AND DevID = {0}";
            DataTable data = SQLiteHelp.ExecuteQuery(sql);
            // 判断是否存在数据
            if (data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (int.Parse(data.Rows[i][1].ToString()) > 0)
                    {
                        AddGoodList.Add(new GoodDataFormat()
                        {
                            Time = data.Rows[i][0].ToString(),
                            Produce = data.Rows[i][1].ToString(),
                            Good = data.Rows[i][2].ToString()
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 保存按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKClick(object sender, RoutedEventArgs e)
        {
            int GoodSum = 0;
            int ProduceSum = 0;
            for (int i = 0; i < AddGoodList.Count; i++)
            {
                int Produce = int.Parse(AddGoodList[i].Produce);
                int Good = int.Parse(AddGoodList[i].Good);
                if (Produce < Good)
                {
                    Message message = new Message(1, $"{AddGoodList[i].Time}:00 的良品数超过了生产总数");
                    message.ShowDialog();
                    return;
                }
                else
                {
                    GoodSum = GoodSum + Good;
                    ProduceSum = ProduceSum + Produce;
                    // 修改日数据库数据 UPDATE 表名称 SET 列名称 = 新值 WHERE 列名称 = 某值
                    string sql = $"UPDATE DayDeviceData SET Produce = '{Produce}',GoodProduct = {Good} WHERE Time = {AddGoodList[i].Time} AND Day = '{Daytime}'";
                    SQLiteHelp.SQLUpdate(sql);
                }
            }

            // 更新月数据库
            // 查询月数据下是否包含当日数据
            string updatasql = $"UPDATE MonthDeviceData SET Produce = '{ProduceSum}',GoodProduct = {GoodSum} WHERE Day = '{Daytime}' AND DevID = {0}";
            SQLiteHelp.SQLUpdate(updatasql);

            // 关闭弹框
            DialogResult = true;

            Message success = new Message(0, "修改成功");
            success.Show();
        }

        /// <summary>
        /// 页面绑定标题
        /// </summary>
        public string AddGoodTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 良品数据表格数据格式
        /// </summary>
        public class GoodDataFormat
        {
            /// <summary>
            /// 时间
            /// </summary>
            public string Time
            {
                get;
                set;
            }

            /// <summary>
            /// 生产数量
            /// </summary>
            public string Produce
            {
                get;
                set;
            }

            /// <summary>
            /// 良品数
            /// </summary>
            public string Good
            {
                get;
                set;
            }

            /// <summary>
            /// 烤焦（不良统计）
            /// </summary>
            public string Burnt 
            {
                get;
                set;
            }

            /// <summary>
            /// 冲切大小边（不良统计）
            /// </summary>
            public string DieCutDiversity 
            {
                get;
                set;
            }

            /// <summary>
            /// 压印（不良统计）
            /// </summary>
            public string Imprint 
            {
                get;
                set;
            }

            /// <summary>
            /// 网布反（不良统计）
            /// </summary>
            public string FabricBack 
            {
                get;
                set;
            }

            /// <summary>
            /// 上料偏位（不良统计）
            /// </summary>
            public string Deviation
            {
                get;
                set;
            }

            /// <summary>
            /// 高低边（不良统计）
            /// </summary>
            public string Side 
            {
                get;
                set;
            }

            /// <summary>
            /// 双层底皮（不良统计）
            /// </summary>
            public string DoubleLayer 
            {
                get;
                set;
            }

            /// <summary>
            /// 发亮（不良统计）
            /// </summary>
            public string Shine 
            {
                get;
                set;
            }

            /// <summary>
            /// 孔内毛刺（不良统计）
            /// </summary>
            public string Glitch 
            {
                get;
                set;
            }

            /// <summary>
            /// 定型不良（不良统计）
            /// </summary>
            public string ShapeBad 
            {
                get;
                set;
            }

            /// <summary>
            /// 打孔不良（不良统计）
            /// </summary>
            public string PunchingBad 
            {
                get;
                set;
            }

            /// <summary>
            /// 褶皱（不良统计）
            /// </summary>
            public string Fold 
            {
                get;
                set;
            }

            /// <summary>
            /// 压轻（不良统计）
            /// </summary>
            public string PressureGently 
            {
                get;
                set;
            }

            /// <summary>
            /// 废料（不良统计）
            /// </summary>
            public string Waste 
            {
                get;
                set;
            }

            /// <summary>
            /// 脏污（不良统计）
            /// </summary>
            public string Dirty 
            {
                get;
                set;
            }

            /// <summary>
            /// 破损（不良统计）
            /// </summary>
            public string Damaged 
            {
                get;
                set;
            }

            /// <summary>
            /// 发蓝（不良统计）
            /// </summary>
            public string Blueing 
            {
                get;
                set;
            }
        }
    }
}
