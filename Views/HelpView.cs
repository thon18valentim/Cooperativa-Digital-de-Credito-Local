using Spectre.Console;

namespace AdaCredit.Views
{
  public static class HelpView
  {
    public static void ShowSystem()
    {
      Console.Clear();

      AnsiConsole.MarkupLine("── [darkorange]Arquitetura do Sistema[/] ───────────────────────────────────────────────────");
      Console.WriteLine("\n");

      var tree = new Tree("PC");

      var project = tree.AddNode("Ada Credit");
      var root = project.AddNode("Pasta raiz (junto ao .exe)");
      var file = root.AddNode("adaCredit_employee_database.csv");
      file.AddNode(new Table()
        .AddColumn("Name")
        .AddColumn("Login")
        .AddColumn("PasswordHash")
        .AddColumn("PasswordSalt")
        .AddColumn("LastLogin")
        .AddColumn("Active")
        .AddRow("Fulano", "fulano123", "some hash", "some hash", "04/01/2023 - 22:03", "True"));
      var fileUsers = root.AddNode("adaCredit_client_database.csv");
      fileUsers.AddNode(new Table()
        .AddColumn("Name")
        .AddColumn("Cpf")
        .AddColumn("Account Id")
        .AddColumn("Active")
        .AddRow("Fulano", "12312312301", "1", "False"));
      var fileAccounts = root.AddNode("adaCredit_account_database.csv");
      fileAccounts.AddNode(new Table()
        .AddColumn("Id")
        .AddColumn("Number")
        .AddColumn("Agency")
        .AddColumn("Balance")
        .AddRow("1", "12345-6", "0001", "150000.00"));

      var desktop = tree.AddNode("Desktop");
      var transactions = desktop.AddNode("Transactions");
      var completed = transactions.AddNode("Completed");
      var failed = transactions.AddNode("Failed");
      var pending = transactions.AddNode("Pending");

      completed.AddNode("nome-do-banco-aaaammdd-completed.csv");
      failed.AddNode("nome-do-banco-aaaammdd-failed.csv");
      pending.AddNode("nome-do-banco-aaaammdd.csv");

      AnsiConsole.Write(tree);

      Console.WriteLine("\n");

      AnsiConsole.MarkupLine("Pressione [darkorange]ENTER[/] para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }

    public static void ShowTransactions()
    {
      Console.Clear();

      AnsiConsole.MarkupLine("── [darkorange]Sistema de Transações[/] ───────────────────────────────────────────────────");
      Console.WriteLine("\n");

      var tree = new Tree("Arquivo de transações");
      var csv = tree.AddNode(".csv");
      var structer = csv.AddNode("Estrutura das linhas");
      var lineStructer = structer.AddNode("AAA,BBBB,CCCCCC,DDD,EEEE,FFFFFF,GGG,H,I");

      lineStructer.AddNode("AAA - Número com 3 dígitos representando o Código do Banco de Origem");
      lineStructer.AddNode("BBBB - Número com 4 dígitos representando a Agência do Banco de Origem");
      lineStructer.AddNode("CCCCCC - Número com 6 dígitos representando o número da conta do Banco de Origem");
      lineStructer.AddNode("DDD - Número com 3 dígitos representando o Código do Banco de Destino");
      lineStructer.AddNode("EEEE - Número com 4 dígitos representando a Agência do Banco de Destin");
      lineStructer.AddNode("FFFFFF - Número com 6 dígitos representando o número da conta do Banco de Destino");
      lineStructer.AddNode("GGG - Tipo da Transação (DOC, TED, TEF)");
      lineStructer.AddNode("H - Número representando o sentido da transação (0 - Débito/Saída, 1 - Crédito/Entrada)");
      lineStructer.AddNode("I - número real com duas casas decimais, separadas por um . e sem separador de milhar");

      var process = tree.AddNode("Processamento");
      var steps = process.AddNode("Etapas");

      steps.AddNode("Verificação de informações");
      steps.AddNode("Cálculo de tarifas");
      steps.AddNode("Atualização das contas envolvidas");

      var saving = process.AddNode("Salvamento");
      saving.AddNode("Completas (Processadas com sucesso)");
      saving.AddNode("Fracassadas (Erro no processamento)");

      AnsiConsole.Write(tree);

      Console.WriteLine("\n");

      AnsiConsole.MarkupLine("Pressione [darkorange]ENTER[/] para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }
  }
}
