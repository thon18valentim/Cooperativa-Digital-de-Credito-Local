using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Enums;
using AdaCredit.Infra.Repositories;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoProcessTransactions : IUseCase
  {
    private List<Transaction> completedTransactions = new();
    private List<Transaction> failedTransactions = new();
    private List<TransactionError> failedErrors = new();

    public bool Run(string param1, string param2)
    {
      throw new NotImplementedException();
    }

    public bool Run(Transaction[][] transactions)
    {
      for (int i = 0; i < transactions.Length; i++)
      {
        for (int j = 0; j < transactions[i].Length; j++)
        {
          var sameBank = true;
          var fromHome = false;

          // verificando se contas são do mesmo banco
          if (!string.Equals(transactions[i][j].HomeBankCode, transactions[i][j].DestinationBankCode))
          {
            sameBank = false;
            if (transactions[i][j].Type == TransactionType.TEF)
            {
              failedTransactions.Add(transactions[i][j]);
              failedErrors.Add(new("Transações de tipo TEF devem ser de clientes do mesmo banco", DateTime.Now));
              continue;
            }
          }

          // verificando existencia da conta
          if (string.Equals(transactions[i][j].HomeBankCode, "777"))
          {
            if (!AccountExists(transactions[i][j].HomeBankAccount))
            {
              failedTransactions.Add(transactions[i][j]);
              failedErrors.Add(new("Conta de origem não existe", DateTime.Now));
              continue;
            }
            fromHome = true;
          }
          if (string.Equals(transactions[i][j].DestinationBankAccount, "777"))
          {
            if (!AccountExists(transactions[i][j].DestinationBankAccount))
            {
              failedTransactions.Add(transactions[i][j]);
              failedErrors.Add(new("Conta de destino não existe", DateTime.Now));
              continue;
            }
          }

          if (!ApplyTransaction(transactions[i][j], fromHome, sameBank))
          {
            failedTransactions.Add(transactions[i][j]);
            failedErrors.Add(new("Saldo insuficiente", DateTime.Now));
            continue;
          }

          completedTransactions.Add(transactions[i][j]);
        }

        SaveCompletedTransactions();
        SaveFailureTransactions();
      }

      return false;
    }

    private decimal CalculateTEDRate(decimal transactionValue)
    {
      var v = transactionValue * (1M / 100M);

      if (v < 5M)
        return 1M + v;

      return 6M;
    }

    private bool AccountExists(string accountNumber)
      => AccountRepository.Exists(accountNumber);

    private bool HasEnoughCash(string accountNumber, decimal value)
      => AccountRepository.Find(accountNumber)?.Balance >= value;

    private decimal GenerateRate(Transaction transaction)
    {
      DateTime date = new(2022, 11, 30);

      // credito é isento
      if (transaction.Entry == 1)
        return 0.00M;

      // até 30/11/2022 debito nao tem tarifa
      if (transaction.Entry == 0 & DateTime.Compare(transaction.Date, date) < 0)
        return 0.00M;

      return transaction.Type switch
      {
        TransactionType.TED => 5.00M,
        TransactionType.DOC => CalculateTEDRate(transaction.Value),
        _ => 0.00M
      };
    }

    private bool ApplyTransaction(Transaction transaction, bool fromHome, bool sameBank)
    {
      Account? originAccount;
      Account? destinationAccount;

      if (sameBank)
      {
        originAccount = AccountRepository.Find(transaction.HomeBankAccount);
        destinationAccount = AccountRepository.Find(transaction.DestinationBankAccount);

        var rate = GenerateRate(transaction);

        if (transaction.Entry == 0) // Debit
        {
          if (!HasEnoughCash(transaction.HomeBankAccount, transaction.Value))
            return false;

          originAccount.Balance -= transaction.Value + rate;
          destinationAccount.Balance += transaction.Value;
        }
        else // Credit
        {
          if (!HasEnoughCash(transaction.DestinationBankAccount, transaction.Value))
            return false;

          originAccount.Balance += transaction.Value;
          destinationAccount.Balance -= transaction.Value + rate;
        }
      }
      else
      {
        if (transaction.Type == TransactionType.TEF)
          return false;

        var rate = GenerateRate(transaction);

        if (fromHome)
        {
          originAccount = AccountRepository.Find(transaction.HomeBankAccount);

          if (transaction.Entry == 0) // Debit
          {
            if (!HasEnoughCash(transaction.HomeBankAccount, transaction.Value))
              return false;

            originAccount.Balance -= transaction.Value + rate;
          }
          else // Credit
          {
            originAccount.Balance += transaction.Value;
          }
        }
        else
        {
          destinationAccount = AccountRepository.Find(transaction.DestinationBankAccount);

          if (transaction.Entry == 0) // Debit
          {
            destinationAccount.Balance += transaction.Value;
          }
          else // Credit
          {
            if (!HasEnoughCash(transaction.DestinationBankAccount, transaction.Value))
              return false;

            destinationAccount.Balance -= transaction.Value + rate;
          }
        }
      }

      return AccountRepository.Save();
    }

    private void SaveCompletedTransactions()
    {
      if (completedTransactions.Count == 0)
        return;

      TransactionsRepository.SaveCompleted(completedTransactions, completedTransactions[0].BankName);

      completedTransactions = new();
    }

    private void SaveFailureTransactions()
    {
      if (failedTransactions.Count == 0)
        return;

      TransactionsRepository.SaveFailure(failedTransactions, failedErrors, failedTransactions[0].BankName);

      failedTransactions = new();
    }
  }
}
