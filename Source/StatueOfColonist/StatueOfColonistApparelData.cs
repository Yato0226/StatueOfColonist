// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistApparelData
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using Verse;

 
namespace StatueOfColonist
{
  public class StatueOfColonistApparelData : IExposable
  {
    public ThingDef apparelDef;
    public string apparelDefName;

    public bool IsValid => this.apparelDef != null;

    public StatueOfColonistApparelData()
    {
    }

    public StatueOfColonistApparelData(ThingDef apparel) => this.apparelDef = apparel;

    public void ExposeData()
    {
      if (Scribe.mode == 1 && this.apparelDef != null)
        this.apparelDefName = ((Def) this.apparelDef).defName;
      Scribe_Values.Look<string>(ref this.apparelDefName, "apparelDef", (string) null, false);
      if (Scribe.mode != 2)
        return;
      this.apparelDef = DefDatabase<ThingDef>.GetNamed(this.apparelDefName, false);
      if (this.apparelDef != null)
        return;
      Log.Warning("apparelDef \"" + this.apparelDefName + "\" is not found.");
    }
  }
}
