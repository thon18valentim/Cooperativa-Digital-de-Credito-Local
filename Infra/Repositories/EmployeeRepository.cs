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

namespace AdaCredit.Infra.Repositories
{
  public sealed class EmployeeRepository
  {
    private static List<Employee> registeredEmployees;

    public static List<Employee> RegisteredEmployees
    {
      get => registeredEmployees;
      private set => registeredEmployees = value;
    }

    static EmployeeRepository()
    {
      LoadEmployees();
    }

    private static void LoadEmployees()
    {
      var fileName = "adaCredit_employee_database.csv";
      var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      var filePath = Path.Combine(desktopPath, fileName);

      RegisteredEmployees = new();

      if (!File.Exists(filePath))
      {
        File.Create(filePath);
        return;
      }

      try
      {
        List<Employee> entities = new();

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
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

    private static bool Save()
    {
      var fileName = "adaCredit_employee_database.csv";

      try
      {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, fileName);

        using var writer = new StreamWriter(filePath);
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
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

    public static bool Add(string password)
    {
      Employee employee;
      bool IsUniqueName = false;
      do
      {
        // criar novo employee
        employee = new Faker<Employee>()
          .CustomInstantiator(f => new Employee())
          .RuleFor(e => e.Gender, f => f.PickRandom<Gender>())
          .RuleFor(e => e.FirstName, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
          .RuleFor(e => e.UserName, (f, e) => f.Internet.UserName(e.FirstName, e.LastName))
          .RuleFor(e => e.PasswordSalt, f => GenerateSalt())
          .FinishWith((f, e) =>
          {
            // hash da senha
            var hashedPassword = HashPassword(password, e.PasswordSalt);
            e.PasswordHash = hashedPassword;
          });

        // conferindo se nome é unico
        if (!RegisteredEmployees.Any(e => e.UserName == employee.UserName))
          IsUniqueName = true;

      } while (!IsUniqueName);

      employee.IsActive = true;
      employee.LastLogin = DateTime.Now.ToString("dd/MM/yyyy/HH:mm");

      // adicionar employee
      RegisteredEmployees.Add(employee);

      Console.WriteLine($"Funcionário {employee.UserName} criado com sucesso!");

      // atualizar base de dados
      return Save();
    }

    public static bool IsEmpty()
      => RegisteredEmployees.Count == 0;

    public static Employee? Find(string userName)
    {
      return RegisteredEmployees.FirstOrDefault(e => e.UserName == userName);
    }

    public static bool Login(string userName, string password)
    {
      var employee = Find(userName);

      if (employee == default)
        return false;

      if (!employee.IsActive)
        return false;

      var hashPassword = HashPassword(password, employee.PasswordSalt);

      if (hashPassword == employee.PasswordHash)
      {
        employee.LastLogin = DateTime.Now.ToString("dd/MM/yyyy/HH:mm");
        Save();
        return true;
      }

      return false;
    }

    public static bool VerifyPassword(string userName, string password)
    {
      var employee = Find(userName);

      if (employee == default)
        return false;

      var hashPassword = HashPassword(password, employee.PasswordSalt);

      if (hashPassword == employee.PasswordHash)
        return true;

      return false;
    }

    public static bool ChangePassword(string userName, string newPassword)
    {
      var employee = Find(userName);

      if (employee == default)
        return false;

      employee.PasswordSalt = GenerateSalt();
      var hashedPassword = HashPassword(newPassword, employee.PasswordSalt);
      employee.PasswordHash = hashedPassword;

      return Save();
    }

    public static bool Disable(string userName)
    {
      var employee = Find(userName);

      if (employee == default)
        return false;

      employee.IsActive = false;

      return Save();
    }
  }
}
