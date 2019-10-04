using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POE_RTS_WinForm
{
  [Serializable]
  public class RangedUnit : Unit, IPosition, IUnit
  {
    public RangedUnit(string aName, int aPositionX, int aPositionY, int aHealth, int aSpeed, int aAttack, int aAttackRange, string aFaction, char aSymbol)

      : base(aName, aPositionX, aPositionY, aHealth, aSpeed, aAttack, aFaction, aSymbol)
    {
      this.attackRange = aAttackRange;
      this.xPosition = aPositionX;
      this.yPosition = aPositionY;
      this.health = aHealth;
      this.maxHealth = aHealth;
      this.speed = aSpeed;
      this.attack = aAttack;
      this.faction = aFaction;
      this.symbol = aSymbol;
      this.name = aName;
    }
    public RangedUnit()
    {

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
          base.xPosition = Map.gridSize-1;
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
          base.yPosition = Map.gridSize-1;
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
          KillUnit();
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

    public int Speed
    {
      get
      {
        if (base.speed > 0)
        {
        return base.speed;
        }
        else
        {
          return 3;
        }
      }
      set
      {
        base.speed = value;
        if (speed < 0)
        {
          speed = 1;
        }
      }
    }

    public int Attack
    {
      get
      {
        return base.attack;
      }
      set
      {
        base.attack = value;
        if (base.attack < 0)
        {
          base.attack = 0;
        }
      }
    }
    public int AttackRange
    {
      get
      {
        return base.attackRange;
      }
      set
      {
        base.attackRange = value;
        if (base.attackRange < 0)
        {
          base.attackRange = 0;
        }
      }
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

    public bool IsAttacking
    {
      get { return base.isAttacking; }
      set { base.isAttacking = value; }
    }

    public override void SaveToFile()
    {
      var lClassSerialisation = new ClassSerialisation<RangedUnit>();

      lClassSerialisation.SaveClass(FileName, this);
    }

    public override void Move(Direction direction)
    {
      switch (direction)
      {
        case Direction.Up:
          this.yPos += 1;
          break;
        case Direction.Down:
          this.yPos -= 1;
          break;
        case Direction.Left:
          this.xPos -= 1;
          break;
        case Direction.Right:
          this.xPos -= 1;
          break;
        default:
          break;
      }
    }

    public override void DamageUnit(int aAttack)
    {
      this.Health = this.Health - aAttack;
    }

    public override void EngageUnit(IUnit aTarget)
    {
      isAttacking = true;
      aTarget.Health -= this.Attack;
    }

    public override bool RangeCheck(IUnit aTarget)
    {
      if (aTarget is IPosition)
      {
        var lTarget = aTarget as IPosition;

        int differenceInXPosition = Math.Abs(this.xPosition - lTarget.xPos);
        int differenceInYPosition = Math.Abs(this.yPosition - lTarget.yPos);

        if (differenceInXPosition <= attackRange && differenceInYPosition <= attackRange)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
      else
      {
        throw new ArgumentException("Unit doesn't have a Position");
      }
    }

    public override IUnit FindClosestEnemy(IUnit[,] aFieldToCheck)
    {
      Unit unitFound = null;

      int rangeToCheck = 1;
      int minRange;
      int maxRange;

      while (unitFound == null)
      {
        minRange = this.xPos - rangeToCheck;
        maxRange = this.xPos + rangeToCheck;

        if (minRange < 0)
        {
          minRange = 0;
        }
        if (maxRange > Map.gridSize)
        {
          maxRange = Map.gridSize;
        }

        //Check row
        for (int i = minRange; i < maxRange; i++)
        {
          if (aFieldToCheck[i, minRange] != null)
          {
            if (CheckEnemy(aFieldToCheck[i, minRange]))
            {
            return aFieldToCheck[i, minRange];
            }
          }
        }
        for (int i = minRange; i < maxRange; i++)
        {
          if (aFieldToCheck[i, maxRange - 1] != null)
          {
            if (CheckEnemy(aFieldToCheck[i, maxRange - 1]))
            {
            return aFieldToCheck[i, maxRange - 1];
            }
          }
        }

        minRange = this.yPos - rangeToCheck;
        maxRange = yPos + rangeToCheck;

        if (minRange < 0)
        {
          minRange = 0;
        }
        if (maxRange > Map.gridSize)
        {
          maxRange = Map.gridSize;
        }

        //Check column
        for (int i = minRange; i < maxRange; i++)
        {
          if (aFieldToCheck[i, maxRange - 1] != null)
          {
            if (CheckEnemy(aFieldToCheck[i, maxRange - 1]))
            {
              return aFieldToCheck[i, maxRange - 1];
            }
          }
        }
        for (int i = minRange; i < maxRange; i++)
        {
          if (aFieldToCheck[i, minRange] != null)
          {
            if (CheckEnemy(aFieldToCheck[i, minRange]))
            {
              return aFieldToCheck[i, minRange];
            }
          }
        }
        rangeToCheck++;
      }

      return null;
    }

    public override void KillUnit()
    {
      this.isDead = true;
    }

    public override string ToString()
    {
      string text;
      text = $"{name}: {Environment.NewLine}" +
             $"Position:({xPosition},{yPosition}).{Environment.NewLine}" +
             $" Health: {health}.{Environment.NewLine}" +
             $" Attack: {attack}.{Environment.NewLine}" +
             $"Attack Range: {attackRange}.{Environment.NewLine}";
      if (isAttacking)
      { 
        text += $"Is attacking.{Environment.NewLine}";
      }
      else
      {
        text += $"Is not attacking. {Environment.NewLine}";
      }

      text += $"Faction: {faction}.{Environment.NewLine}" +
              $" Symbol: {symbol}.{Environment.NewLine}";

      return text;
    }

    private bool CheckEnemy(IUnit unit)
    {
      if (unit is IUnit)
      {
        IUnit unitToCheck = unit as IUnit;
        if (this.Symbol == unitToCheck.Symbol)
        {
          return true;
        }
      }

      //if (unit is MeleeUnit)
      //{
      //  MeleeUnit unitToCheck = unit as MeleeUnit;
      //  if (this.Symbol == unitToCheck.Symbol)
      //  {
      //    return true;
      //  }
      //}

      return false;
    }
  }
}
