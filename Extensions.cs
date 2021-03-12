using System;

namespace PMPlatform.JT808TestTool
{
    public class Extensions
    {
        public static string GenerateStringId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return $"{i - DateTime.Now.Ticks:x}";
        }
    }
}