using System.Globalization;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Maps;
using CsvHelper;
using CsvHelper.Configuration;
using System.Reflection;

namespace AdaCredit.Infra.Repositories
{
  public sealed class EmployeeRepository
  {
    private static List<Employee> registeredEmployees;

    public static IEnumerable<Employee> RegisteredEmployees
    {
      get => registeredEmployees;
      private set => registeredEmployees = (List<Employee>)value;
    }

    private static readonly string databasePath;

    static EmployeeRepository()
    {
      databasePath =
        Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;
      Load();
    }

    public static List<Employee> Get()
      => registeredEmployees;

    private static void Load()
    {
      var fileName = "adaCredit_employee_database.csv";
      var filePath = Path.Combine(databasePath, fileName);

      registeredEmployees = new();

      if (!File.Exists(filePath))
      {
        File.Create(filePath);
        return;
      }

      try
      {
        List<Employee> entities = new();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
          NewLine = Environment.NewLine,
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<EmployeeMap>();
        var records = csv.GetRecords<Employee>().ToList();
        registeredEmployees = records;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao ler arquivo {fileName}");
        registeredEmployees = new();
      }
    }

    public static bool Save()
    {
      var fileName = "adaCredit_employee_database.csv";

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
          csv.Context.RegisterClassMap<EmployeeMap>();
          csv.WriteRecords(registeredEmployees);
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
      => registeredEmployees.Count == 0;

    public static void Add(Employee employee)
      => registeredEmployees.Add(employee);

    public static Employee? Find(string userName)
      => RegisteredEmployees.FirstOrDefault(e => e.UserName == userName);

    public static List<Employee> GetActive()
      => registeredEmployees.FindAll(e => e.IsActive);

    public static int CountDisable()
    {
      return registeredEmployees.Count(e => !e.IsActive);
    } 
  }
}
