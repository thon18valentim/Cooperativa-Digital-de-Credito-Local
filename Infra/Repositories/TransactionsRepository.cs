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
    public static void Setup()
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

    public static List<Transaction> LoadPending()
    {
      DirectoryInfo di = new(pendingPath);

      List<FileInfo> pendingFiles = di.EnumerateFiles("*.csv").ToList();

      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = false,
        NewLine = Environment.NewLine
      };

      try
      {
        List<Transaction> transactions = new();

        foreach (var file in pendingFiles)
        {
          var fileName = file.Name;
          var filePath = Path.Combine(pendingPath, fileName);

          using var reader = new StreamReader(filePath);
          using var csv = new CsvReader(reader, config);
          csv.Context.RegisterClassMap<TransactionMap>();
          var records = csv.GetRecords<Transaction>().ToList();

          foreach (Transaction t in records)
          {
            t.BankName = Util.GetBankFromFileName(fileName);
            t.Date = Util.GetDateFromFileName(fileName);

            transactions.Add(t);
          }
        }

        foreach (FileInfo file in di.EnumerateFiles())
          file.Delete();

        return transactions;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao ler arquivos de transações pendentes");
        return default;
      }
    }

    public static Transaction[]? LoadFailed()
    {
      DirectoryInfo di = new(failedPath);

      FileInfo[] failedFiles = di.EnumerateFiles("*.csv").ToArray();

      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = false,
        NewLine = Environment.NewLine
      };

      try
      {
        List<Transaction> transactions = new();

        for (int i = 0; i < failedFiles.Length; i++)
        {
          var filePath = Path.Combine(failedPath, failedFiles[i].Name);

          using var reader = new StreamReader(filePath);
          using (var csv = new CsvReader(reader, config))
          {
            while (csv.Read())
            {
              var record = csv.GetRecord<Transaction>();
              record.ErrorMessage = csv.GetField(9);

              transactions.Add(record);
            }
          }
        }

        return transactions.ToArray();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao ler arquivos de transações com falhas");
        return default;
      }
    }

    public static bool SaveCompleted(List<Transaction> transactions)
    {
      var bankName = transactions[0].BankName;
      var date = transactions[0].Date;

      var fileName =
        $"{bankName}-{date.Year}{date.Month.ToString().PadLeft(2, '0')}{date.Day.ToString().PadLeft(2, '0')}-completed.csv";

      var filePath = Path.Combine(completedPath, fileName);

      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = false,
        NewLine = Environment.NewLine
      };

      try
      {
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

    public static bool SaveFailure(List<Transaction> transactions)
    {
      DateTime date = transactions[0].Date;
      var bankName = transactions[0].BankName;

      var fileName =
        $"{bankName}-{date.Year}{date.Month.ToString().PadLeft(2, '0')}{date.Day.ToString().PadLeft(2, '0')}-failed.csv";

      var filePath = Path.Combine(failedPath, fileName);

      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = false,
        NewLine = Environment.NewLine
      };

      try
      {
        var errors = new List<TransactionError>();
        foreach (var transaction in transactions)
          errors.Add(new(){ Message = transaction.ErrorMessage, Date = transaction.ErrorDate });

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, config))
        {
          int index = 0;
          foreach (var transaction in transactions)
          {
            var errorMessage = transaction.ErrorMessage;
            var errorDate = transaction.ErrorDate;

            csv.WriteRecord(transaction);
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
