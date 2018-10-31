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
        public static async new void Show()
        {
            await ShowProcessAsync();
        }

        private static Task ShowProcessAsync()
        {
            Action showProc = new Action(ShowProcess);
            return Task.Run(showProc);
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

        // クローズ処理
        public static new void Close()
        {
            if (instance == null) return;

            if (instance.Dispatcher.CheckAccess())
            {
                ((Window)instance).Close();
                instance = null;
            }
            else
            {
                instance.Dispatcher.Invoke(new Action(Close));
            }
        }

        // プログレスバー最大値の設定
        public static void SetProgressBar(int max)
        {
            if (instance == null) return;

            if (instance.Dispatcher.CheckAccess())
            {
                instance.progressBar.Maximum = max;
            }
            else
            {
                instance.Dispatcher.Invoke(new Action<int>(SetProgressBar), new object[] { max });
            }
        }

        // プログレスバー現在値の設定
        public static void SetProgressBarValue(int val)
        {
            if (instance == null) return;

            if (instance.Dispatcher.CheckAccess())
            {
                instance.progressBar.Value = val;
            }
            else
            {
                instance.Dispatcher.Invoke(new Action<int>(SetProgressBarValue), new object[] { val });
            }
        }
    }
}