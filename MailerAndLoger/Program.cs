using System;

namespace MailerAndLoger
{
  class Program
  {
    static void Main(string[] args)
    {
      DateTime rojstni_dan = new DateTime(1966, 11, 16); ;
      int starost = 50;

      if (DateTime.Today.AddYears(-starost).Equals(rojstni_dan))
      {
        Console.WriteLine("Vse najboljše za tvojih " + starost + " let.");
      }
    }
  }
}
