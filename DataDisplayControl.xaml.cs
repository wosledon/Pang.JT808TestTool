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

namespace PMPlatform.JT808TestTool
{
    /// <summary>
    /// DataDisplayControl.xaml 的交互逻辑
    /// </summary>
    public partial class DataDisplayControl : UserControl
    {
        private readonly int _count;
        private readonly string _data;

        public DataDisplayControl(int count, string data)
        {
            _count = count;
            _data = data;
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            TbkId.Text = $"[{_count}]";
            TbkTime.Text = $"{DateTime.Now}";
            TbkData.Text = _data;
        }
    }
}
