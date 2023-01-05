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

      var employee = string.Empty;

      bool changed = false;

      do
      {
        Console.Clear();

        AnsiConsole.MarkupLine("── [darkorange]Alterar senha de Funcionário[/] ───────────────────────────────────────────────");

        employee = AnsiConsole.Prompt(Util.CreateSelectionList("Selecione um [blue]funcionário[/] listado abaixo:", options));

        if (string.Equals(employee, "Voltar"))
          break;

        var oldPassword = AnsiConsole.Ask<string>($"Entre com a senha atual de [darkorange]{employee}[/]:");

        var result = new DoPassVerification().Run(
            new StringUseCaseParameter[] {
              new(){ ParameterName = "UserName", ParameterValue = employee },
              new(){ ParameterName = "OldPassword", ParameterValue = oldPassword },
            });

        if (!result)
        {
          Console.WriteLine("\n");
          AnsiConsole.Write(new Markup("[bold red]Senha incorreta, tente novamente![/]"));
          Thread.Sleep(2500);
          continue;
        }

        var newPassword = AnsiConsole.Ask<string>($"Entre com a nova senha de [darkorange]{employee}[/]:");

        changed = new DoPassChange().Run(
            new StringUseCaseParameter[] {
              new(){ ParameterName = "UserName", ParameterValue = employee },
              new(){ ParameterName = "NewPassword", ParameterValue = newPassword },
            });

      } while (!changed);

      if (changed)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup($"[bold green]Senha de {employee} alterada com sucesso![/]"));
        Thread.Sleep(2000);
      }     
    }

    public static void ShowDisableEmployee()
    {
      var list = EmployeeRepository.GetActive();
      var options = list.Select(e => e.UserName).ToList();
      options.Add("Voltar");

      var employee = string.Empty;

      bool disabled = false;

      do
      {
        Console.Clear();

        AnsiConsole.MarkupLine("── [darkorange]Desativar conta de Funcionário[/] ─────────────────────────────────────────────");

        employee = AnsiConsole.Prompt(Util.CreateSelectionList("Selecione um [blue]funcionário[/] listado abaixo:", options));

        if (string.Equals(employee, "Voltar"))
          break;

        disabled = new DoDisableEmployee().Run(
            new StringUseCaseParameter[] {
              new(){ ParameterName = "UserName", ParameterValue = employee },
            });

        if (!disabled)
        {
          Console.WriteLine("\n");
          AnsiConsole.Write(new Markup("[bold red]Falha ao desativar funcionário. Deve haver pelo menos um funcionário ativo no sistema.[/]"));
        }

      } while (!disabled);

      if (disabled)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup($"[bold green]Funcionário {employee} desativado com sucesso![/]"));
        Thread.Sleep(2000);
      }
    }

    public static void ShowDisableClient()
    {
      var list = ClientRepository.GetActive();
      var options = list.Select(c => $"{c.Name} - {c.Cpf}").ToList();
      options.Add("Voltar");

      var client = string.Empty;

      bool disabled = false;

      do
      {
        Console.Clear();

        AnsiConsole.MarkupLine("── [darkorange]Desativar conta de Cliente[/] ─────────────────────────────────────────────────");

        client = AnsiConsole.Prompt(Util.CreateSelectionList("Selecione um [blue]cliente[/] listado abaixo:", options));

        if (string.Equals(client, "Voltar"))
          break;

        disabled = new DoDisableClient().Run(
            new StringUseCaseParameter[] {
              new(){ ParameterName = "Cpf", ParameterValue = client.GetCpf() },
            });

      } while (!disabled);

      if (disabled)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup($"[bold green]Cliente {client} desativado com sucesso![/]"));
        Thread.Sleep(2000);
      }
    }

    public static void ShowClients()
    {
      var options = ClientRepository.RegisteredClients.Select(c => $"{c.Name} - {c.Cpf}").ToList();
      options.Add("Voltar");

      var client = string.Empty;

      Client? clientObj = new();

      bool selected = false;

      do
      {
        Console.Clear();

        AnsiConsole.MarkupLine("── [darkorange]Consultar dados de Cliente[/] ─────────────────────────────────────────────────");

        client = AnsiConsole.Prompt(Util.CreateSelectionList("Selecione um [blue]cliente[/] listado abaixo:", options));

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

      var table = new Table().Format();

      table.AddColumn("Nome");
      table.AddColumn("[grey58]Cpf[/]");
      table.AddColumn("[lightslateblue]Conta[/]");
      table.AddColumn("[lightslateblue]Agência[/]");
      table.AddColumn("[green]Saldo[/]");

      var account = AccountRepository.GetAccount(client.ClientAccountId);

      if (account == null)
        return;

      table.AddRow(
        client.Name, 
        $"[grey70]{client.Cpf}[/]",
        $"[skyblue2]{account.Number}[/]",
        $"[skyblue2]{account.AgencyNumber}[/]",
        $"[springgreen4]{string.Format("{0:C}", account.Balance)}[/]");

      AnsiConsole.MarkupLine("───────────────────────────────────────────── [darkorange]Infos doo Cliente[/] ─────────────────────────────────────────────");
      AnsiConsole.Write(table);

      AnsiConsole.MarkupLine("Pressione [darkorange]ENTER[/] para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }

    public static void ShowEditClient()
    {
      var options = ClientRepository.RegisteredClients.Select(c => $"{c.Name} - {c.Cpf}").ToList();
      options.Add("Voltar");

      var client = string.Empty;

      Client? clientObj = new();

      bool selected = false;

      do
      {
        Console.Clear();

        AnsiConsole.MarkupLine("── [darkorange]Editar cadastro de Cliente[/] ─────────────────────────────────────────────────");

        client = AnsiConsole.Prompt(Util.CreateSelectionList("Selecione um [blue]cliente[/] listado abaixo:", options));

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

      var enableOptions = new List<string>() { "Ativar", "Desativar" };

      var account = AccountRepository.GetAccount(client.ClientAccountId);

      if (account == null)
        return;

      bool completed = false;

      do
      {
        Console.Clear();

        var table = new Table();

        table.AddColumn("Nome");
        table.AddColumn("[grey58]Cpf[/]");
        table.AddColumn("[lightslateblue]Conta[/]");

        table.AddRow(
          client.Name,
          $"[grey70]{client.Cpf}[/]",
          $"[skyblue2]{account.Number}[/]"
        );

        AnsiConsole.MarkupLine("────────────────────────────────────────────────── [darkorange]Cliente Infos[/] ──────────────────────────────────────────────────");
        AnsiConsole.Write(table);

        Console.WriteLine("\n");
        AnsiConsole.MarkupLine("── [darkorange]Novos dados do cliente[/] ──────────────────────────────────────────────────────────");

        var fullName = AnsiConsole.Ask<string>("Entre com o seu [bold blue]nome completo[/]:");
        var cpf = AnsiConsole.Ask<string>("Entre com o seu [bold blue]cpf[/]:");
        var phone = AnsiConsole.Ask<string>("Entre com o seu [bold blue]telefone[/]:");
        var country = AnsiConsole.Ask<string>("Entre com o seu [bold blue]país de residência[/]:");
        var city = AnsiConsole.Ask<string>("Entre com a sua [bold blue]cidade de residência[/]:");
        var streetAddress = AnsiConsole.Ask<string>("Entre com o [bold blue]endereço de rua[/]:");

        Console.WriteLine("\n");
        var activeConfig = AnsiConsole.Prompt(Util.CreateSelectionList("[bold blue]Ative[/] ou [bold red]desative[/] o cliente", enableOptions));
        var activeConfigSelected = activeConfig switch
        {
          "Ativar" => true,
          _ => false
        };

        completed = new DoEditClient().Run(
            new StringUseCaseParameter[] {
              new(){ ParameterName = "Name", ParameterValue = fullName },
              new(){ ParameterName = "Cpf", ParameterValue = cpf },
              new(){ ParameterName = "OldCpf", ParameterValue = client.Cpf },
              new(){ ParameterName = "Phone", ParameterValue = phone },
              new(){ ParameterName = "Country", ParameterValue = country },
              new(){ ParameterName = "City", ParameterValue = city },
              new(){ ParameterName = "StreetAddress", ParameterValue = streetAddress },
              new(){ ParameterName = "ActiveConfigSelected", ParameterValue = activeConfigSelected.ToString() },
            });

      } while(!completed);

      AnsiConsole.WriteLine("[bold green]Cliente atualizado com sucesso![/]");
      Thread.Sleep(2500);
    }
  }
}
