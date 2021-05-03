using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_B1._1._4
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.I4)] int SessionId,
            String pTitle,
            [MarshalAs(UnmanagedType.U4)] int TitleLength,
            String pMessage,
            [MarshalAs(UnmanagedType.U4)] int MessageLength,
            [MarshalAs(UnmanagedType.U4)] int Style,
            [MarshalAs(UnmanagedType.U4)] int Timeout,
            [MarshalAs(UnmanagedType.U4)] out int pResponse,
            bool bWait);

        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        public static int WTS_CURRENT_SESSION = 2; //giá trị này thay đổi tùy thuộc vào từng máy

        /// <summary>
        /// Hàm có chức năng pop-up MSSV
        /// </summary>
        public static void PopUp()
        {
            bool result = false;
            String title = "PopUp";
            int tlen = title.Length;
            String msg = "18521215";
            int mlen = msg.Length;
            int resp = 0;
            result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, WTS_CURRENT_SESSION, title, tlen, msg, mlen, 0, 0, out resp, false);
        }

        /// <summary>
        /// Hàm có chức năng bắt sự kiện lock, unlock,... của máy tính
        /// </summary>
        /// <param name="changeDescription"></param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {

            switch (changeDescription.Reason)
            {   
                //trường hợp user login và unlock máy tính thì gọi hàm PopUp
                case SessionChangeReason.SessionLogon:
                case SessionChangeReason.SessionUnlock:
                    {
                        PopUp();
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
