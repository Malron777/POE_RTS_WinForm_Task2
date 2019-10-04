using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_RTS_WinForm
{
  public interface IUnit : IPosition
  {
    int Health { get; set; }
    int MaxHealth { get; }

    string Faction { get; set; }
    char Symbol { get; set; }
  }
}
