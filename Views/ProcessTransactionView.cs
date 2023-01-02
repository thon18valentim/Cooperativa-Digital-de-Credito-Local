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
      table.AddColumn("Tipo (Sentido)"); // DOC, TED, TEF (0 - Débito/Saída, 1 - Crédito/Entrada)
      table.AddColumn("Valor");

      Transaction[][]? transactions = TransactionsRepository.LoadPending();

      if (transactions == default || transactions.Length == 0)
      {
        Console.WriteLine("Nenhuma transação pendente encontrada\n");
      }
      else
      {
        for (int i = 0; i < transactions.Length; i++)
        {
          for (int j = 0; j < transactions[i].Length; j++)
          {
            var entry = transactions[i][j].Entry switch
            {
              0 => "Débito",
              _ => "Crédito"
            };

            table.AddRow(
              $"{transactions[i][j].OriginBankCode} ({transactions[i][j].OriginBankAgency}) - {transactions[i][j].OriginBankAccount}",
              $"{transactions[i][j].DestinationBankCode} ({transactions[i][j].DestinationBankAgency}) - {transactions[i][j].DestinationBankAccount}",
              $"{transactions[i][j].Type} ({entry})",
              string.Format("{0:C}", transactions[i][j].Value)
              );
          }
        }

        AnsiConsole.Write(table);

        new DoProcessTransactions().Run(transactions);

        Console.WriteLine("Processamento de transações concluído!\n");
      }

      Console.WriteLine("Pressione ENTER para voltar");

      ConsoleKeyInfo info = Console.ReadKey(true);
      while (info.Key != ConsoleKey.Enter) { info = Console.ReadKey(true); }
    }
  }
}
