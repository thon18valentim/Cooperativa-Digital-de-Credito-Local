using AdaCredit.Domain.Entities;
using AdaCredit.Domain.Entities.Enums;
using AdaCredit.Infra.Repositories;
using AdaCredit.Utils;

namespace AdaCredit.Domain.UseCases
{
  public sealed class DoProcessTransactions : IUseCase
  {
    private readonly List<Transaction> completedTransactions = new();
    private readonly List<Transaction> failedTransactions = new();

    public bool Run(IEnumerable<IUseCaseParameter> parameter)
    {
      var transactions = parameter.FirstOrDefault(x => x.ParameterName == "Transactions").ToList<Transaction>();

      foreach (var transaction in transactions)
      {
        var sameBank = true;
        var fromHome = false;

        // verifying if the accounts are the same
        if (string.Equals(transaction.OriginBankCode, transaction.DestinationBankCode))
        {
          if (string.Equals(transaction.OriginBankAccount, transaction.DestinationBankAccount))
          {
            transaction.ErrorMessage = "Erro, as contas da transação se referem ao mesmo registro";
            transaction.ErrorDate = DateTime.Now;
            failedTransactions.Add(transaction);
            continue;
          }
        }

        // verifying if the accounts are from the same bank
        if (!string.Equals(transaction.OriginBankCode, transaction.DestinationBankCode))
        {
          sameBank = false;
          if (transaction.Type == TransactionType.TEF)
          {
            transaction.ErrorMessage = "Transações de tipo TEF devem ser entre clientes do mesmo banco";
            transaction.ErrorDate = DateTime.Now;
            failedTransactions.Add(transaction);
            continue;
          }
        }

        // verifying if the accounts exists and active
        if (string.Equals(transaction.OriginBankCode, "777"))
        {
          if (!AccountAvailable(transaction.OriginBankAccount))
          {
            transaction.ErrorMessage = "Conta de origem não existe ou está desativada";
            transaction.ErrorDate = DateTime.Now;
            failedTransactions.Add(transaction);
            continue;
          }
          fromHome = true;
        }
        if (string.Equals(transaction.DestinationBankCode, "777"))
        {
          if (!AccountAvailable(transaction.DestinationBankAccount))
          {
            transaction.ErrorMessage = "Conta de destino não existe ou está desativada";
            transaction.ErrorDate = DateTime.Now;
            failedTransactions.Add(transaction);
            continue;
          }
        }

        // applying transaction rate & value
        if (!ApplyTransaction(transaction, fromHome, sameBank))
        {
          transaction.ErrorMessage = "Saldo insuficiente";
          transaction.ErrorDate = DateTime.Now;
          failedTransactions.Add(transaction);
          continue;
        }

        completedTransactions.Add(transaction);
      }

      SaveCompletedTransactions();
      SaveFailureTransactions();

      return true;
    }

    private decimal CalculateTEDRate(decimal transactionValue)
    {
      var v = transactionValue * (1M / 100M);

      if (v < 5M)
        return 1M + v;

      return 6M;
    }

    private bool AccountAvailable(string accountNumber)
    {
      return AccountRepository.Exists(accountNumber) && ClientRepository.IsActive(accountNumber);
    }

    private bool HasEnoughCash(string accountNumber, decimal value)
      => AccountRepository.Find(accountNumber)?.Balance >= value;

    private decimal GenerateRate(Transaction transaction)
    {
      DateTime date = new(2022, 11, 30);

      if (transaction.Entry == 1)
        return 0.00M;

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
        originAccount = AccountRepository.Find(transaction.OriginBankAccount);
        destinationAccount = AccountRepository.Find(transaction.DestinationBankAccount);

        var rate = GenerateRate(transaction);

        if (transaction.Entry == 0)
        {
          if (!HasEnoughCash(transaction.OriginBankAccount, transaction.Value))
            return false;

          originAccount.Balance -= transaction.Value + rate;
          destinationAccount.Balance += transaction.Value;
        }
        else
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
          originAccount = AccountRepository.Find(transaction.OriginBankAccount);

          if (transaction.Entry == 0)
          {
            if (!HasEnoughCash(transaction.OriginBankAccount, transaction.Value))
              return false;

            originAccount.Balance -= transaction.Value + rate;
          }
          else
          {
            originAccount.Balance += transaction.Value;
          }
        }
        else
        {
          destinationAccount = AccountRepository.Find(transaction.DestinationBankAccount);

          if (transaction.Entry == 0)
          {
            destinationAccount.Balance += transaction.Value;
          }
          else
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

      var groups = 
        completedTransactions.GroupBy(t => new { t.BankName, t.Date});

      foreach (var group in groups)
      {
        TransactionsRepository.SaveCompleted(group.ToList());
      }
    }

    private void SaveFailureTransactions()
    {
      if (failedTransactions.Count == 0)
        return;

      var groups = 
        failedTransactions.GroupBy(t => new { t.BankName, t.Date });

      foreach (var group in groups)
      {
        TransactionsRepository.SaveFailure(group.ToList());
      }
    }
  }
}
