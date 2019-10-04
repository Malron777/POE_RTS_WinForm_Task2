using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace POE_RTS_WinForm
{
  public class GameEngine
  {
    public GameEngine(int aNumberOfUnits, int aNumberOfBuildings)
    {
      map = new Map(aNumberOfUnits, aNumberOfBuildings);
      rand = new Random();
    }

    private Random rand;
    public Map map;

    public int roundsCompleted = 1;

    public void StartGame()
    {
      map.GenerateBattleField();
      map.PopulateMap();
      map.UpdateDisplay();
    }

    public void StartNewRound()
    {
      for (int i = 0; i < map.units.Count; i++)
      {
        if (map.units[i] is Unit)
        {
          PerformAction(map.units[i]);
        }
      }

      for (int i = 0; i < map.buildings.Count; i++)
      {
        if (map.buildings[i] is ResourceBuilding)
        {
          ResourceBuilding building = map.buildings[i] as ResourceBuilding;
          building.GenerateResources();
        }
        else
        {
          CheckBuildingProduction(i);
        }
      }

      map.PopulateMap();
      map.UpdateDisplay();

      roundsCompleted++;
    }

    private void CheckBuildingProduction(int index)
    {
      if (map.buildings[index] is FactoryBuilding<RangedUnit>)
      {
        FactoryBuilding<RangedUnit> building = map.buildings[index] as FactoryBuilding<RangedUnit>;
        //if (roundsCompleted == 0)
        //{
        //  string unitNumber = (map.numberOfUnits + map.numberOfBuildings).ToString();
        //  RangedUnit RU = building.BuildNewUnit($"Archer{unitNumber}", building.xPos, building.SpawnPoint, 10) as RangedUnit;
        //  map.units.Add(RU);
        //}
        //else
        if (roundsCompleted % building.ProductionSpeed == 0)
        {
          string unitNumber = (map.numberOfUnits + map.numberOfBuildings).ToString();
          RangedUnit RU = building.BuildNewUnit($"Archer{unitNumber}", building.xPos, building.SpawnPoint, 10) as RangedUnit;
          map.units.Add(RU);
        }
      }
      if (map.buildings[index] is FactoryBuilding<MeleeUnit>)
      {
        FactoryBuilding<MeleeUnit> building = map.buildings[index] as FactoryBuilding<MeleeUnit>;
        //if (roundsCompleted == 0)
        //{
        //  string unitNumber = (map.numberOfUnits + map.numberOfBuildings).ToString();
        //  MeleeUnit MU = building.BuildNewUnit($"Tank{unitNumber}", building.xPos, building.SpawnPoint, 10) as MeleeUnit;
        //  map.units.Add(MU);
        //}
        //else 
        if (roundsCompleted % building.ProductionSpeed == 0)
        {
          string unitNumber = (map.numberOfUnits + map.numberOfBuildings).ToString();
          MeleeUnit MU = building.BuildNewUnit($"Tank{unitNumber}", building.xPos, building.SpawnPoint, 10) as MeleeUnit;
          map.units.Add(MU);
        }
      }
    }

    private void PerformAction(Unit aUnit)
    {
      if (aUnit is RangedUnit)
      {
        RangedUnit lUnit = aUnit as RangedUnit;
        IUnit closestEnemy = aUnit.FindClosestEnemy(map.Battlefield);

        if (closestEnemy != null && lUnit.Health >= 0.25 * lUnit.MaxHealth)
        {
          if (lUnit.RangeCheck(closestEnemy))
          {
            lUnit.EngageUnit(closestEnemy);
            lUnit.IsAttacking = true;
          }
          else if (roundsCompleted % lUnit.Speed == 0)
          {
            if (closestEnemy is IPosition)
            { //Move toward the enemy
              var lTarget = closestEnemy as IPosition;
              int differenceInXPosition = Math.Abs(lUnit.xPos - lTarget.xPos);
              int differenceInYPosition = Math.Abs(lUnit.yPos - lTarget.yPos);
              if (differenceInXPosition > differenceInYPosition)
              { //Move vertical
                if (lUnit.yPos <= lTarget.yPos)
                {
                  lUnit.Move(Unit.Direction.Up);
                }
                else if (lUnit.yPos > lTarget.yPos)
                {
                  lUnit.Move(Unit.Direction.Down);
                }
              }
              else if (differenceInXPosition > differenceInYPosition)
              { //Move horizontal
                if (lUnit.xPos <= lTarget.xPos)
                {
                  lUnit.Move(Unit.Direction.Right);
                }
                else if (lUnit.xPos > lTarget.xPos)
                {
                  lUnit.Move(Unit.Direction.Left);
                }
              }
              else
              {
                lUnit.Move(Unit.Direction.Up);
              }
            }
          }
          else if (lUnit.Health < 0.25 * lUnit.MaxHealth)
          {
            lUnit.Move(RandomDirection());
          }
        }
      }
      if (aUnit is MeleeUnit)
      {
        MeleeUnit lUnit = aUnit as MeleeUnit;
        IUnit closestEnemy = lUnit.FindClosestEnemy(map.Battlefield);

        if (closestEnemy != null && lUnit.Health >= 0.25 * lUnit.MaxHealth)
        {
          if (lUnit.RangeCheck(closestEnemy))
          {
            lUnit.EngageUnit(closestEnemy);
            lUnit.IsAttacking = true;
          }
          else if (roundsCompleted % lUnit.Speed == 0)
          {
            if (closestEnemy is IPosition)
            { //Move toward the enemy
              var lTarget = closestEnemy as IPosition;
              int differenceInXPosition = Math.Abs(lUnit.xPos - lTarget.xPos);
              int differenceInYPosition = Math.Abs(lUnit.yPos - lTarget.yPos);
              if (differenceInXPosition > differenceInYPosition)
              { //Move vertical
                if (lUnit.yPos <= lTarget.yPos)
                {
                  lUnit.Move(Unit.Direction.Up);
                }
                else if (lUnit.yPos > lTarget.yPos)
                {
                  lUnit.Move(Unit.Direction.Down);
                }
              }
              else if (differenceInXPosition > differenceInYPosition)
              { //Move horizontal
                if (lUnit.xPos <= lTarget.xPos)
                {
                  lUnit.Move(Unit.Direction.Right);
                }
                else if (lUnit.xPos > lTarget.xPos)
                {
                  lUnit.Move(Unit.Direction.Left);
                }
              }
              else
              {
                lUnit.Move(Unit.Direction.Up);
              }
            }
          }
        }
        else if (lUnit.Health < 0.25 * lUnit.MaxHealth)
        {
          lUnit.Move(RandomDirection());
        }
      }
    }

    private Unit.Direction RandomDirection()
    {
      int r = rand.Next(0, 4);
      switch (r)
      {
        case 0:
          return Unit.Direction.Up;
        case 1:
          return Unit.Direction.Down;
        case 2:
          return Unit.Direction.Left;
        case 3:
          return Unit.Direction.Right;
        default:
          return Unit.Direction.Up;
      }
    }

    public void SaveUnits()
    {
      foreach (Unit unit in map.units)
      {
        unit.SaveToFile();
      }
    }

    public void LoadUnits()
    {
      List<Unit> lUnits = new List<Unit>();

      foreach (var lFilname in GameSettings.GetUnitFilenameList)
      {
        Unit lUnit = LoadUnitFromFilename(lFilname);

        lUnits.Add(lUnit);
      }
      map.units = lUnits;
    }

    public T LoadUnit<T>(string aFileName)
      where T : Unit, new()
    {
      if (!File.Exists(aFileName))
        return null;

      ClassSerialisation<T> CS = new ClassSerialisation<T>();

      var lUnit = CS.LoadClass(aFileName);

      return lUnit;
    }

    public Unit LoadUnitFromFilename(string aFilename)
    {
      FileInfo lFileInfo = new FileInfo(aFilename);
      string lName = lFileInfo.Name.Replace(".txt", "");

      var lUnitClassList = typeof(Unit).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Unit)));

      foreach (var lUnitClass in lUnitClassList)
      {
        if (lName.StartsWith(lUnitClass.Name))
        {
          Type lType = Type.GetType($"POE_RTS_WinForm.{lUnitClass.Name}");

          var lMethodInfo = this.GetType().GetMethod("LoadUnit");
          var lMethod = lMethodInfo.MakeGenericMethod(new Type[] { lType });

          Unit lUnit = lMethod.Invoke(this, new object[] { aFilename }) as Unit;

          return lUnit;
        }
      }

      return null;
    }
  }
}
