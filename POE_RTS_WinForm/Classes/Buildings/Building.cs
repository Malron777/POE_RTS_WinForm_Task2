using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace POE_RTS_WinForm
{
  [Serializable]
  public abstract class Building 
  {
    public Building(int aXPos, int aYPos, int aHealth, string aFaction, char aSymbol)
    {

    }
    public Building(int aXPos, int aYPos, int aHealth, string aFaction, char aSymbol, int aMaxHP)
    {

    }
    public int unitNumber;

    protected int xPosition;
    protected int yPosition;

    protected int health;
    protected int maxHealth;

    protected string faction;
    protected char symbol;

    public abstract void Death();

    public abstract override string ToString();

    public abstract void SaveToFile();
  }
}
