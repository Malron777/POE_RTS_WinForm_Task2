using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace POE_RTS_WinForm
{
  [Serializable]
  public class FactoryBuilding<T> : Building, IUnit
    where T : Unit, new()
  {
    public FactoryBuilding(int aXPos, int aYPos, int aHealth, string aFaction, char aSymbol)
              : base(aXPos, aYPos, aHealth, aFaction, aSymbol)
    {
      base.xPosition = aXPos;
      base.yPosition = aYPos;

      base.health = aHealth;
      base.maxHealth = aHealth;

      base.faction = aFaction;
      base.symbol = aSymbol;

      //Set spawn point
      SpawnPoint = aYPos + 1;
      if (SpawnPoint > Map.gridSize)
      {
        SpawnPoint = aYPos - 1;
      }
    }

    public FactoryBuilding(int aXPos, int aYPos, int aHealth, string aFaction, char aSymbol, int aMaxHealth)
: base(aXPos, aYPos, aHealth, aFaction, aSymbol, aMaxHealth)
    {
      base.xPosition = aXPos;
      base.yPosition = aYPos;

      base.health = aHealth;
      base.maxHealth = aMaxHealth;

      base.faction = aFaction;
      base.symbol = aSymbol;

      //Set spawn point
      SpawnPoint = aYPos + 1;
      if (SpawnPoint > Map.gridSize)
      {
        SpawnPoint = aYPos - 1;
      }
    }

    public int xPos
    {
      get
      {
        return base.xPosition;
      }
      set
      {
        if (value < 0)
        {
          base.xPosition = 0;
        }
        else if (value >= Map.gridSize)
        {
          base.xPosition = Map.gridSize - 1;
        }
        else
        {
          base.xPosition = value;
        }
      }
    }
    public int yPos
    {
      get
      {
        return base.yPosition;
      }
      set
      {
        if (value < 0)
        {
          base.yPosition = 0;
        }
        else if (value >= Map.gridSize)
        {
          base.yPosition = Map.gridSize - 1;
        }
        else
        {
          base.yPosition = value;
        }
      }
    }

    public int Health
    {
      get
      {
        return base.health;
      }
      set
      {
        base.health = value;
        if (base.health < 0)
        {
          Death();
        }
        if (base.health > maxHealth)
        {
          base.health = maxHealth;
        }
      }
    }
    public int MaxHealth
    {
      get
      {
        return base.maxHealth;
      }
      //no set because it shouldn't be changed unless from an expansion
    }

    public string Faction
    {
      get
      {
        return base.faction;
      }
      set
      {
        base.faction = value;
      }
    }
    public char Symbol
    {
      get
      {
        return base.symbol;
      }
      set
      {
        base.symbol = value;
      }
    }

    public string FileName
    {
      get
      {
        return Path.Combine(GameSettings.ApplicationPath, GameSettings.sUnitsPath, $"FactoryBuilding{unitNumber}.txt");
      }
    }

    public string UnitType
    {
      get
      {
        return typeof(T).Name;
      }
    }

    public int ProductionSpeed { get; } = 5;

    public int SpawnPoint { get; }

    public Unit BuildNewUnit(string aName, int aXPos, int aYPos, int aHealth)
    {
      var lNewUnit = new T();

      if (lNewUnit is IUnit)
      {
        var lUnit = lNewUnit as IUnit;

        lUnit.Health = aHealth;
        lUnit.Faction = this.faction;
        char[] charArray = this.Faction.ToCharArray();
        lUnit.Symbol = charArray[0];
        lUnit.xPos = this.xPos;
        lUnit.yPos = this.SpawnPoint;
      }

      return lNewUnit;
    }

    public override void Death()
    {
      throw new NotImplementedException();
    }

    public override string ToString()
    {
      string text = "";

      text += $"Factory Building:{ Environment.NewLine }" +
              $"Unit produced: { UnitType }{ Environment.NewLine }" +
              $"Position: ({ xPos },{ yPos }){ Environment.NewLine }" +
              $"Health: { health }.{ Environment.NewLine }" +
              $"Production speed: { ProductionSpeed }{ Environment.NewLine }" +
              $"Faction: { faction }({ symbol }){ Environment.NewLine }";


      return text;
    }

    public override void SaveToFile()
    {
      var lClassSerialisation = new ClassSerialisation<FactoryBuilding<T>>();

      lClassSerialisation.SaveClass(FileName, this);
    }
  }
}
