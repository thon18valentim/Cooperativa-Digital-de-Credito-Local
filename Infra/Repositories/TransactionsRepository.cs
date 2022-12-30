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
using AdaCredit.Utils;

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

      FileInfo[] pendingFiles = di.EnumerateFiles("*.csv").ToArray();

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

          foreach (Transaction t in records)
          {
            t.BankName = Util.GetBankFromFileName(pendingFiles[i].Name);
            t.Date = Util.GetDateFromFileName(pendingFiles[i].Name);
          }

          transactions[i] = records;
        }

        foreach (FileInfo file in di.EnumerateFiles())
          file.Delete();

        return transactions;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao ler arquivos de transações");
        return default;
      }
    }

    public static bool SaveCompleted(List<Transaction> transactions, string bankName)
    {
      DateTime now = DateTime.Now;
      var fileName = $"{bankName}-{now.Year}{now.Month}{now.Day}-completed.csv";

      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = false,
      };

      try
      {
        var filePath = Path.Combine(completedPath, fileName);

        using var writer = new StreamWriter(filePath);
        using (var csv = new CsvWriter(writer, config))
        {
          csv.Context.RegisterClassMap<TransactionMap>();
          csv.WriteRecords(transactions);
          csv.Flush();
        }

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao escrever arquivo de transações concluídos");
        return false;
      }
    }

    public static bool SaveFailure(List<Transaction> transactions, List<TransactionError> errors, string bankName)
    {
      DateTime now = DateTime.Now;
      var fileName = $"{bankName}-{now.Year}{now.Month}{now.Day}-failed.csv";

      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = false,
      };

      try
      {
        var filePath = Path.Combine(failedPath, fileName);

        //using var writer = new StreamWriter(filePath);
        //using (var csv = new CsvWriter(writer, config))
        //{
        //  csv.Context.RegisterClassMap<TransactionMap>();
        //  csv.WriteRecords(transactions);
        //  csv.Flush();
        //}

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, config))
        {
          //csv.WriteHeader<Transaction>();
          //csv.NextRecord();
          int index = 0;
          foreach (var record in transactions)
          {
            csv.WriteRecord(record);
            csv.WriteRecord(errors[index]);
            csv.NextRecord();
            index++;
          }
        }

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao escrever arquivo de transações com falha");
        return false;
      }
    }
  }
}
