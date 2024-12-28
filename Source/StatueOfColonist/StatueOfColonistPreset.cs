// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistPreset
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

#nullable disable
namespace StatueOfColonist
{
  public class StatueOfColonistPreset : IExposable
  {
    public string name;
    public BodyTypeDef bodyType;
    public string headGraphicPath;
    public string hairDefName;
    public List<StatueOfColonistApparelData> apparels = new List<StatueOfColonistApparelData>();
    public float weight = 1f;
    public string raceDefName;
    public string lifeStageDefName;
    public Gender gender;
    public HeadTypeDef headType;
    public HairDef hairDef;
    public ThingDef raceDef;
    public LifeStageDef lifeStageDef;
    public BeardDef beardDef;
    public float beardBlightness;
    public TattooDef faceTattooDef;
    public TattooDef bodyTattooDef;
    public List<GeneData> genes = new List<GeneData>();
    public string alienCrownType;
    public List<int> addonVariants;
    public string bufferWeight = "1.0";

    public bool IsValid
    {
      get
      {
        return this.apparels.All<StatueOfColonistApparelData>((Func<StatueOfColonistApparelData, bool>) (ap => ap.IsValid)) && this.hairDef != null;
      }
    }

    public StatueOfColonistPreset() => this.apparels = new List<StatueOfColonistApparelData>();

    public StatueOfColonistPreset(string name, Building_StatueOfColonist statue)
    {
      this.name = name;
      this.SetStatueOfColonist(statue);
    }

    public void SetStatueOfColonist(Building_StatueOfColonist statue)
    {
      this.bodyType = statue.Data.bodyType;
      this.headGraphicPath = statue.Data.headGraphicPath;
      this.hairDefName = ((Def) Util.GetHairDefFromGraphicPath(statue.Data.hairGraphicPath)).defName;
      this.gender = statue.Data.gender;
      this.apparels = new List<StatueOfColonistApparelData>();
      foreach (ThingDef wornApparelDef in statue.Data.wornApparelDefs)
        this.apparels.Add(new StatueOfColonistApparelData(wornApparelDef));
      this.beardDef = statue.Data.beardDef;
      this.beardBlightness = statue.Data.beardBlightness;
      this.faceTattooDef = statue.Data.faceTattooDef;
      this.bodyTattooDef = statue.Data.bodyTattooDef;
      this.headType = statue.Data.crownType;
      this.hairDef = DefDatabase<HairDef>.GetNamed(this.hairDefName, false);
      this.raceDef = statue.Data.raceDef;
      this.raceDefName = ((Def) this.raceDef).defName;
      this.lifeStageDef = statue.Data.lifeStageDef;
      this.lifeStageDefName = ((Def) this.lifeStageDef).defName;
      if (GenList.NullOrEmpty<GeneData>((IList<GeneData>) statue.Data.genes))
        this.genes = new List<GeneData>();
      else
        this.genes = new List<GeneData>((IEnumerable<GeneData>) statue.Data.genes);
    }

    public void ExposeData()
    {
      Scribe_Values.Look<string>(ref this.name, "name", (string) null, false);
      Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
      Scribe_Values.Look<string>(ref this.headGraphicPath, "headGraphicPath", (string) null, false);
      Scribe_Values.Look<string>(ref this.hairDefName, "hairDefName", (string) null, false);
      Scribe_Values.Look<float>(ref this.weight, "weight", 1f, false);
      Scribe_Values.Look<string>(ref this.raceDefName, "raceDefName", "Human", false);
      Scribe_Values.Look<string>(ref this.lifeStageDefName, "lifeStageDefName", "HumanlikeAdult", false);
      Scribe_Values.Look<Gender>(ref this.gender, "gender", (Gender) 1, false);
      Scribe_Collections.Look<StatueOfColonistApparelData>(ref this.apparels, "appearanceClothes", (LookMode) 2, Array.Empty<object>());
      Scribe_Defs.Look<BeardDef>(ref this.beardDef, "beardDef");
      Scribe_Values.Look<float>(ref this.beardBlightness, "beardBlightness", 1f, false);
      Scribe_Defs.Look<TattooDef>(ref this.faceTattooDef, "faceTattooDef");
      Scribe_Defs.Look<TattooDef>(ref this.bodyTattooDef, "bodyTattooDef");
      Scribe_Values.Look<string>(ref this.alienCrownType, "alienCrownType", (string) null, false);
      Scribe_Collections.Look<int>(ref this.addonVariants, "addonVariants", (LookMode) 0, Array.Empty<object>());
      if (this.raceDefName == "Human")
        Scribe_Defs.Look<HeadTypeDef>(ref this.headType, "headTypeDef");
      if (Scribe.mode == 2)
      {
        this.hairDef = DefDatabase<HairDef>.GetNamed(this.hairDefName, false);
        this.raceDef = DefDatabase<ThingDef>.GetNamed(this.raceDefName, false);
        this.lifeStageDef = DefDatabase<LifeStageDef>.GetNamed(this.lifeStageDefName, false);
        this.bufferWeight = this.weight.ToString();
      }
      Scribe_Collections.Look<GeneData>(ref this.genes, "genes", (LookMode) 2, Array.Empty<object>());
      if (Scribe.mode != 2 || this.genes != null)
        return;
      this.genes = new List<GeneData>();
    }

    public string GetToolTip()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.raceDef != ThingDefOf.Human)
      {
        if (this.raceDef != null)
          stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.RaceData", NamedArgument.op_Implicit(((Def) this.raceDef).LabelCap))));
        else
          stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.RaceIsNotFound", NamedArgument.op_Implicit(this.raceDefName))));
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.GenderData", NamedArgument.op_Implicit(GenderUtility.GetLabel(this.gender, false)))));
      }
      if (this.lifeStageDef != null)
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.LifeStageData", NamedArgument.op_Implicit(((Def) this.lifeStageDef).LabelCap))));
      else
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.LifeStageIsNotFound", NamedArgument.op_Implicit(this.lifeStageDefName))));
      if (this.hairDef != null)
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.HairData", NamedArgument.op_Implicit(((Def) this.hairDef).LabelCap))));
      else
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.HairIsNotFound", NamedArgument.op_Implicit(this.hairDefName))));
      if (this.raceDef == ThingDefOf.Human)
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.HeadData", NamedArgument.op_Implicit(this.headType != null ? ((Def) this.headType).LabelCap : new TaggedString("null")))));
      else
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.HeadData", NamedArgument.op_Implicit(this.alienCrownType))));
      stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.BodyData", NamedArgument.op_Implicit(this.bodyType.GetName()))));
      if (ModsConfig.IdeologyActive)
      {
        if (this.faceTattooDef != null)
          stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.FaceTatooData", NamedArgument.op_Implicit(((Def) this.faceTattooDef).LabelCap))));
        if (this.bodyTattooDef != null)
          stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.BodyTatooData", NamedArgument.op_Implicit(((Def) this.bodyTattooDef).LabelCap))));
      }
      if (this.beardDef != null)
      {
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.BeardData", NamedArgument.op_Implicit(((Def) this.beardDef).LabelCap))));
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.BeardBlightnessData", NamedArgument.op_Implicit(this.beardBlightness))));
      }
      foreach (StatueOfColonistApparelData apparel in this.apparels)
      {
        if (apparel.IsValid)
          stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.ApparelData", NamedArgument.op_Implicit(((Def) apparel.apparelDef).LabelCap))));
      }
      if (GenCollection.Any<StatueOfColonistApparelData>(this.apparels, (Predicate<StatueOfColonistApparelData>) (ap => !ap.IsValid)))
      {
        if (stringBuilder.Length > 0)
          stringBuilder.AppendLine();
        foreach (StatueOfColonistApparelData apparel in this.apparels)
        {
          if (!apparel.IsValid)
            stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.ApparelIsNotFound", NamedArgument.op_Implicit(apparel.apparelDefName))));
        }
      }
      foreach (GeneData gene in this.genes)
        stringBuilder.AppendLine(TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.GeneData", NamedArgument.op_Implicit(gene.Label))));
      return stringBuilder.ToString();
    }
  }
}
