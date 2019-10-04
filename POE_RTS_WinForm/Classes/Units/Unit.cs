using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_RTS_WinForm
{
  [Serializable]
  public abstract class Unit
  {
    public Unit()
    {

    }

    public Unit(string aName, int aPositionX, int aPositionY, int aHealth, int aSpeed, int aAttack, string aFaction, char aSymbol)
    {
    }

    public string name;

    public enum Direction { Up, Down, Left, Right};
    protected int xPosition;
    protected int yPosition;

    protected int maxHealth;
    protected int health;

    protected int speed;

    protected int attack;
    protected int attackRange;

    protected string faction;
    protected char symbol;

    protected bool isAttacking;

    public bool isDead = false;

    public string FileName
    {
      get
      {
        return Path.Combine(GameSettings.ApplicationPath, GameSettings.sUnitsPath, $"{name}.txt");
      }
    }

    public abstract void Move(Direction direction);

    public abstract void EngageUnit(IUnit aTarget);

    public abstract bool RangeCheck(IUnit aTarget);

    public abstract IUnit FindClosestEnemy(IUnit[,] aFieldToCheck);

    public abstract void DamageUnit(int aAttack);

    public abstract void KillUnit();

    public abstract override string ToString();

    public abstract void SaveToFile();
  }
}
