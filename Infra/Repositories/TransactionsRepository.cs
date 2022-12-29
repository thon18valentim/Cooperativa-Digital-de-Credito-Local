using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Maps;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit.Infra.Repositories
{
  public sealed class TransactionsRepository
  {
    private static string pendingPath;
    private static string completedPath;
    private static string failedPath;

    static TransactionsRepository()
    {
      Setup();
    }

    private static void Setup()
    {
      var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

      pendingPath = Path.Combine(desktopPath, "Transactions", "Pending");
      completedPath = Path.Combine(desktopPath, "Transactions", "Completed");
      failedPath = Path.Combine(desktopPath, "Transactions", "Failed");

      try
      {
        Directory.CreateDirectory(pendingPath);
        Directory.CreateDirectory(completedPath);
        Directory.CreateDirectory(failedPath);
      }
      catch
      {
        throw;
      }
    }

    public static Transaction[][]? LoadPending()
    {
      DirectoryInfo di = new(pendingPath);

      FileInfo[] pendingFiles = di.GetFiles("*.csv");

      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = false,
      };

      try
      {
        Transaction[][] transactions = new Transaction[pendingFiles.Length][];

        for (int i = 0; i < pendingFiles.Length; i++)
        {
          var filePath = Path.Combine(pendingPath, pendingFiles[i].Name);
          using var reader = new StreamReader(filePath);
          using var csv = new CsvReader(reader, config);
          csv.Context.RegisterClassMap<TransactionMap>();
          var records = csv.GetRecords<Transaction>().ToArray();
          transactions[i] = records;
        }

        return transactions;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao ler arquivos de transações");
        return default;
      }
    }
  }
}
