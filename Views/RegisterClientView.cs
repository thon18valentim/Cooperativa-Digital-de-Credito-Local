using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.UseCases;

namespace AdaCredit.Views
{
  public static class RegisterClientView
  {
    public static void Show()
    {
      bool isRegistered;

      do
      {
        Console.Clear();

        Console.WriteLine("| Cadastrar novo Cliente |");

        isRegistered = new DoCreateClient().Run();

      } while (!isRegistered);

      Console.WriteLine("Cliente cadastrado com sucesso!");
      Thread.Sleep(2500);
    }
  }
}
