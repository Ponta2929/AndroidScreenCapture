using adb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndroidScreenCapture
{
    public partial class MainForm : Form
    {
        private List<Point> swipes = new List<Point>();

        private int x, y;
        private int c_width, c_height;
        private Bitmap surface;

        private ADBImage image = ADBImage.GetInstance();
        private ADBInput input = ADBInput.GetInstance();

        private string eventNum = "event0";
        private bool swipe = false;

        public MainForm()
        {
            InitializeComponent();

            // 初期化
            Initialize();
        }

        private void Initialize()
        {
            image.CaptureComplete += image_CaptureComplete;

            if (CheckADB())
                image.StartServer();

            eventNum = input.GetInputEvent();
        }

        private void image_CaptureComplete(object sender, EventArgs e)
        {
            var bmp = image.Image.Clone() as Bitmap;

            if (surface == null || surface.Width != bmp.Width || surface.Height != bmp.Height)
            {
                surface = new Bitmap(bmp.Width, bmp.Height);

                // 比率計算
                var px = (float)surface.Width / (float)surface.Height;
                var width = this.ClientSize.Width;
                this.SetClientSizeCore(width, (int)(width / px));
            }

            // サーフェイス設定
            PictureBox_View.Image = surface;

            // ダブルバッファ
            using (var g = Graphics.FromImage(surface))
            {
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }
        }

        private void Timer_Capture_Tick(object sender, EventArgs e)
        {
            // キャプチャ
            image.Capture();
        }

        /// <summary>
        /// ADBが存在するかチェックします。
        /// </summary>
        /// <returns></returns>
        private bool CheckADB()
        {
            // 環境変数の取得（PATH要素）
            var variable = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Process);

            // パスの分割
            var list = variable.Split(';');

            foreach (var path in list)
            {
                // 実体の存在チェック
                if (File.Exists(System.IO.Path.Combine(path, "adb.exe")))
                {
                    return true;
                }
            }

            return false;
        }
        
        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            c_width = this.ClientSize.Width;
            c_height = this.ClientSize.Height;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            // 変更率の大きい方に比率を合わせる
            var c_w = (float)c_width / (float)this.ClientSize.Width;
            var c_h = (float)c_height / (float)this.ClientSize.Height;

            if (c_w < 1)
            {
                c_w -= 1.0f;
                c_w = -c_w;
            }
            else
            {
                c_w -= 1.0f;
            }

            if (c_h < 1)
            {
                c_h -= 1.0f;
                c_h = -c_h;
            }
            else
            {
                c_h -= 1.0f;
            }

            // 比率を保ってリサイズ
            if (surface != null && surface.Width != 0 && surface.Height != 0)
            {
                var px = (float)surface.Width / (float)surface.Height;

                if (c_w > c_h)
                {
                    var width = this.ClientSize.Width;

                    this.SetClientSizeCore(width, (int)(width / px));
                }
                else
                {
                    var height = this.ClientSize.Height;
                    this.SetClientSizeCore((int)(height * px), height);
                }
            }
        }

        private void PictureBox_View_MouseDown(object sender, MouseEventArgs e)
        {
            swipes.Clear();
            swipes.Add(e.Location);
            swipe = true;
        }

        private void ToolStripMenuItem_Save_Click(object sender, EventArgs e)
        {
            var capture = (Bitmap)surface.Clone();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Bitmap File|*.bmp";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    capture.Save(sfd.FileName);
                }
            }
        }

        private void PictureBox_View_MouseMove(object sender, MouseEventArgs e) =>
            (x, y) = (e.X, e.Y);

        private void Timer_Swipe_Tick(object sender, EventArgs e)
        {
            if (swipe)
            {
                swipes.Add(new Point() { X = x, Y = y });
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckADB())
                image.KillServer();
        }

        private void PictureBox_View_MouseUp(object sender, MouseEventArgs e)
        {
            if (surface != null)
            {
                swipes.Add(e.Location);

                input.SwipeAndTouch(eventNum, ADBInput.ConvertToTouch(this.ClientSize.Width, surface.Size, swipes.ToArray()));
            }

            swipe = false;
        }
    }
}
