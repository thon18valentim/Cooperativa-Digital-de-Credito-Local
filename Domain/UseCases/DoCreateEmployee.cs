using AdaCredit.Infra.Repositories;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Enums;
using Bogus;
using static BCrypt.Net.BCrypt;
using AdaCredit.Utils;

namespace AdaCredit.Domain.UseCases
{
  public class DoCreateEmployee : IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters)
    {
      var password = parameters.FirstOrDefault(x => x.ParameterName == "Password").ToStringValue();

      Employee employee;
      bool IsUniqueName = false;
      do
      {
        employee = new Faker<Employee>()
          .CustomInstantiator(f => new Employee())
          .RuleFor(e => e.Gender, f => f.PickRandom<Gender>())
          .RuleFor(e => e.FirstName, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
          .RuleFor(e => e.UserName, (f, e) => f.Internet.UserName(e.FirstName, e.LastName))
          .RuleFor(e => e.PasswordSalt, f => GenerateSalt())
          .FinishWith((f, e) =>
          {
            var hashedPassword = HashPassword(password, e.PasswordSalt);
            e.PasswordHash = hashedPassword;
          });

        if (!EmployeeRepository.RegisteredEmployees.Any(e => e.UserName == employee.UserName))
          IsUniqueName = true;

      } while (!IsUniqueName);

      employee.IsActive = true;
      employee.LastLogin = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

      EmployeeRepository.Add(employee);

      Console.WriteLine($"Funcionário {employee.UserName} criado com sucesso!");

      return EmployeeRepository.Save();
    }
  }
}
