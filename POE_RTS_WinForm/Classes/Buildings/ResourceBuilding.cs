using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace POE_RTS_WinForm
{
  [Serializable]
  public class ResourceBuilding : Building, IUnit
  {
    public ResourceBuilding(int aXPos, int aYPos, int aHealth, string aFaction, char aSymbol, string aResourceType, int aResourceAmmount, int aResourcesPR) 
      : base(aXPos, aYPos, aHealth, aFaction, aSymbol)
    {
      base.xPosition = aXPos;
      base.yPosition = aYPos;

      base.health = aHealth;
      base.maxHealth = aHealth;

      base.faction = aFaction;
      base.symbol = aSymbol;

      this.resourceType = aResourceType;
      this.resourcesGenerated = aResourceAmmount;
      this.resourcesGeneratedPerRound = aResourcesPR;
    }

    public ResourceBuilding(int aXPos, int aYPos, int aHealth, string aFaction, char aSymbol, string aResourceType, int aResourceAmmount, int aResourcesPR, int aMaxHealth)
: base(aXPos, aYPos, aHealth, aFaction, aSymbol, aMaxHealth)
    {
      base.xPosition = aXPos;
      base.yPosition = aYPos;

      base.health = aHealth;
      base.maxHealth = aMaxHealth;

      base.faction = aFaction;
      base.symbol = aSymbol;

      this.resourceType = aResourceType;
      this.resourcesGenerated = 0;
      this.resourcesGeneratedPerRound = aResourcesPR;
      this.ResourcePoolRemaining = aResourceAmmount;
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
        return Path.Combine(GameSettings.ApplicationPath, GameSettings.sUnitsPath, $"ResourceBuilding{unitNumber}.txt");
      }
    }

    private string resourceType;
    private int resourcesGenerated;
    private int resourcesGeneratedPerRound;
    private int ResourcePoolRemaining;

    public void GenerateResources()
    {
      resourcesGenerated += resourcesGeneratedPerRound;
      ResourcePoolRemaining += resourcesGeneratedPerRound;
    }

    public void RemoveResources(int aResourcesLost)
    {
      ResourcePoolRemaining -= aResourcesLost;
    }

    public override void Death()
    {
      resourcesGeneratedPerRound = 0;
      ResourcePoolRemaining = 0;
    }

    public override string ToString()
    {
      string text = "";
      text += $"Resource Building:{ Environment.NewLine }" +
              $"Resource type: {resourceType}{Environment.NewLine}" +
              $"Resources generated: {resourcesGenerated}{Environment.NewLine}" +
              $"Resources generated per round: {resourcesGeneratedPerRound}{Environment.NewLine}" +
              $"Resource pool remaining: {ResourcePoolRemaining}{Environment.NewLine}";

      return text;
    }

    public override void SaveToFile()
    {
      var lClassSerialisation = new ClassSerialisation<ResourceBuilding>();

      lClassSerialisation.SaveClass(FileName, this);
    }
  }
}
