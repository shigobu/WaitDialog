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
using System.Windows.Shapes;

namespace RevitAddinRebar
{
    /// <summary>
    /// WaitDialigWPF.xaml の相互作用ロジック
    /// </summary>
    public partial class WaitDialogWPF : Window
    {
        private static WaitDialogWPF instance = null;

        private WaitDialogWPF()
        {
            InitializeComponent();
        }

        // Showと言うメソッド名を使いたかったので、newで上書きしていますが別にこれは任意の名前で構いません
        public static new void Show()
        {
            // BeginInvokeで実処理を別スレッド実行
            Action showProc = new Action(ShowProcess);
            IAsyncResult async = showProc.BeginInvoke(null, null);

            // そのままメインスレッドは処理が流れるため、別スレッドでインスタンスが生成されるまで待つ
            while (true)
            {
                if (instance != null)
                {
                    break;
                }
            }
            return;
        }

        private static void ShowProcess()
        {
            if (instance == null)
            {
                instance = new WaitDialogWPF();
            }

           // Showだと処理が流れてスレッドが終了してしまうので、ShowDialogで表示して
           // 別スレッド側は処理を待つ
           ((Window)instance).ShowDialog();
        }
    }
}