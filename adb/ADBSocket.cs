using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace adb
{
    /// <summary>
    /// AndroidDebugBridge用ソケット
    /// </summary>
    public class ADBSocket : IDisposable
    {
        /// <summary>
        /// デフォルト通信先
        /// </summary>
        public static IPAddress Address = IPAddress.Parse("127.0.0.1");

        /// <summary>
        /// デフォルト通信先ポート
        /// </summary>
        public static int Port = 5037;

        /// <summary>
        /// Tcpソケット
        /// </summary>
        public TcpClient Socket { get; private set; }

        /// <summary>
        /// 通信先
        /// </summary>
        public IPEndPoint RemoteEP { get; private set; }

        /// <summary>
        /// メインバッファ
        /// </summary>
        private MemoryStream stream;

        /// <summary>
        /// バッファ
        /// </summary> 
        private byte[] buffer;

        /// <summary>
        /// 破棄フラグ
        /// </summary>
        protected bool disposed;

        public ADBSocket() : this(Address, Port) { }

        public ADBSocket(IPAddress address, int port)
        {
            // 初期化
            RemoteEP = new IPEndPoint(address, port);
            stream = new MemoryStream();
            buffer = new byte[4096];
            disposed = false;
        }

        public bool Connect()
        {
            if (Socket == null)
                Socket = new TcpClient();

            Socket.SendTimeout = 1000;
            Socket.ReceiveTimeout = 1000;

            try
            {
                Socket.Connect(RemoteEP);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public byte[] Send(string postData, bool okay = false)
        {
            if (Socket != null)
            {
                if (!Socket.Connected)
                    return Encoding.ASCII.GetBytes("FAIL");

                var post = $"{postData.Length:X4}{postData}";

                // ポストデータ
                var byteData =
                    Encoding.UTF8.GetBytes(post);

                // バッファの位置を戻す
                stream.Seek(0, SeekOrigin.Begin);

                // ストリーム
                var ns = Socket.GetStream();

                // 書き込み
                ns.Write(byteData, 0, byteData.Length);

                try
                {
                    if (okay)
                    {

                        //データの一部を受信する
                        var resSize = ns.Read(buffer, 0, 4);

                        //受信したデータを蓄積する
                        stream.Write(buffer, 0, resSize);
                    }
                    else
                    {
                        while (true)
                        {

                            //データの一部を受信する
                            var resSize = ns.Read(buffer, 0, buffer.Length);

                            if (resSize == 0)
                                break;

                            //受信したデータを蓄積する
                            stream.Write(buffer, 0, resSize);
                        }
                    }
                }
                catch
                {
                    return Encoding.ASCII.GetBytes("FAIL");
                }

                return stream.ToArray();
            }

            return null;
        }

        public void Close()
        {
            if (Socket != null)
            {
                Socket.Close();
                Socket = null;
            }
        }

        #region 破棄

        /// <summary>Disposeが呼ばれたら破棄します。</summary>
        /// <param name="disposing">破棄フラグ</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Socket != null)
                    {
                        Socket.Close();
                        Socket = null;
                    }

                    if (stream != null)
                    {
                        stream.Close();
                        stream = null;
                    }
                }
                disposed = true;
            }
        }

        /// <summary>Disposeメソッドです。</summary>
        public void Dispose()
        {
            // 使用したDLLを破棄します。
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>デストラクタ</summary>
        ~ADBSocket()
        {
            // デストラクタによる破棄は行いません。
            Dispose(false);
        }

        #endregion
    }
}
