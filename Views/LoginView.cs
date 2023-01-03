using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;
using AdaCredit.Domain.UseCases;

namespace AdaCredit.Views
{
  public static class LoginView
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
        var password = Util.ReadLinePassword();

        if (EmployeeRepository.IsEmpty())
        {
          if (userName.Equals("user", StringComparison.InvariantCultureIgnoreCase) &&
          password.Equals("pass", StringComparison.InvariantCultureIgnoreCase))
          {
            RegisterEmployeesView.Show();
            isLogged = true;
          }
        }
        else
        {
          var tuple = new StringUseCaseParameter[] { ("UserName", userName), ("Password", password) };
          isLogged = new DoLogin().Run(tuple);
        }
        
        if (!isLogged)
        {
          Console.WriteLine("Usuário ou senha incorreta!");
          Thread.Sleep(2000);
        }

      } while (!isLogged);

      Console.WriteLine("Login efetuado com sucesso!");
      Thread.Sleep(2000);
    }
  }
}
