using CsvHelper;
using System.Globalization;
using AdaCredit.Domain.Entities.Maps;
using Spectre.Console;
using AdaCredit.Domain.UseCases;

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

      selectionPrompt.HighlightStyle = new Style(Color.Orange1);

      return selectionPrompt;
    }

    public static string GenerateCpf()
    {
      int sum = 0;
      int[] multiply1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      int[] multiply2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

      Random rnd = new();
      string seed = rnd.Next(100000000, 999999999).ToString();

      for (int i = 0; i < 9; i++)
        sum += int.Parse(seed[i].ToString()) * multiply1[i];

      int rest = sum % 11;
      if (rest < 2)
        rest = 0;
      else
        rest = 11 - rest;

      seed += rest;
      sum = 0;

      for (int i = 0; i < 10; i++)
        sum += int.Parse(seed[i].ToString()) * multiply2[i];

      rest = sum % 11;

      if (rest < 2)
        rest = 0;
      else
        rest = 11 - rest;

      seed += rest;

      return seed;
    }

    public static string GetCpf(this string original)
    {
      int start = 0;

      for (int i = 0; i < original.Length; i++)
      {
        if (original[i] == '-')
          start = i;
      }

      return original[(start + 1)..].Trim();
    }

    public static bool ValidateCpf(string? cpf)
    {
      if (cpf == null)
        return false;

      int[] multiply1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
      int[] multiply2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

      cpf = cpf.Trim();
      cpf = cpf.Replace(".", "").Replace("-", "");

      if (cpf.Length != 11)
        return false;

      var tempCpf = cpf.Substring(0, 9);
      var sum = 0;

      for (int i = 0; i < 9; i++)
        sum += int.Parse(tempCpf[i].ToString()) * multiply1[i];

      var rest = sum % 11;

      if (rest < 2)
        rest = 0;
      else
        rest = 11 - rest;

      var digit = rest.ToString();
      tempCpf += digit;
      sum = 0;

      for (int i = 0; i < 10; i++)
        sum += int.Parse(tempCpf[i].ToString()) * multiply2[i];

      rest = sum % 11;

      if (rest < 2)
        rest = 0;
      else
        rest = 11 - rest;

      digit += rest.ToString();

      return cpf.EndsWith(digit);
    }

    public static string GetBankFromFileName(string fileName)
    {
      int start = GetLastOf(fileName, '-');

      return fileName[..start].Trim();
    }

    public static DateTime GetDateFromFileName(string fileName)
    {
      int start = GetLastOf(fileName, '-');

      var date = 
        fileName[(start + 1)..].Replace(".csv","").Trim().Insert(4, "/").Insert(7, "/");

      return Convert.ToDateTime(date);
    }

    private static int GetLastOf(string text, char target)
    {
      int index = 0;
      for (int i = 0; i < text.Length; i++)
      {
        if (char.Equals(text[i], target))
          index = i;
      }

      return index;
    }

    public static bool ToBool(this IUseCaseParameter parameter)
    {
      return Convert.ToBoolean(parameter.ParameterValue);
    }

    public static string? ToStringValue(this IUseCaseParameter parameter)
    {
      return Convert.ToString(parameter.ParameterValue);
    }

    public static List<T> ToList<T>(this IUseCaseParameter parameter)
    {
      return (List<T>)parameter.ParameterValue;
    }

    public static Table Format(this Table table)
    {
      table.Centered();
      table.Border(TableBorder.Rounded);

      return table;
    }
  }
}
