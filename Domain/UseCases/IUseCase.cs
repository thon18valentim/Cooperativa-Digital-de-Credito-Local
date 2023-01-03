﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Domain.UseCases
{
  public interface IUseCase
  {
    public bool Run(IEnumerable<IUseCaseParameter> parameters);
  }
}
