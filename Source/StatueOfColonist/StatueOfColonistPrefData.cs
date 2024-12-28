// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistPrefData
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using System;
using System.Collections.Generic;
using Verse;

 
namespace StatueOfColonist
{
  public class StatueOfColonistPrefData : IExposable
  {
    public List<StatueOfColonistPreset> presets = new List<StatueOfColonistPreset>();

    public void ExposeData()
    {
      Scribe_Collections.Look<StatueOfColonistPreset>(ref this.presets, "presets", (LookMode) 2, Array.Empty<object>());
    }
  }
}
