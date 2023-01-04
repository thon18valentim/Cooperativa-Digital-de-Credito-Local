using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;
using AdaCredit.Domain.UseCases;
using Spectre.Console;

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

        AnsiConsole.MarkupLine("── [darkorange]Cadastrar novo Funcionário[/] ─────────────────────────────────────────────────");
        var password = AnsiConsole.Prompt(new TextPrompt<string>("Entre com a [darkorange]senha[/] do novo funcionário:").PromptStyle("red").Secret());

        isRegistered = new DoCreateEmployee().Run(
            new StringUseCaseParameter[] {
              new(){ ParameterName = "Password", ParameterValue = password }
            });

      } while (!isRegistered);

      AnsiConsole.Write(new Markup("[bold green]Funcionário cadastrado com sucesso![/]"));
      Thread.Sleep(2000);
    }
  }
}
