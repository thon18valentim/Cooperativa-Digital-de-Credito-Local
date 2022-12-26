using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;

namespace AdaCredit.Views
{
  public static class RegisterEmployees
  {
    public static void Show()
    {
      bool isRegistered;

      do
      {
        Console.Clear();

        Console.WriteLine("Senha do novo funcionário: ");
        var password = Console.ReadLine();

        isRegistered = EmployeeRepository.Add(password);

      } while (!isRegistered);

      Console.WriteLine($"Funcionário cadastrado com sucesso!");
    }
  }
}
