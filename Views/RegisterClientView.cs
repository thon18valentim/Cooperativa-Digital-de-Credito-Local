using AdaCredit.Domain.UseCases;
using Spectre.Console;

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

        AnsiConsole.MarkupLine("── [darkorange]Cadastrar novo Cliente[/] ─────────────────────────────────────────────────────");

        isRegistered = new DoCreateClient().Run();

      } while (!isRegistered);

      Console.WriteLine("\n");
      AnsiConsole.Write(new Markup("[bold green]Cliente cadastrado com sucesso![/]"));
      Thread.Sleep(2500);
    }
  }
}
