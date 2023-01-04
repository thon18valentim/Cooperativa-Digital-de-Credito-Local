using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;

namespace AdaCredit.Views
{
  public static class ReportView
  {
    public static void ShowActiveEmployees()
    {
      Console.Clear();

      var table = new Table().Format();

      table.AddColumn("Funcionários");
      table.AddColumn("[deepskyblue1]Usuários[/]");
      table.AddColumn("[grey58]Último Login[/]");

      var list = EmployeeRepository.GetActive();

      AnsiConsole.MarkupLine("─────────────────────────────────────────────── [darkorange]Funcionários Ativos[/] ───────────────────────────────────────────────");

      if (list.Count == 0)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup("[bold red]Nenhum funcionário ativo encontrado[/]"));
        Console.WriteLine("\n");
      }
      else
      {
        foreach (Employee e in list)
          table.AddRow($"{e.FirstName} {e.LastName}", $"[mediumturquoise]{e.UserName}[/]", $"[grey70]{e.LastLogin}[/]");

        AnsiConsole.Write(table);
      }

      AnsiConsole.MarkupLine("Pressione [darkorange]ENTER[/] para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }

    public static void ShowActiveClients()
    {
      Console.Clear();

      var table = new Table().Format();

      table.AddColumn("Clientes");
      table.AddColumn("[deepskyblue1]Contas[/]");
      table.AddColumn("[green]Saldo (R$)[/]");

      var list = ClientRepository.GetActive();

      if (list.Count == 0)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup("[bold red]Nenhum cliente ativo encontrado[/]"));
        Console.WriteLine("\n");
      }
      else
      {
        foreach (Client c in list)
        {
          var account = AccountRepository.GetAccount(c.ClientAccountId);

          if (account == null)
            account = new Account();

          table.AddRow(c.Name, $"[mediumturquoise]{account.Number}[/]", $"[springgreen4]{string.Format("{0:C}", account.Balance)}[/]");
        }

        AnsiConsole.MarkupLine("────────────────────────────────────────────────── [darkorange]Clientes Ativos[/] ──────────────────────────────────────────────────");
        AnsiConsole.Write(table);
      }

      AnsiConsole.MarkupLine("Pressione [darkorange]ENTER[/] para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }

    public static void ShowDisabledClients()
    {
      Console.Clear();

      var table = new Table().Format();

      table.AddColumn("Clientes");
      table.AddColumn("[grey58]Cpf[/]");

      var list = ClientRepository.GetDisable();

      if (list.Count == 0)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup("[bold red]Nenhum cliente inativo encontrado![/]"));
        Console.WriteLine("\n");
      }
      else
      {
        foreach (Client c in list)
          table.AddRow(c.Name, $"[grey70]{c.Cpf}[/]");

        AnsiConsole.MarkupLine("───────────────────────────────────────────── [darkorange]Clientes Desativados[/] ─────────────────────────────────────────────");
        AnsiConsole.Write(table);
      }

      AnsiConsole.MarkupLine("Pressione [darkorange]ENTER[/] para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }

    public static void ShowFailedTransactions()
    {
      Console.Clear();

      var table = new Table().Format();

      table.AddColumn("Banco de Origem & Conta");
      table.AddColumn("Banco de Destino & Conta");
      table.AddColumn("[green]Valor (R$)[/] & [grey58]Tipo[/]");
      table.AddColumn("[red]Erro[/]");

      var transactions = TransactionsRepository.LoadFailed();

      if (transactions?.Length == 0)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup("[bold red]Nenhuma transação com falha encontrada[/]"));
        Console.WriteLine("\n");
      }
      else
      {
        for (int i = 0; i < transactions?.Length; i++)
        {
          var entry = transactions[i].Entry switch
          {
            0 => "Débito",
            _ => "Crédito"
          };

          table.AddRow(
            $"{transactions[i].OriginBankCode} ({transactions[i].OriginBankAgency}) - {transactions[i].OriginBankAccount}",
            $"{transactions[i].DestinationBankCode} ({transactions[i].DestinationBankAgency}) - {transactions[i].DestinationBankAccount}",
            $"[springgreen4]{string.Format("{0:C}", transactions[i].Value)}[/] - [grey70]{transactions[i].Type} ({entry})[/]",
            $"[maroon]{transactions[i].ErrorMessage}.[/]"
            );
        }

        AnsiConsole.MarkupLine("─────────────────────────────────────────── [darkorange]Transações que falharam[/] ───────────────────────────────────────────");
        AnsiConsole.Write(table);
      }

      AnsiConsole.MarkupLine("Pressione [darkorange]ENTER[/] para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }
  }
}
