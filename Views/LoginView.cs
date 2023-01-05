using AdaCredit.Infra.Repositories;
using AdaCredit.Domain.UseCases;
using Spectre.Console;

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

        AnsiConsole.MarkupLine("── [darkorange]Login[/] ──────────────────────────────────────────────────────────────────────");
        var userName = AnsiConsole.Ask<string>("Entre com o seu [darkorange]login[/]:");
        var password = AnsiConsole.Prompt(new TextPrompt<string>("Entre com a sua [darkorange]senha[/]:").PromptStyle("red").Secret());

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
          isLogged = new DoLogin().Run(
            new StringUseCaseParameter[] { 
              new(){ ParameterName = "UserName", ParameterValue = userName },
              new(){ ParameterName = "Password", ParameterValue = password }
            });
        }
        
        if (!isLogged)
        {
          Console.WriteLine("\n");
          AnsiConsole.Write(new Markup("[bold red]Usuário ou senha incorreta![/]"));
          Thread.Sleep(2000);
        }

      } while (!isLogged);

      AnsiConsole.Status()
        .Start("[bold blue]Login efetuado com sucesso[/]", ctx =>
        {
          Console.WriteLine("\n");
          Thread.Sleep(2000);
          AnsiConsole.MarkupLine("[bold blue]LOG:[/] Carregando transações...");
          Thread.Sleep(2000);

          ctx.Status("[bold blue]Preparando ambiente dos funcionários[/]");
          ctx.Spinner(Spinner.Known.Balloon);
          ctx.SpinnerStyle(Style.Parse("blue"));

          AnsiConsole.MarkupLine("[bold blue]LOG:[/] Carregando clientes...");
          Thread.Sleep(2000);

          ctx.Status("[bold blue]Preparando ambiente de gerenciamento[/]");
          ctx.Spinner(Spinner.Known.Balloon);
          ctx.SpinnerStyle(Style.Parse("blue"));

          AnsiConsole.MarkupLine("[bold blue]LOG:[/] Carregando contas...");
          Thread.Sleep(2000);

          ctx.Status("[bold blue]Finalizando preparativos[/]");
          ctx.Spinner(Spinner.Known.Star2);
          ctx.SpinnerStyle(Style.Parse("blue"));
        });

      Console.WriteLine("\n");
      AnsiConsole.Write(new Markup("[bold green]Seja bem vindo ao sistema Ada Credit![/]"));
      Thread.Sleep(4000);
    }
  }
}
