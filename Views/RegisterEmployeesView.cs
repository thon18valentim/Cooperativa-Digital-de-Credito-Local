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
  public static class RegisterEmployeesView
  {
    public static void Show()
    {
      bool isRegistered;

      do
      {
        Console.Clear();

        Console.WriteLine("| Cadastrar novo Funcionário |");
        Console.WriteLine("Senha do novo funcionário: ");
        var password = Util.ReadLinePassword();

        //var tuple = new StringUseCaseParameter[] { ("Password", password) };
        isRegistered = new DoCreateEmployee().Run(
            new StringUseCaseParameter[] {
              new(){ ParameterName = "Password", ParameterValue = password }
            });

      } while (!isRegistered);

      Console.WriteLine("Funcionário cadastrado com sucesso!");
      Thread.Sleep(2000);
    }
  }
}
