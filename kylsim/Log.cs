using System;
using System.IO;

namespace kylsim
{
    public class Log
    {
        public string Message { get; set; }
        public static string Filename { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="Filename">The filename.</param>
        public Log()
        {
        }

        /// <summary>
        /// Creates the file.
        /// </summary>
        public void CreateFile()
        {
            File.Create(Filename);
        }

        /// <summary>
        /// Writes the specified MSG.
        /// </summary>
        /// <param name="Message">The Message.</param>
        public static void Write(string Filename, string Message)
        {
            DateTime dateTimeStamp = DateTime.Now;

            string stringToFile = Message + " " + dateTimeStamp.ToString();

            using (StreamWriter writer = File.AppendText(Filename))
            {
                writer.WriteLine(stringToFile);
            }
        }
    }
}
