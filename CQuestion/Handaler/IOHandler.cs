using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Question.Handaler
{
    class IOHandler
    {
        private static Action<String> m_WriteLine;
        private static Action<String> m_Write;
        private static Func<String> m_ReadLine;
        public IOHandler()
        {
        }
        public IOHandler(Action<String> writeLine, Action<String> write, Func<String> readLine)
        {
        }
        public static IOHandler ConsoleHandler
        {
            get
            {
                return new IOHandler(Console.WriteLine, Console.Write, Console.ReadLine);
            }
        }
        internal override String Input
        {
            get
            {
                return "";
            }
            set
            {
                return;
            }
        }

        internal void dispaly(String p)
        {
            throw new NotImplementedException();
        }
    }
}
