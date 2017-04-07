using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailerAndLoger
{
    public class Loger
    {
        public static void PrintInFile(string output, List<string> outputs, string fileName, bool printConsole)
        {
            string ERROR_FOLDER = "";
            string ERROR_FILE = "";

            try
            {
                string method_caller = new StackTrace().GetFrame(1).GetMethod().DeclaringType.FullName.ToString();
                ERROR_FOLDER = Strings.ERROR_FOLDER_AND_FILE(method_caller)[0];
                ERROR_FILE = Strings.ERROR_FOLDER_AND_FILE(method_caller)[1];

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true))
                {
                    if (outputs != null)
                    {
                        foreach (string el in outputs)
                        {
                            if (printConsole)
                            {
                                Console.WriteLine(el);
                            }
                            file.WriteLine(el);
                        }
                    }
                    else
                    {
                        if (printConsole)
                        {
                            Console.WriteLine(output);
                        }
                        file.WriteLine(output);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                string error = "Writing in file exception: ObjectDisposedException.";
                Console.WriteLine(error);
                PrintInFile(error, null, Strings.ERROR_PATH_DISK + ERROR_FOLDER + ERROR_FILE, false);
            }
            catch (IndexOutOfRangeException)
            {
                string error = "Writing in file exception: IndexOutOfRangeException.";
                Console.WriteLine(error);
                PrintInFile(error, null, Strings.ERROR_PATH_DISK + ERROR_FOLDER + ERROR_FILE, false);
            }
        }
    }
}
