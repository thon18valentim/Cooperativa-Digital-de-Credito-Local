using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Infra.Repositories;

namespace AdaCredit.Views
{
  public static class ReportView
  {
    public static void ShowActiveEmployees()
    {
      Console.Clear();

      var table = new Table();

      table.AddColumn("Funcionários");
      table.AddColumn("Último Login");

      foreach (Employee e in EmployeeRepository.GetActive())
        table.AddRow(e.UserName, e.LastLogin);

      AnsiConsole.Write(table);

      Console.WriteLine("Pressione ENTER para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }
  }
}
