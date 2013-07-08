using System;
using System.Windows;
using System.Windows.Interop;
using WinForms = System.Windows.Forms;

namespace ThScoreFileConverter
{
    /// <summary>
    /// System.Windows.Window から System.Windows.Forms.IWin32Window への変換用クラス
    /// </summary>
    public class Win32Window : WinForms.IWin32Window
    {
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param Name="window">変換元の System.Windows.Window インスタンス</param>
        public Win32Window(Window window)
        {
            this.Handle = new WindowInteropHelper(window).Handle;
        }
    }
}
