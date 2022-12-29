using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;
using AdaCredit.Domain.UseCases;
using AdaCredit.Domain.Entities;

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

        var result = new DoPassVerification().Run(employee, oldPassword);

        if (!result)
        {
          Console.WriteLine("Senha incorreta! Tente novamente");
          Thread.Sleep(2500);
          Console.Clear();
          continue;
        }

        Console.WriteLine($"\nEntre com a nova senha de {employee}: ");
        var newPassword = Util.ReadLinePassword();

        changed = new DoPassChange().Run(employee, newPassword);

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

        disabled = new DoDisableEmployee().Run(employee);

      } while (!disabled);

      if (disabled)
      {
        Console.WriteLine($"\nFuncionário {employee} desativado com sucesso!");
        Thread.Sleep(2000);
      }
    }

    public static void ShowDisableClient()
    {
      var options = ClientRepository.RegisteredClients.Select(c => $"{c.Name} - {c.Cpf}").ToList();
      options.Add("Voltar");

      var client = "um Cliente";

      bool disabled = false;

      do
      {
        Console.Clear();

        Console.WriteLine($"| Desativar conta de {client} |");

        client = AnsiConsole.Prompt(Util.CreateSelectionList("\nSelecione um cliente listado abaixo:", options));

        if (string.Equals(client, "Voltar"))
          break;

        var cpf = client.GetCpf();
        disabled = new DoDisableClient().Run(cpf);

      } while (!disabled);

      if (disabled)
      {
        Console.WriteLine($"\nCliente {client} desativado com sucesso!");
        Thread.Sleep(2000);
      }
    }

    public static void ShowClients()
    {
      var options = ClientRepository.RegisteredClients.Select(c => $"{c.Name} - {c.Cpf}").ToList();
      options.Add("Voltar");

      var client = "um Cliente";

      Client? clientObj = new();

      bool selected = false;

      do
      {
        Console.Clear();

        Console.WriteLine($"| Desativar conta de {client} |");

        client = AnsiConsole.Prompt(Util.CreateSelectionList("\nSelecione um cliente listado abaixo:", options));

        if (string.Equals(client, "Voltar"))
          break;

        var cpf = client.GetCpf();
        clientObj = ClientRepository.Find(cpf);

        if (clientObj == null)
          selected = false;
        else
          selected = true;

      } while (!selected);

      if (selected)
      {
        ShowClientInfo(clientObj);
      }
    }

    private static void ShowClientInfo(Client? client)
    {
      if (client == null)
        return;

      Console.Clear();

      var table = new Table();

      table.AddColumn("Nome");
      table.AddColumn("Cpf");
      table.AddColumn("Conta");
      table.AddColumn("Agência");
      table.AddColumn("Saldo");

      var account = AccountRepository.GetAccount(client.ClientAccountId);

      if (account == null)
        return;

      table.AddRow(
        client.Name, 
        client.Cpf,
        account.Number,
        account.AgencyNumber,
        string.Format("{0:C}", account.Balance.ToString()));

      AnsiConsole.Write(table);

      Console.WriteLine("Pressione ENTER para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }

    public static void ShowEditClient()
    {
      var options = ClientRepository.RegisteredClients.Select(c => $"{c.Name} - {c.Cpf}").ToList();
      options.Add("Voltar");

      var client = "um Cliente";

      Client? clientObj = new();

      bool selected = false;

      do
      {
        Console.Clear();

        Console.WriteLine($"| Editar cadastro de {client} |");

        client = AnsiConsole.Prompt(Util.CreateSelectionList("\nSelecione um cliente listado abaixo:", options));

        if (string.Equals(client, "Voltar"))
          break;

        var cpf = client.GetCpf();
        clientObj = ClientRepository.Find(cpf);

        if (clientObj == null)
          selected = false;
        else
          selected = true;

      } while (!selected);

      if (selected)
      {
        ShowEditClientInfo(clientObj);
      }
    }

    private static void ShowEditClientInfo(Client? client)
    {
      if (client == null)
        return;

      var table = new Table();

      table.AddColumn("Nome");
      table.AddColumn("Cpf");
      table.AddColumn("Conta");

      var account = AccountRepository.GetAccount(client.ClientAccountId);

      if (account == null)
        return;

      bool completed = false;

      do
      {
        Console.Clear();

        table.AddRow(
        client.Name,
        client.Cpf,
        account.Number
        );

        AnsiConsole.Write(table);

        Console.WriteLine("| Entre com os novos dados do cliente abaixo |");

        Console.WriteLine("\nNome completo:");
        var fullName = Console.ReadLine();

        Console.WriteLine("Cpf:");
        var cpf = Console.ReadLine();

        // validar cpf

      } while(!completed);

      
    }
  }
}
