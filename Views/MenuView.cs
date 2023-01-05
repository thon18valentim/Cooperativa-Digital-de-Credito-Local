
using ConsoleTools;
using Spectre.Console;

namespace AdaCredit.Views
{
  public static class MenuView
  {
    public static void Show()
    {
      var subClient = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Cadastrar Novo Cliente", () => RegisterClientView.Show())
       .Add("Consultar os Dados de um Cliente existente", () => SelectionView.ShowClients())
       .Add("Alterar o Cadastro de um Cliente existente", () => SelectionView.ShowEditClient())
       .Add("Desativar Cadastro de um Cliente existente", () => SelectionView.ShowDisableClient())
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "> ";
         config.EnableFilter = false;
         config.Title = "Ada Credit / Clientes";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
         config.SelectedItemBackgroundColor = Color.Orange3;
         config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
       });

      var subEmployee = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Cadastrar Novo Funcionário", () => RegisterEmployeesView.Show())
       .Add("Alterar Senha de um Funcionário existente", () => SelectionView.ShowChangePassword())
       .Add("Desativar Cadastro de um Funcionário existente", () => SelectionView.ShowDisableEmployee())
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "> ";
         config.EnableFilter = false;
         config.Title = "Ada Credit / Funcionários";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
         config.SelectedItemBackgroundColor = Color.Orange3;
         config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
       });

      var subTransactions = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Processar Transações (Reconciliação Bancária)", () => ProcessTransactionView.Show())
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "> ";
         config.EnableFilter = false;
         config.Title = "Ada Credit / Transações";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
         config.SelectedItemBackgroundColor = Color.Orange3;
         config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
       });

      var subReports = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Exibir Todos os Clientes Ativos com seus Respectivos Saldos", () => ReportView.ShowActiveClients())
       .Add("Exibir Todos os Clientes Inativos", () => ReportView.ShowDisabledClients())
       .Add("Exibir Todos os Funcionários Ativos e sua Última Data e Hora de Login", () => ReportView.ShowActiveEmployees())
       .Add("Exibir Transações com Erro (Detalhes da transação e do Erro)", () => ReportView.ShowFailedTransactions())
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "> ";
         config.EnableFilter = false;
         config.Title = "Ada Credit / Relatórios";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
         config.SelectedItemBackgroundColor = Color.Orange3;
         config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
       });

      var subHelp = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Sistema", () => HelpView.ShowSystem())
       .Add("Transações", () => HelpView.ShowTransactions())
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "> ";
         config.EnableFilter = false;
         config.Title = "Ada Credit / Relatórios";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
         config.SelectedItemBackgroundColor = Color.Orange3;
         config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
       });

      var menu = new ConsoleMenu(Array.Empty<string>(), level: 2)
        .Add("Clientes", () => subClient.Show())
        .Add("Funcionários", () => subEmployee.Show())
        .Add("Transações", () => subTransactions.Show())
        .Add("Relatórios", () => subReports.Show())
        .Add("Ajuda", () => subHelp.Show())
        .Add("Exit", () => Quit())
        .Configure(config =>
        {
          config.Selector = "> ";
          config.EnableFilter = false;
          config.Title = "Ada Credit";
          config.EnableWriteTitle = false;
          config.EnableBreadcrumb = true;
          config.SelectedItemBackgroundColor = Color.Orange3;
          config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
        });

      menu.Show();
    }

    private static void Quit()
    {
      AnsiConsole.Write(new Markup("[bold red]Sistema encerrado, volte sempre![/]"));
      Console.WriteLine("\n");

      Environment.Exit(0);
    }
  }
}
