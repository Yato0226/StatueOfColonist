// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistSettings
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

 
namespace StatueOfColonist
{
  public class StatueOfColonistSettings : ModSettings
  {
    public bool autoName = true;
    public bool showClothForDisplay = true;
    public float clothIconSize = 64f;
    private Dictionary<string, float> scaleOfCloth = new Dictionary<string, float>();
    public float possibilityOfStatueFromPreset;
    public List<float> offsetStatueBody = new List<float>();

    public StatueOfColonistSettings() => this.InitScaleOfCloth();

    public float GetScaleOfCloth(BodyTypeDef bodyType)
    {
      return !this.scaleOfCloth.ContainsKey(((Def) bodyType).defName) ? 1f : this.scaleOfCloth[((Def) bodyType).defName];
    }

    public void SetScaleOfCloth(BodyTypeDef bodyType, float scale)
    {
      this.scaleOfCloth[((Def) bodyType).defName] = scale;
    }

    public float GetOffsetStatueBody(BodyTypeDef bodyType, int size)
    {
      int index = bodyType.ToInt() * 3 + size;
      return index >= this.offsetStatueBody.Count || index < 0 ? 0.0f : this.offsetStatueBody[index];
    }

    public void Refresh()
    {
      if (this.scaleOfCloth == null || this.scaleOfCloth.Count < 6)
        this.InitScaleOfCloth();
      if (this.offsetStatueBody != null && this.offsetStatueBody.Count >= 15)
        return;
      this.InitOffsetStatueBody();
    }

    public virtual void ExposeData()
    {
      Scribe_Values.Look<bool>(ref this.autoName, "autoName", false, false);
      Scribe_Values.Look<bool>(ref this.showClothForDisplay, "showClothForDisplay", false, false);
      Scribe_Values.Look<float>(ref this.clothIconSize, "clothIconSize", 64f, false);
      Scribe_Collections.Look<string, float>(ref this.scaleOfCloth, "scaleOfCloth", (LookMode) 0, (LookMode) 0);
      Scribe_Values.Look<float>(ref this.possibilityOfStatueFromPreset, "possibilityOfStatueFromPreset", 0.0f, false);
      Scribe_Collections.Look<float>(ref this.offsetStatueBody, "offsetStatueBody", (LookMode) 0, Array.Empty<object>());
      if (Scribe.mode != 2)
        return;
      this.Refresh();
    }

    public void InitScaleOfCloth()
    {
      this.scaleOfCloth = new Dictionary<string, float>();
      this.scaleOfCloth["Male"] = 1f;
      this.scaleOfCloth["Female"] = 1f;
      this.scaleOfCloth["Thin"] = 1f;
      this.scaleOfCloth["Hulk"] = 1f;
      this.scaleOfCloth["Fat"] = 1f;
    }

    public void InitOffsetStatueBody()
    {
      this.offsetStatueBody = new List<float>(15);
      this.offsetStatueBody.Add(0.0f);
      this.offsetStatueBody.Add(0.0f);
      this.offsetStatueBody.Add(-0.1f);
      this.offsetStatueBody.Add(0.0f);
      this.offsetStatueBody.Add(0.05f);
      this.offsetStatueBody.Add(0.0f);
      this.offsetStatueBody.Add(0.0f);
      this.offsetStatueBody.Add(0.0f);
      this.offsetStatueBody.Add(-0.1f);
      this.offsetStatueBody.Add(0.2f);
      this.offsetStatueBody.Add(0.3f);
      this.offsetStatueBody.Add(0.3f);
      this.offsetStatueBody.Add(0.05f);
      this.offsetStatueBody.Add(0.05f);
      this.offsetStatueBody.Add(0.0f);
    }
  }
}
