using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POE_RTS_WinForm
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }
    public GameEngine GE;

    private int numberOfUnits = 12;
    private int numberOfBuildings = 4;
    //private float waitTime = 1;
    //private float timeElapsed = 0;
    //private bool isPaused = true;

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (GE == null)
      {
        GE = new GameEngine(numberOfUnits, numberOfBuildings);
        GE.StartGame();
      }

      timer1.Enabled = true;
    }

    private void time1_Tick(object sender, EventArgs e)
    {
      if ((GE.map.units.Count > 1))
      {
        GE.StartNewRound();
        rtbMap.Text = GE.map.PrintMap();
        lblRoundCount.Text = $"Round: {GE.roundsCompleted-1}";

        UpdateUnitInfo();
      }      
    }

    private void UpdateUnitInfo()
    {
      rtbUnitInfo.Text = "";
      foreach (var unit in GE.map.units)
      {
        rtbUnitInfo.Text += $"{unit.ToString()}{Environment.NewLine}";
      }
      foreach (var building in GE.map.buildings)
      {
        rtbUnitInfo.Text += $"{building.ToString()}{Environment.NewLine}";
      }
    }

    private void btnPause_Click(object sender, EventArgs e)
    {
      timer1.Enabled = false;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      GE.SaveUnits();
    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
      GE.LoadUnits();
    }
  }
}
