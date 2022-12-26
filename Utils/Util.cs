using CsvHelper;
using System.Globalization;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Maps;
using System.Collections;
using CsvHelper.Configuration;

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
  }
}
