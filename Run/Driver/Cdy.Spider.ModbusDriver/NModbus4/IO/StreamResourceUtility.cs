namespace Modbus.IO
{
    using Cdy.Spider;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    internal static class StreamResourceUtility
    {
        internal static string ReadLine(ICommChannel2 stream, int timeout)
        {
            var result = new StringBuilder();
            var singleByteBuffer = new byte[1];
            Stopwatch sw = new Stopwatch();
            do
            {
                if (stream.Read(singleByteBuffer, 0, 1) == 0)
                {
                    continue;
                }

                result.Append(Encoding.UTF8.GetChars(singleByteBuffer).First());
            }
            while (!result.ToString().EndsWith(Modbus.NewLine) && sw.ElapsedMilliseconds < timeout);

            sw.Stop();

            return result.ToString().Substring(0, result.Length - Modbus.NewLine.Length);
        }
    }
}
