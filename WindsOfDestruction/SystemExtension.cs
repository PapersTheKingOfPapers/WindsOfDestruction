using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindsOfDestruction
{
    public static class SystemExtension
    {
        public static ConsoleColor defaultForegroundColor = ConsoleColor.White;
        public static ConsoleColor defaultBackgroundColor = ConsoleColor.Black;
        
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static ConsoleKeyInfo cki;

        public static dynamic Input<T>()
        {
            object key = cki.KeyChar;

            if (cki.KeyChar == '\u001a')
            {
                return "UNDO";
            }
            else
            {
                return cki.KeyChar.ToString();
            }
            return false;
        }

        #region WriteWithColors
        //Write Colored
        public static void WriteColored(string inputText, ConsoleColor foregroundColor)
        {
            Console.Write(inputText, Console.ForegroundColor = foregroundColor);
            Console.ForegroundColor = defaultForegroundColor;
        }
        public static void WriteColored(string inputText, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.Write(inputText, Console.ForegroundColor = foregroundColor, Console.BackgroundColor = backgroundColor);
            Console.ForegroundColor = defaultForegroundColor;
            Console.BackgroundColor = defaultBackgroundColor;
        }
        //WriteLine Colored
        public static void WriteLineColored(string inputText, ConsoleColor foregroundColor)
        {
            Console.WriteLine(inputText, Console.ForegroundColor = foregroundColor);
            Console.ForegroundColor = defaultForegroundColor;
        }
        public static void WriteLineColored(string inputText, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.WriteLine(inputText, Console.ForegroundColor = foregroundColor, Console.BackgroundColor = backgroundColor);
            Console.ForegroundColor = defaultForegroundColor;
            Console.BackgroundColor = defaultBackgroundColor;
        }
        #endregion
    }
}
