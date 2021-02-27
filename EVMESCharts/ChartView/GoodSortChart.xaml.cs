using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace EVMESCharts.ChartView
{
    /// <summary>
    /// GoodSortChart.xaml 的交互逻辑
    /// </summary>
    public partial class GoodSortChart : UserControl
    {
        public GoodSortChart()
        {
            InitializeComponent();

            // 定义图表X轴Y轴显示
            var mapper = Mappers.Xy<DefectData>()
            .X(model => model.Index)
            .Y(model => model.Value);
            Charting.For<DefectData>(mapper);

            FontColor = MainWindow.WindowFontColor;
            BgColor = MainWindow.WindowBgColor;

            // 获取饼状图数据
            GetDefectPieChartList();
            // 获取柱状图数据
            GetDefectBarChartList();

            DataContext = this;
        }

        #region 数据定义
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
        /// 饼状图数据
        /// </summary>
        public SeriesCollection DefectPieChart
        {
            get;
            set;
        } = new SeriesCollection();

        /// <summary>
        /// 饼状图标签
        /// </summary>
        public Func<ChartPoint, string> PointLabel
        {
            get;
            set;
        }

        /// <summary>
        /// 柱状图图表数据
        /// </summary>
        public SeriesCollection DefectSortList
        {
            get;
            private set;
        } = new SeriesCollection();

        /// <summary>
        /// 数据集合
        /// </summary>
        public List<DefectDataList> ChartDataList
        {
            get;
            set;
        } = new List<DefectDataList>();

        /// <summary>
        /// 柱状图数据组
        /// </summary>
        public class DefectDataList
        {
            /// <summary>
            /// 产量柱状图
            /// </summary>
            public ChartValues<DefectData> BarChartData
            {
                get;
                set;
            } = new ChartValues<DefectData>();
        }

        #endregion

        #region 函数
        /// <summary>
        /// 获取饼状图数据信息
        /// </summary>
        private void GetDefectPieChartList()
        {
            // 格式化饼状图标签字符串
            PointLabel = chartPoint =>
                string.Format("{0}", chartPoint.Y, chartPoint.Participation);

            for (int i = 0; i < 5; i++)
            {
                BrushConverter brushconverter = new BrushConverter();

                // 添加饼状图数据
                DefectPieChart.Add(new PieSeries
                {
                    Title = MainWindow.FaultList[i % MainWindow.FaultList.Length],
                    Values = new ChartValues<double>(),
                    DataLabels = true,
                    LabelPoint = PointLabel,
                    Fill = (Brush)brushconverter.ConvertFromString(MainWindow.ColorList[i % MainWindow.ColorList.Length])
                });
                GetPieChartValue(i);
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="i"></param>
        private void GetPieChartValue(int i)
        {
            DefectPieChart[i].Values = new ChartValues<double> { 5 };
        }

        /// <summary>
        /// 获取柱状图数据
        /// </summary>
        private void GetDefectBarChartList()
        {

            ChartDataList.Add(new DefectDataList());
            BrushConverter brushconverter = new BrushConverter();
            Brush color = (Brush)brushconverter.ConvertFromString(MainWindow.ColorList[1 % MainWindow.ColorList.Length]);
            color.Opacity = 0.6;
            DefectSortList.Add(new StackedColumnSeries
            {
                Values = new ChartValues<double>() { 1, 2, 3 },
                DataLabels = true,
                Title = "",
                Fill = color,
            });

            GetBarChartValue();
        }

        /// <summary>
        /// 获取柱状图数据
        /// </summary>
        private void GetBarChartValue()
        {
        }

        #endregion
    }

    /// <summary>
    /// 故障数据图表数据类型
    /// </summary>
    public class DefectData
    {
        /// <summary>
        /// X轴对应数据
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Y轴对应数据
        /// </summary>
        public double Value
        {
            get;
            set;
        }
    }
}
