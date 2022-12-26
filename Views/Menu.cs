
using ConsoleTools;

namespace AdaCredit.Views
{
  public static class Menu
  {
    public static void Show()
    {
      var subClient = new ConsoleMenu(Array.Empty<string>(), level: 1)
       .Add("Cadastrar Novo Cliente", () => Console.WriteLine("Sub_One"))
       .Add("Consultar os Dados de um Cliente existente", () => Console.WriteLine("Sub_Two"))
       .Add("Alterar o Cadastro de um Cliente existente", () => Console.WriteLine("Sub_Three"))
       .Add("Desativar Cadastro de um Cliente existente", () => Console.WriteLine("Sub_Four"))
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
       .Add("Cadastrar Novo Funcionário", () => Console.WriteLine("Sub_One"))
       .Add("Alterar Senha de um Funcionário existente", () => Console.WriteLine("Sub_Two"))
       .Add("Desativar Cadastro de um Funcionário existente", () => Console.WriteLine("Sub_Three"))
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
       .Add("Processar Transações (Reconciliação Bancária)", () => Console.WriteLine("Sub_One"))
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
       .Add("Exibir Todos os Clientes Ativos com seus Respectivos Saldos", () => Console.WriteLine("Sub_One"))
       .Add("Exibir Todos os Clientes Inativos", () => Console.WriteLine("Sub_Two"))
       .Add("Exibir Todos os Funcionários Ativos e sua Última Data e Hora de Login", () => Console.WriteLine("Sub_Three"))
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
