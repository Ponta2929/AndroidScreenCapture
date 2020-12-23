using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adb
{
    public sealed class ADBInput : ADBBase
    {
        #region Singleton

        /// <summary>
        /// インスタンス
        /// </summary>
        private static ADBInput instance;

        /// <summary>
        /// インスタンスを取得します。
        /// </summary>
        /// <returns></returns>
        public static ADBInput GetInstance() => instance ?? (instance = new ADBInput());

        #endregion

        /// <summary>
        /// 書き込みカウンター
        /// </summary>
        private long writeCount;

        /// <summary>
        /// タッチ操作を送信します。
        /// </summary>
        /// <param name="data"></param>
        public void Touch(Point data)
        {
            Command("host:transport-any", $"shell:input tap {data.X} {data.Y}");
        }

        /// <summary>
        /// スワイプ, タッチ操作を送信します。
        /// </summary>
        /// <param name="data"></param>
        public void SwipeAndTouch(string num, Point[] data)
        {
            Command("host:transport-any", $"shell:sendevent '/dev/input/{num}' '{3}' '{57}' '{++writeCount}'");
            Command("host:transport-any", $"shell:sendevent '/dev/input/{num}' '{3}' '{48}' '{10}'");
            Command("host:transport-any", $"shell:sendevent '/dev/input/{num}' '{3}' '{58}' '{29}'");

            for (var i = 0; i < data.Length; i++)
            {
                Command("host:transport-any", $"shell:sendevent '/dev/input/{num}' '{3}' '{53}' '{data[i].X}'");
                Command("host:transport-any", $"shell:sendevent '/dev/input/{num}' '{3}' '{54}' '{data[i].Y}'");
                Command("host:transport-any", $"shell:sendevent '/dev/input/{num}' '{0}' '{0}' '{0}'");

            }

            Command("host:transport-any", $"shell:sendevent '/dev/input/{num}' '{3}' '{57}' '{4294967295}'");
            Command("host:transport-any", $"shell:sendevent '/dev/input/{num}' '{0}' '{0}' '{0}'");
        }

        public void Retry(string command1, string command2, int retry = 1)
        {
            byte[] result;

            for (var i = 0; i < retry; i++)
            {
                result = Command(command1, command2);

                if (Encoding.ASCII.GetString(result, 0, 4).Contains("OKAY"))
                    break;
            }
        }

        /// <summary>
        /// タッチ操作用のイベント番号を取得します。
        /// </summary>
        /// <returns></returns>
        public string GetInputEvent()
        {
            byte[] result;

            for (var i = 0; i < 10; i++)
            {
                result = Command("host:transport-any",
                     $"shell:getevent '-i' '/dev/input/event{i}'");

                if (Encoding.ASCII.GetString(result).Contains("INPUT_PROP_DIRECT"))
                    return $"event{i}";
            }

            return "event0";
        }

        #region Convert

        public static Point ConvertToTouch(int width, Size size, Point value)
        {
            double v = (double)size.Width / (double)width;

            return new Point()
            {
                X = (int)(v * value.X),
                Y = (int)(v * value.Y)
            };
        }

        public static Point[] ConvertToTouch(int width, Size size, Point[] value)
        {
            var v = (double)size.Width / (double)width;

            var result = new Point[value.Length];

            for (var i = 0; i < value.Length; i++)
            {
                result[i] = new Point()
                {
                    X = (int)(v * value[i].X),
                    Y = (int)(v * value[i].Y)
                };
            }

            return result;
        }

        public static int WidthToHeight(int width, Size size)
        {
            return (int)(size.Height * ((double)width / (double)size.Width));
        }

        #endregion
    }
}
