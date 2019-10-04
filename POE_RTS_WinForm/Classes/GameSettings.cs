using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace POE_RTS_WinForm
{ //Improved version by Philip De Kock
  public class GameSettings
  {
    public const string sUnitsPath = "Units\\";

    public static string ApplicationPath
    {
      get
      {
        return Path.GetDirectoryName(Application.ExecutablePath);
      }
    }

    public static string UnitsPath
    {
      get
      {
        return Path.Combine(ApplicationPath, sUnitsPath);
      }
    }

    public static List<string> GetUnitFilenameList
    {
      get
      {
        string[] lFiles = Directory.GetFiles(UnitsPath, "*.txt");

        return lFiles.ToList<string>();
      }
    }
  }
}
