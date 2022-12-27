
using AdaCredit.Views;
using AdaCredit.Domain.Entities;
using AdaCredit.Utils;
using AdaCredit.Infra.Repositories;

namespace AdaCredit
{
  public static class Program
  {
    static void Main()
    {
      LoginView.Show();
      MenuView.Show();
    }
  }
}
