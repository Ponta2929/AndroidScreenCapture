using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace adb
{
    public sealed class ADBImage : ADBBase
    {
        #region Singleton

        /// <summary>
        /// インスタンス
        /// </summary>
        private static ADBImage instance;

        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        /// <returns></returns>
        public static ADBImage GetInstance() => instance ?? (instance = new ADBImage());

        #endregion

        /// <summary>
        /// キャプチャが完了されると呼び出されます。
        /// </summary>
        public event EventHandler CaptureComplete;

        /// <summary>
        /// キャプチャされたイメージです。
        /// </summary>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// キャプチャ中
        /// </summary>
        private bool capturing;

        /// <summary>
        /// キャプチャ出来た
        /// </summary>
        private bool captured;

        public void Capture()
        {
            if (capturing)
                return;

            var task = Task.Run(() =>
            {
                capturing = true;

                // データ
                var value
                    = Command("host:transport-any", "framebuffer:");

                if (Encoding.ASCII.GetString(value, 0, 4).Contains("OKAY"))
                    captured = true;

                // OKAY or FAIL
                if (captured)
                {
                    var size = BitConverter.ToInt32(value, 12);
                    var width = BitConverter.ToInt32(value, 16);
                    var height = BitConverter.ToInt32(value, 20);

                    // 再作成
                    if (Image == null || Image.Width != width || Image.Height != height)
                        Image = new Bitmap(width, height);

                    // Bitmap固定
                    var bmpData = Image.LockBits(new Rectangle(new Point(), Image.Size), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

                    // 画像色順
                    for (int i = 0; i < value.Length; i += 4)
                    {
                        var v = value[i + 0];

                        value[i + 0] = value[i + 2];
                        value[i + 2] = v;
                    }

                    // 配列コピー
                    Marshal.Copy(value, 56, bmpData.Scan0, size);

                    // 固定解除
                    Image.UnlockBits(bmpData);
                }
            })
            .ContinueWith(t =>
            {
                if (captured)
                {
                    // イベント
                    if (CaptureComplete != null)
                        CaptureComplete(this, EventArgs.Empty);
                }

                // キャプチャ完了
                capturing = false;
                captured = false;

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
