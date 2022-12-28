using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Enums;
using Bogus;
using static BCrypt.Net.BCrypt;

namespace AdaCredit.Domain.UseCases
{
  public class DoCreateEmployee : IUseCase
  {
    public bool Run(string param1, string param2)
    {
      throw new NotImplementedException();
    }

    public bool Run(string password)
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
        if (!EmployeeRepository.RegisteredEmployees.Any(e => e.UserName == employee.UserName))
          IsUniqueName = true;

      } while (!IsUniqueName);

      employee.IsActive = true;
      employee.LastLogin = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

      // adicionar employee
      EmployeeRepository.Add(employee);

      Console.WriteLine($"Funcionário {employee.UserName} criado com sucesso!");

      // atualizar base de dados
      return EmployeeRepository.Save();
    }
  }
}
