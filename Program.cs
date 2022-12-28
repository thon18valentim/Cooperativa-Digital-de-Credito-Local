﻿
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
      ClientRepository.Load();
      AccountRepository.Load();

      LoginView.Show();
      MenuView.Show();
    }
  }
}
