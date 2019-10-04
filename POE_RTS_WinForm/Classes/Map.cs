using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POE_RTS_WinForm
{
  public class Map
  {
    public Map(int aNumberOfUnits, int aNumberOfBuildings)
    {
      this.numberOfUnits = aNumberOfUnits;
      this.numberOfBuildings = aNumberOfBuildings;

      rand = new Random();

      map = new char[gridSize, gridSize];
      units = new List<Unit>();
      buildings = new List<Building>();
    }

    private char blankSpaceCharacter = 'o';
    public static int gridSize = 20;

    public int numberOfUnits;
    public int numberOfBuildings;

    public List<Unit> units;
    public List<Building> buildings;

    private Random rand;

    public IUnit[,] Battlefield;
    private char[,] map;

    public void GenerateBattleField()
    {
      Battlefield = new IUnit[gridSize, gridSize];

      for (int i = 0; i < numberOfUnits; i++)
      {
        Unit unit = GenerateRandomUnit(i);
        units.Add(unit);
      }

      for (int i = 0; i < numberOfBuildings; i++)
      {
        Building building = GenerateRandomBuilding();
        building.unitNumber = i;
        buildings.Add(building);
      }
    }

    public Point GetRandomOpenPosition()
    {
      Point point = new Point();

      bool posFound = false;
      do
      {
        point.xPos = rand.Next(0, gridSize);
        point.yPos = rand.Next(0, gridSize);
        if (Battlefield[point.xPos, point.yPos] == null)
        {
          posFound = true;
        }
      }
      while (!posFound);

      return point;
    }

    private Unit GenerateRandomUnit(int aUnitNumber)
    {
      Point point = GetRandomOpenPosition();

      int xPos = point.xPos;
      int yPos = point.yPos;

      int whichUnit = rand.Next(0, 2);
      int speed = rand.Next(1, 4);

      char lSymbol;
      string lFaction;
      int whichFaction = rand.Next(0, 2);
      if (whichFaction == 0)
      {
        lSymbol = 'H';
        lFaction = "Horde";
      }
      else
      {
        lSymbol = 'A';
        lFaction = "Alliance";
      }
      Unit unit;
      if (whichUnit == 0)
      { //spawn a ranged unit
      string name = $"RangedUnit{aUnitNumber}";
        unit = new RangedUnit(name, xPos, yPos, 12, speed, 1, 3, lFaction, lSymbol);
      }
      else 
      { //spawn a melee unit
        unit = new MeleeUnit($"MeleeUnit{aUnitNumber}", xPos, yPos, 20, speed, 2, lFaction, lSymbol);
      }
      return unit;
    }

    private Building GenerateRandomBuilding()
    {
      int whichBuilding =  rand.Next(0, 2);

      if (whichBuilding == 0)
      {
        Point point = GetRandomOpenPosition();

        char lSymbol;
        string lFaction;
        int whichFaction = rand.Next(0, 2);
        if (whichFaction == 0)
        {
          lSymbol = 'H';
          lFaction = "Horde";
        }
        else
        {
          lSymbol = 'A';
          lFaction = "Alliance";
        }

        ResourceBuilding building = new ResourceBuilding(point.xPos, point.yPos, 10, lFaction, 'R', "Coal", 2, 1);
        return building;
      }
      else
      {
        return GenerateRandomFactory();
      }
    }

    private Building GenerateRandomFactory()
    {
      Point point = GetRandomOpenPosition();
      int xPos = point.xPos;
      int yPos = point.yPos;

      int whichUnit = rand.Next(0, 2);
      int speed = rand.Next(1, 4);

      string lFaction;
      int whichFaction = rand.Next(0, 2);
      if (whichFaction == 0)
      {
        lFaction = "Horde";
      }
      else
      {
        lFaction = "Alliance";
      }

      Building building;

      if (whichUnit == 0)
      { //spawn a ranged Factory 
        building = new FactoryBuilding<RangedUnit>(xPos, yPos, 20, lFaction, 'F');
      }
      else
      { //spawn a melee Factory
        building = new FactoryBuilding<MeleeUnit>(xPos, yPos, 20, lFaction, 'F');
      }

      return building;
    }

    public void PopulateMap()
    {
      int index = 0;
      while (index < units.Count)
      {
        IUnit lUnit = units[index] as IUnit;
        if (lUnit.Health <= 0)
        {
          units.Remove(units[index]);
        }
        else
        {
          index++;
        }
      }

      foreach (IUnit unit in units)
      {
        Battlefield[unit.xPos, unit.yPos] = unit;
      }
      foreach (IUnit building in buildings)
      {
        Battlefield[building.xPos, building.yPos] = building;
      }
    }

    public void UpdateDisplay()
    {
      for (int i = 0; i < gridSize; i++)
      {
        for (int j = 0; j < gridSize; j++)
        {
          map[i, j] = blankSpaceCharacter;
        }
      }

      foreach (IUnit unit in units)
      {
        if (unit is IUnit)
        {
          IUnit lUnit = unit as IUnit;
          map[lUnit.xPos, lUnit.yPos] = lUnit.Symbol;
        }
      }

      foreach (IUnit building in buildings)
      {
          map[building.xPos, building.yPos] = building.Symbol;
      }
    }

    public void UpdateUnitPosition(Unit aUnit, int aNewXPosition, int aNewYPosition)
    {
      if (aUnit is RangedUnit)
      {
        RangedUnit lUnit = aUnit as RangedUnit;
        lUnit.xPos = aNewXPosition;
        lUnit.yPos = aNewYPosition;
      }
      if (aUnit is MeleeUnit)
      {
        MeleeUnit lUnit = aUnit as MeleeUnit;
        lUnit.xPos = aNewXPosition;
        lUnit.yPos = aNewYPosition;
      }
    }

    public string PrintMap()
    {
      string text = "";

      for (int i = 0; i < gridSize; i++)
      {
        for (int j = 0; j < gridSize; j++)
        {
          text += map[i, j].ToString() + " ";
        }
        text += Environment.NewLine;
      }
      return text;
    }
  }
}
