using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAddinRebar
{
    public partial class WaitDialog : Form
    {
        // 外からインスタンスを生成できないようにコンストラクタはprivate
        private static WaitDialog mInstance = null;

        private WaitDialog()
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
                if (mInstance != null)
                {
                    break;
                }
            }
            return;
        }

        private static void ShowProcess()
        {
            if (mInstance == null || mInstance.IsDisposed == true)
            {
                mInstance = new WaitDialog();
            }

           // Showだと処理が流れてスレッドが終了してしまうので、ShowDialogで表示して
           // 別スレッド側は処理を待つ
           ((Form)mInstance).ShowDialog();
        }

        // クローズ処理
        public static new void Close()
        {
            if (mInstance == null || mInstance.IsDisposed == true) return;

            if (mInstance.InvokeRequired == true)
            {
                mInstance.Invoke(new Action(Close));
            }
            else
            {
                ((Form)mInstance).Close();
            }
        }

        // プログレスバー最大値の設定
        public static void SetProgressBar(int iMax)
        {
            if (mInstance == null || mInstance.IsDisposed == true) return;

            if (mInstance.InvokeRequired == true)
            {
                mInstance.Invoke(new Action<int>(SetProgressBar), new object[] { iMax });
            }
            else
            {
                mInstance.progressBar1.Maximum = iMax;
            }
        }

        // プログレスバー現在値の設定
        public static void SetProgressBarValue(int iVal)
        {
            if (mInstance == null || mInstance.IsDisposed == true) return;

            if (mInstance.InvokeRequired == true)
            {
                mInstance.Invoke(new Action<int>(SetProgressBarValue), new object[] { iVal });
            }
            else
            {
                mInstance.progressBar1.Value = iVal;
            }
        }
    }
}