using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;

namespace POE_RTS_WinForm
{
  public class ClassSerialisation<T>
    where T : class
  {
    public void SaveClass(string aFilePath, T aClassToSave)
    {
      BinaryFormatter bf = new BinaryFormatter();

      using (FileStream fs = new FileStream(aFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
      {
        try
        {
          bf.Serialize(fs, aClassToSave);
        }
        catch (Exception e)
        {
          MessageBox.Show($"Single Class Saving Error.{Environment.NewLine}{e}");
        }
      }
    }

    public void SaveClassListToFile(string aFileDirectory, List<T> list)
    {
      BinaryFormatter bf = new BinaryFormatter();
      for (int i = 0; i < list.Count; i++)
      {
        string filePath = aFileDirectory + i.ToString();
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        {
          try
          {
            bf.Serialize(fs, list[i]);
          }
          catch (Exception e)
          {
            MessageBox.Show($"Multiple Class Saving Error at position {i}.{Environment.NewLine}{e}");
          }
        }
      }
    }

    public T LoadClass(string aFileDirectory)
    {
      T lClass = null;
      BinaryFormatter bf = new BinaryFormatter();
      using (FileStream fs = new FileStream(aFileDirectory, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
      {
        try
        {
          lClass = (T)bf.Deserialize(fs);
        }
        catch (Exception e)
        {
          MessageBox.Show($"Single Class Loading Error.{Environment.NewLine}{e}");
        }
      }
      return lClass;
    }

    public List<T> LoadClassList(List<string> aFileDirectoryList)
    {
      List<T> lClassList = null;
      BinaryFormatter bf = new BinaryFormatter();
      foreach (string directory in aFileDirectoryList)
      {
        using (FileStream fs = new FileStream(directory, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        {
          try
          {
            lClassList.Add((T)bf.Deserialize(fs));
          }
          catch (Exception e)
          {
            MessageBox.Show($"Single Class Loading Error.{Environment.NewLine}{e}");
          }
        }
      }
      return lClassList;
    }
  }
}
