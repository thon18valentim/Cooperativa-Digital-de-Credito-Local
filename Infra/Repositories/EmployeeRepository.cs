using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Maps;
using Bogus;
using CsvHelper;
using AdaCredit.Domain.Entities.Enums;
using BCrypt.Net;
using static BCrypt.Net.BCrypt;
using System.Collections;
using CsvHelper.Configuration;
using System.Reflection;

namespace AdaCredit.Infra.Repositories
{
  public sealed class EmployeeRepository
  {
    private static List<Employee> registeredEmployees;

    public static List<Employee> RegisteredEmployees
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

    private static void Load()
    {
      var fileName = "adaCredit_employee_database.csv";
      var filePath = Path.Combine(databasePath, fileName);

      RegisteredEmployees = new();

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
        RegisteredEmployees = records;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{ex.Message} ao ler arquivo {fileName}");
        RegisteredEmployees = new();
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
          csv.WriteRecords(RegisteredEmployees);
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
      => RegisteredEmployees.Count == 0;

    public static void Add(Employee employee)
      => RegisteredEmployees.Add(employee);

    public static Employee? Find(string userName)
      => RegisteredEmployees.FirstOrDefault(e => e.UserName == userName);

    public static List<Employee> GetActive()
      => RegisteredEmployees.FindAll(e => e.IsActive);

    public static int CountDisable()
    {
      return RegisteredEmployees.Count(e => !e.IsActive);
    } 
  }
}
