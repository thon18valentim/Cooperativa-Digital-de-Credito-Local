using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Maps;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Infra.Repositories
{
  public sealed class AccountRepository
  {
    private static List<Account> registeredAccounts;

    public static List<Account> RegisteredAccounts
    {
      get => registeredAccounts;
      private set => registeredAccounts = value;
    }

    public static void Load()
    {
      var fileName = "adaCredit_account_database.csv";
      var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      var filePath = Path.Combine(desktopPath, fileName);

      RegisteredAccounts = new();

      if (!File.Exists(filePath))
      {
        File.Create(filePath);
        return;
      }

      try
      {
        List<Account> entities = new();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          NewLine = Environment.NewLine,
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<AccountMap>();
        var records = csv.GetRecords<Account>().ToList();
        RegisteredAccounts = records;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao ler arquivo {fileName}");
        RegisteredAccounts = new();
      }
    }

    public static bool Save()
    {
      var fileName = "adaCredit_account_database.csv";

      try
      {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, fileName);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          NewLine = Environment.NewLine,
        };

        using var writer = new StreamWriter(filePath);
        using (var csv = new CsvWriter(writer, config))
        {
          csv.Context.RegisterClassMap<AccountMap>();
          csv.WriteRecords(RegisteredAccounts);
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

    public static void Add(Account account)
      => RegisteredAccounts.Add(account);

    public static bool Exists(string number)
      => RegisteredAccounts.Find(a => a.Number.Replace("-", "") == number.Replace("-", "")) != null;

    public static Account? Find(string number)
      => RegisteredAccounts.Find(a => a.Number.Replace("-", "") == number.Replace("-", ""));

    public static bool IsEmpty()
      => RegisteredAccounts.Count == 0;

    public static int GetNextId()
    {
      if (RegisteredAccounts.Count == 0)
        return 1;

      return RegisteredAccounts[RegisteredAccounts.Count - 1].Id + 1;
    }

    public static Account? GetAccount(int id)
      => RegisteredAccounts.FirstOrDefault(x => x.Id == id);
  }
}
