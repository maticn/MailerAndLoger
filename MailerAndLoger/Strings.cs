using System;

namespace MailerAndLoger
{
  public class Strings
  {
    public static string ERROR_PATH_DISK = @"C:\Users\maticn\Desktop\";
    public static string SOURCE_PROJECT = "The notification from project: ";

    public static string[] ERROR_FOLDER_AND_FILE(string method_caller)
    {
      String[] method_caller_split = method_caller.Split('.');
      string error_folder = method_caller_split[0];
      string error_file = "Error_" + method_caller + ".txt";
      string[] result = new string[] { error_folder, error_file };
      return result;
    }
  }
}
