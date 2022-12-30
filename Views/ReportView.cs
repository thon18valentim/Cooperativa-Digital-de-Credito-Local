﻿using Spectre.Console;
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

      var list = EmployeeRepository.GetActive();

      if (list.Count == 0)
      {
        Console.WriteLine("Nenhum funcionário ativo encontrado\n");
      }
      else
      {
        foreach (Employee e in list)
          table.AddRow(e.UserName, e.LastLogin);

        AnsiConsole.Write(table);
      }

      Console.WriteLine("Pressione ENTER para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }

    public static void ShowActiveClients()
    {
      Console.Clear();

      var table = new Table();

      table.AddColumn("Clientes");
      table.AddColumn("Contas");
      table.AddColumn("Saldo");

      var list = ClientRepository.GetActive();

      if (list.Count == 0)
      {
        Console.WriteLine("Nenhum cliente ativo encontrado\n");
      }
      else
      {
        foreach (Client c in list)
        {
          var account = AccountRepository.GetAccount(c.ClientAccountId);

          if (account == null)
            account = new Account();

          table.AddRow(c.Name, account.Number, string.Format("{0:C}", account.Balance.ToString()));
        }

        AnsiConsole.Write(table);
      }

      Console.WriteLine("Pressione ENTER para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }

    public static void ShowDisabledClients()
    {
      Console.Clear();

      var table = new Table();

      table.AddColumn("Clientes");
      table.AddColumn("Cpf");

      var list = ClientRepository.GetDisable();

      if (list.Count == 0)
      {
        Console.WriteLine("Nenhum cliente inativo encontrado\n");
      }
      else
      {
        foreach (Client c in list)
          table.AddRow(c.Name, c.Cpf);

        AnsiConsole.Write(table);
      }

      Console.WriteLine("Pressione ENTER para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }
  }
}
