using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Enums;
using AdaCredit.Infra.Repositories;
using AdaCredit.Domain.UseCases;

namespace AdaCredit.Views
{
  public static class ProcessTransactionView
  {
    public static void Show()
    {
      Console.Clear();

      var table = new Table();

      table.AddColumn("Banco de Origem & Conta"); // codigo (agencia) + conta
      table.AddColumn("Banco de Destino & Conta"); // codigo (agencia) + conta
      table.AddColumn("[grey58]Tipo (Sentido)[/]"); // DOC, TED, TEF (0 - Débito/Saída, 1 - Crédito/Entrada)
      table.AddColumn("[green]Valor[/]");

      List<Transaction> transactions = TransactionsRepository.LoadPending();

      if (transactions == default || transactions.Count == 0)
      {
        Console.WriteLine("\n");
        AnsiConsole.Write(new Markup("[bold red]Nenhuma transação pendente encontrada[/]"));
      }
      else
      {
        foreach (Transaction transaction in transactions)
        {
          var entry = transaction.Entry switch
          {
            0 => "Débito",
            _ => "Crédito"
          };

          table.AddRow(
            $"{transaction.OriginBankCode} ({transaction.OriginBankAgency}) - {transaction.OriginBankAccount}",
            $"{transaction.DestinationBankCode} ({transaction.DestinationBankAgency}) - {transaction.DestinationBankAccount}",
            $"[grey70]{transaction.Type} ({entry})[/]",
            $"[springgreen4]{string.Format("{0:C}", transaction.Value)}[/]"
            );
        }

        AnsiConsole.MarkupLine("─────────────────────────────────────── [darkorange]Transações em processo[/] ───────────────────────────────────────");
        AnsiConsole.Write(table);

        new DoProcessTransactions().Run(new ListUseCaseParameter<Transaction>[] { new(){ ParameterName = "Transactions", ParameterValue = transactions } });

        AnsiConsole.Write(new Markup("[bold green]Processamento de transações concluído![/]"));
      }

      Console.WriteLine("\n");
      AnsiConsole.MarkupLine("Pressione [darkorange]ENTER[/] para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }
  }
}
