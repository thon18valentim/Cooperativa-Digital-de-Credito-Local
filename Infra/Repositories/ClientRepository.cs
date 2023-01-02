using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Maps;
using CsvHelper;
using CsvHelper.Configuration;

namespace AdaCredit.Infra.Repositories
{
  public sealed class ClientRepository
  {
    private static List<Client> registeredClients;

    public static List<Client> RegisteredClients
    {
      get => registeredClients;
      private set => registeredClients = value;
    }

    private static readonly string  databasePath;

    static ClientRepository()
    {
      databasePath = 
        Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
    }

    public static void Load()
    {
      var fileName = "adaCredit_client_database.csv";
      var filePath = Path.Combine(databasePath, fileName);

      RegisteredClients = new();

      if (!File.Exists(filePath))
      {
        File.Create(filePath);
        return;
      }

      try
      {
        List<Client> entities = new();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          NewLine = Environment.NewLine,
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<ClientMap>();
        var records = csv.GetRecords<Client>().ToList();
        RegisteredClients = records;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao ler arquivo {fileName}");
        RegisteredClients = new();
      }
    }

    public static bool Save()
    {
      var fileName = "adaCredit_client_database.csv";

      try
      {
        var filePath = Path.Combine(databasePath, fileName);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          NewLine = Environment.NewLine,
        };

        using var writer = new StreamWriter(filePath);
        using (var csv = new CsvWriter(writer, config))
        {
          csv.Context.RegisterClassMap<ClientMap>();
          csv.WriteRecords(RegisteredClients);
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

    public static bool IsEmpty()
      => RegisteredClients.Count == 0;

    public static void Add(Client client)
      => RegisteredClients.Add(client);

    public static Client? Find(string cpf)
      => RegisteredClients.FirstOrDefault(c => string.Equals(c.Cpf, cpf));

    private static Client? GetClientByAccountId(int id)
      => RegisteredClients.FirstOrDefault(c => c.ClientAccountId == id);

    public static bool IsActive(string accountNumber)
    {
      var account = AccountRepository.Find(accountNumber);
      var client = GetClientByAccountId(account.Id);

      return client.IsActive;
    }

    public static List<Client> GetActive()
      => RegisteredClients.FindAll(c => c.IsActive);

    public static List<Client> GetDisable()
      => RegisteredClients.FindAll(c => !c.IsActive);
  }
}
