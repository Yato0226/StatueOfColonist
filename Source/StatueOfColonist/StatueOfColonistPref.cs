// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistPref
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using System;
using System.Collections.Generic;
using System.IO;
using Verse;

#nullable disable
namespace StatueOfColonist
{
  public static class StatueOfColonistPref
  {
    public static List<StatueOfColonistPreset> presets = new List<StatueOfColonistPreset>();
    public static readonly string PrefFilePath = Path.Combine(GenFilePaths.ConfigFolderPath, "StatueOfColonist.xml");

    public static void LoadPref()
    {
      if (!File.Exists(StatueOfColonistPref.PrefFilePath))
      {
        Log.Message(StatueOfColonistPref.PrefFilePath + " is not found.");
      }
      else
      {
        try
        {
          Scribe.loader.InitLoading(StatueOfColonistPref.PrefFilePath);
          try
          {
            ScribeMetaHeaderUtility.LoadGameDataHeader((ScribeMetaHeaderUtility.ScribeHeaderMode) 0, true);
            List<StatueOfColonistPreset> ofColonistPresetList = new List<StatueOfColonistPreset>();
            Scribe_Collections.Look<StatueOfColonistPreset>(ref ofColonistPresetList, "presets", (LookMode) 2, Array.Empty<object>());
            StatueOfColonistPref.presets = ofColonistPresetList;
            Scribe.loader.FinalizeLoading();
          }
          catch
          {
            Scribe.ForceStop();
            throw;
          }
        }
        catch (Exception ex)
        {
          Log.Error("Exception loading StatueOfColonistPref: " + ex.ToString());
          StatueOfColonistPref.presets = new List<StatueOfColonistPreset>();
          Scribe.ForceStop();
        }
      }
    }

    public static void SavePref()
    {
      try
      {
        SafeSaver.Save(StatueOfColonistPref.PrefFilePath, "statueOfColonistPref", (Action) (() =>
        {
          ScribeMetaHeaderUtility.WriteMetaHeader();
          List<StatueOfColonistPreset> presets = StatueOfColonistPref.presets;
          Scribe_Collections.Look<StatueOfColonistPreset>(ref presets, "presets", (LookMode) 2, Array.Empty<object>());
        }), false);
      }
      catch (Exception ex)
      {
        Log.Error("Exception while saving world: " + ex.ToString());
      }
    }
  }
}
