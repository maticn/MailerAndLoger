using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailerAndLoger
{
    public class Strings
    {
        public static string ERROR_PATH_DISK = @"D:\";
        public static string OBVESTILO = "Obvestilo s projekta: ";

        public static string[] ERROR_FOLDER_AND_FILE(string method_caller)
        {
            String[] method_caller_split = method_caller.Split('.');
            string ERROR_FOLDER = method_caller_split[0];
            string ERROR_FILE = "Error_" + method_caller + ".txt";
            string[] result = new string[] { ERROR_FOLDER, ERROR_FILE };
            return result;
        }
    }
}
