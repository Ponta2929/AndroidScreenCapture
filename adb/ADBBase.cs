using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adb
{
    public class ADBBase
    {
        /// <summary>
        /// ADB通信用ソケット
        /// </summary>
        protected ADBSocket socket;

        public ADBBase()
        {
            // 通信ソケット初期化
            socket = new ADBSocket();
        }

        /// <summary>
        /// コマンドを送信します。
        /// </summary>
        public byte[] Command(string command)
        {
            // 接続
            socket.Connect();

            var result = socket.Send(command);

            socket.Close();

            return result;
        }

        /// <summary>
        /// コマンドを送信します。
        /// </summary>
        public byte[] Command(string command1, string command2)
        {
            // 接続
            socket.Connect();

            var result = socket.Send(command1, true);

            // 成功したか
            if (Encoding.ASCII.GetString(result, 0, 4).Contains("OKAY"))
                result = socket.Send(command2);

            socket.Close();

            return result;
        }

        /// <summary>
        /// ADBサーバーを開始します。
        /// </summary>
        public void StartServer()
        {
            var p = new Process();

            // ADB 設定
            p.StartInfo.FileName = @"adb.exe";
            p.StartInfo.Arguments = "start-server";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            // ADB Start
            p.Start();
            p.WaitForExit();
            p.Close();
        }

        /// <summary>
        /// ADBサーバーを停止します。
        /// </summary>
        public void KillServer()
        {
            var p = new Process();

            // ADB 設定
            p.StartInfo.FileName = "adb.exe";
            p.StartInfo.Arguments = "kill-server";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            // ADB Start
            p.Start();
            p.WaitForExit();
            p.Close();
        }
    }
}
