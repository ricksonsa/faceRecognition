using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveService
{
    public class Logger
    {
        public static void Write(string logMessage)
        {
            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(exePath + @"\log.txt"))
                {
                    Log(logMessage, w);
                }

            }
            catch (Exception)
            {
            }
        }

        private static void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine(logMessage);
                txtWriter.WriteLine("---------------------------------------------------------------------------------------------");
            }
            catch (Exception)
            {
            }
        }
    }   
}

