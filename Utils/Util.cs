using CsvHelper;
using System.Globalization;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Maps;
using System.Collections;
using CsvHelper.Configuration;
using Spectre.Console;

namespace AdaCredit.Utils
{
  public static class Util
  {
    public static bool WriteFileInDesktop<T>(string fileName, List<T> entities)
    {
      try
      {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, fileName);

        using var writer = new StreamWriter(filePath);
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
          csv.Context.RegisterClassMap<EmployeeMap>();
          csv.WriteRecords(entities);
          csv.Flush();
        }

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao escrever arquivo {fileName}");
        return false;
      }
    }

    public static string ReadLinePassword()
    {
      var password = string.Empty;
      ConsoleKeyInfo info = Console.ReadKey(true);

      while(info.Key != ConsoleKey.Enter)
      {
        if (info.Key == ConsoleKey.Backspace)
        {
          if (!string.IsNullOrEmpty(password))
          {
            password = password.Substring(0, password.Length - 1);
            
            int pos = Console.CursorLeft;

            Console.SetCursorPosition(pos - 1, Console.CursorTop);
            Console.Write(" ");
            Console.SetCursorPosition(pos - 1, Console.CursorTop);

            info = Console.ReadKey(true);
            continue;
          }
        }

        Console.Write("*");
        password += info.KeyChar;

        info = Console.ReadKey(true);
      }

      Console.WriteLine();
      return password;
    }

    public static SelectionPrompt<string> CreateSelectionList(string title, List<string> options)
    {
      var selectionPrompt = new SelectionPrompt<string>()
              .Title(title)
              .PageSize(10)
              .AddChoices(options);

      selectionPrompt.HighlightStyle = new Style(Color.White);

      return selectionPrompt;
    }
  }
}
