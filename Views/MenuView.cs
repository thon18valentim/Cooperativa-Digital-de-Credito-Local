
using ConsoleTools;

namespace AdaCredit.Views
{
  public static class MenuView
  {
    public static void Show()
    {
      var subClient = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Cadastrar Novo Cliente", () => RegisterClientView.Show())
       .Add("Consultar os Dados de um Cliente existente", () => SelectionView.ShowClients())
       .Add("Alterar o Cadastro de um Cliente existente", () => Console.WriteLine("Sub_Three"))
       .Add("Desativar Cadastro de um Cliente existente", () => SelectionView.ShowDisableClient())
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "--> ";
         config.EnableFilter = false;
         config.Title = "Clientes";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
       });

      var subEmployee = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Cadastrar Novo Funcionário", () => RegisterEmployeesView.Show())
       .Add("Alterar Senha de um Funcionário existente", () => SelectionView.ShowChangePassword())
       .Add("Desativar Cadastro de um Funcionário existente", () => SelectionView.ShowDisableEmployee())
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "--> ";
         config.EnableFilter = false;
         config.Title = "Funcionários";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
       });

      var subTransactions = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Processar Transações (Reconciliação Bancária)", () => ProcessTransactionView.Show())
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "--> ";
         config.EnableFilter = false;
         config.Title = "Transações";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
       });

      var subReports = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Exibir Todos os Clientes Ativos com seus Respectivos Saldos", () => ReportView.ShowActiveClients())
       .Add("Exibir Todos os Clientes Inativos", () => ReportView.ShowDisabledClients())
       .Add("Exibir Todos os Funcionários Ativos e sua Última Data e Hora de Login", () => ReportView.ShowActiveEmployees())
       .Add("Exibir Transações com Erro (Detalhes da transação e do Erro)", () => Console.WriteLine("Sub_Three"))
       .Add("Voltar", ConsoleMenu.Close)
       .Configure(config =>
       {
         config.Selector = "--> ";
         config.EnableFilter = false;
         config.Title = "Relatórios";
         config.EnableBreadcrumb = true;
         config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
       });

      var menu = new ConsoleMenu(Array.Empty<string>(), level: 2)
        .Add("Clientes", () => subClient.Show())
        .Add("Funcionários", () => subEmployee.Show())
        .Add("Transações", () => subTransactions.Show())
        .Add("Relatórios", () => subReports.Show())
        .Add("Exit", () => Environment.Exit(0))
        .Configure(config =>
        {
          config.Selector = "--> ";
          config.EnableFilter = false;
          config.Title = "Ada Credit";
          config.EnableWriteTitle = false;
          config.EnableBreadcrumb = true;
        });

      menu.Show();
    }
  }
}
