
using AdaCredit.Views;
using AdaCredit.Infra.Repositories;

namespace AdaCredit
{
  public static class Program
  {
    static void Main()
    {
      TransactionsRepository.Setup();
      ClientRepository.Load();
      AccountRepository.Load();

      LoginView.Show();
      MenuView.Show();
    }
  }
}
