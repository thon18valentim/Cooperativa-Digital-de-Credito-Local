using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;

namespace AdaCredit.Views
{
  public static class SelectionView
  {
    public static void ShowChangePassword()
    {
      var options = EmployeeRepository.RegisteredEmployees.Select(e => e.UserName).ToList();
      options.Add("Voltar");

      var employee = "um Funcionário";

      bool changed = false;

      do
      {
        Console.Clear();

        Console.WriteLine($"| Alterar senha de {employee} |");

        employee = AnsiConsole.Prompt(Util.CreateSelectionList("\nSelecione um funcionário listado abaixo:", options));

        if (string.Equals(employee, "Voltar"))
          break;

        Console.WriteLine($"Senha antiga de {employee}: ");
        var oldPassword = Util.ReadLinePassword();

        var result = EmployeeRepository.VerifyPassword(employee, oldPassword);

        if (!result)
        {
          Console.WriteLine("Senha incorreta! Tente novamente");
          Thread.Sleep(2500);
          Console.Clear();
          continue;
        }

        Console.WriteLine($"\nEntre com a nova senha de {employee}: ");
        var newPassword = Util.ReadLinePassword();

        changed = EmployeeRepository.ChangePassword(employee, newPassword);

      } while (!changed);

      if (changed)
      {
        Console.WriteLine($"\nSenha de {employee} alterada com sucesso!");
        Thread.Sleep(2000);
      }     
    }

    public static void ShowDisableEmployee()
    {
      var options = EmployeeRepository.RegisteredEmployees.Select(e => e.UserName).ToList();
      options.Add("Voltar");

      var employee = "um Funcionário";

      bool disabled = false;

      do
      {
        Console.Clear();

        Console.WriteLine($"| Desativar conta de {employee} |");

        employee = AnsiConsole.Prompt(Util.CreateSelectionList("\nSelecione um funcionário listado abaixo:", options));

        if (string.Equals(employee, "Voltar"))
          break;

        disabled = EmployeeRepository.Disable(employee);

      } while (!disabled);

      if (disabled)
      {
        Console.WriteLine($"\nFuncionário {employee} desativado com sucesso!");
        Thread.Sleep(2000);
      }
    }
  }
}
