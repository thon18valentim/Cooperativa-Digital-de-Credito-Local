using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;

namespace AdaCredit.Views
{
  public static class Login
  {
    public static void Show()
    {
      bool isLogged = false;

      do
      {
        Console.Clear();

        Console.WriteLine("Login: ");
        var userName = Console.ReadLine();

        Console.WriteLine("Senha: ");
        var password = Console.ReadLine();

        if (EmployeeRepository.IsEmpty())
        {
          if (userName.Equals("user", StringComparison.InvariantCultureIgnoreCase) &&
          password.Equals("pass", StringComparison.InvariantCultureIgnoreCase))
          {
            RegisterEmployees.Show();
            isLogged = true;
          }
        }
        else
        {
          isLogged = EmployeeRepository.Find(userName, password);
        }
        

      } while (!isLogged);

      Console.Clear();
    }
  }
}
