// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistData
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

 
namespace StatueOfColonist
{
  public class StatueOfColonistData : IExposable
  {
    public BodyTypeDef bodyType;
    public string headGraphicPath;
    public string hairGraphicPath;
    public HeadTypeDef crownType;
    public Color color;
    public string shaderCutoutPath;
    public List<ThingDef> wornApparelDefs;
    public ThingDef raceDef;
    public LifeStageDef lifeStageDef;
    public Gender gender;
    public Vector2 offset;
    public BeardDef beardDef = BeardDefOf.NoBeard;
    public float beardBlightness = 1f;
    public TattooDef faceTattooDef = TattooDefOf.NoTattoo_Face;
    public TattooDef bodyTattooDef = TattooDefOf.NoTattoo_Body;
    public List<GeneData> genes = new List<GeneData>();

    public FurDef Fur
    {
      get
      {
        foreach (GeneData gene in this.genes)
        {
          FurDef fur = gene.geneDef.graphicData?.fur;
          if (fur != null)
            return fur;
        }
        return (FurDef) null;
      }
    }

    public StatueOfColonistData()
    {
    }

    public StatueOfColonistData(Color color, string shaderCutoutPath, ThingDef raceDef = null)
    {
      this.color = color;
      this.shaderCutoutPath = shaderCutoutPath;
      this.lifeStageDef = DefDatabase<LifeStageDef>.GetNamed("HumanlikeAdult", true);
      this.raceDef = raceDef;
      if (this.raceDef != null)
        return;
      this.raceDef = ThingDefOf.Human;
    }

    public StatueOfColonistData(Pawn pawn, Color color, string shaderCutoutPath)
    {
      this.bodyType = this.GetBodyTypeDef(pawn);
      this.headGraphicPath = pawn.story.headType.graphicPath;
      this.hairGraphicPath = ((StyleItemDef) pawn.story.hairDef).texPath;
      this.crownType = pawn.story.headType;
      this.raceDef = ThingDefOf.Human;
      this.lifeStageDef = pawn.ageTracker.CurLifeStage;
      this.gender = pawn.gender;
      this.offset = Vector2.zero;
      if (pawn.style != null)
      {
        this.beardDef = pawn.style.beardDef;
        this.faceTattooDef = pawn.style.FaceTattoo;
        this.bodyTattooDef = pawn.style.BodyTattoo;
      }
      this.beardBlightness = 1f;
      this.color = color;
      this.shaderCutoutPath = shaderCutoutPath;
      this.wornApparelDefs = this.GetApparelDefs(pawn);
      this.genes = this.GetGenes(pawn);
    }

    public StatueOfColonistData(StatueOfColonistData source, Color color, string shaderCutoutPath)
    {
      if (source != null)
      {
        this.bodyType = source.bodyType;
        this.headGraphicPath = source.headGraphicPath;
        this.hairGraphicPath = source.hairGraphicPath;
        this.crownType = source.crownType;
        this.raceDef = source.raceDef;
        this.lifeStageDef = source.lifeStageDef;
        this.gender = source.gender;
        this.offset = source.offset;
        this.beardDef = source.beardDef;
        this.faceTattooDef = source.faceTattooDef;
        this.faceTattooDef = source.bodyTattooDef;
        this.beardBlightness = source.beardBlightness;
        this.wornApparelDefs = !GenList.NullOrEmpty<ThingDef>((IList<ThingDef>) source.wornApparelDefs) ? new List<ThingDef>((IEnumerable<ThingDef>) source.wornApparelDefs) : new List<ThingDef>();
        this.genes = !GenList.NullOrEmpty<GeneData>((IList<GeneData>) source.genes) ? new List<GeneData>((IEnumerable<GeneData>) source.genes) : new List<GeneData>();
      }
      this.color = color;
      this.shaderCutoutPath = shaderCutoutPath;
    }

    private BodyTypeDef GetBodyTypeDef(Pawn p) => p.story.bodyType;

    private List<ThingDef> GetApparelDefs(Pawn p)
    {
      return p.apparel.WornApparel.ConvertAll<ThingDef>((Converter<Apparel, ThingDef>) (ap => ((Thing) ap).def));
    }

    private List<GeneData> GetGenes(Pawn p)
    {
      List<GeneData> genes = new List<GeneData>();
      foreach (Gene gene in p.genes.GenesListForReading)
      {
        if (gene.def.HasGraphic || gene.def.graphicData?.fur != null)
        {
          int geneGraphicIndex = -1;
          if (!GenList.NullOrEmpty<string>((IList<string>) gene.def.graphicData.graphicPaths))
            geneGraphicIndex = ((Thing) p).thingIDNumber % gene.def.graphicData.graphicPaths.Count;
          genes.Add(new GeneData(gene.def, geneGraphicIndex, p.gender));
        }
      }
      return genes;
    }

    public void CopyFromPreset(StatueOfColonistPreset preset)
    {
      this.bodyType = preset.bodyType;
      this.headGraphicPath = preset.headGraphicPath;
      this.hairGraphicPath = ((StyleItemDef) preset.hairDef).texPath;
      this.raceDef = preset.raceDef;
      this.lifeStageDef = preset.lifeStageDef;
      this.gender = preset.gender;
      this.beardDef = preset.beardDef;
      if (this.beardDef == null)
        this.beardDef = BeardDefOf.NoBeard;
      this.faceTattooDef = preset.faceTattooDef;
      if (this.faceTattooDef == null)
        this.faceTattooDef = TattooDefOf.NoTattoo_Face;
      this.bodyTattooDef = preset.bodyTattooDef;
      if (this.bodyTattooDef == null)
        this.bodyTattooDef = TattooDefOf.NoTattoo_Body;
      this.beardBlightness = preset.beardBlightness;
      this.wornApparelDefs = preset.apparels.ConvertAll<ThingDef>((Converter<StatueOfColonistApparelData, ThingDef>) (data => data.apparelDef));
      if (GenList.NullOrEmpty<GeneData>((IList<GeneData>) preset.genes))
        this.genes = new List<GeneData>();
      else
        this.genes = new List<GeneData>((IEnumerable<GeneData>) preset.genes);
    }

    public void ExposeData()
    {
      Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
      Scribe_Values.Look<string>(ref this.headGraphicPath, "headGraphicPath", (string) null, false);
      Scribe_Values.Look<string>(ref this.hairGraphicPath, "hairGraphicPath", (string) null, false);
      Scribe_Defs.Look<HeadTypeDef>(ref this.crownType, "crownType");
      Scribe_Values.Look<Color>(ref this.color, "color", new Color(), false);
      Scribe_Values.Look<string>(ref this.shaderCutoutPath, "shaderCutoutPath", (string) null, false);
      Scribe_Defs.Look<ThingDef>(ref this.raceDef, "raceDef");
      Scribe_Defs.Look<LifeStageDef>(ref this.lifeStageDef, "lifeStageDef");
      Scribe_Values.Look<Gender>(ref this.gender, "gender", (Gender) 1, false);
      Scribe_Collections.Look<ThingDef>(ref this.wornApparelDefs, "wornApparelDefs", (LookMode) 4, Array.Empty<object>());
      Scribe_Values.Look<Vector2>(ref this.offset, "offset", new Vector2(), false);
      Scribe_Defs.Look<BeardDef>(ref this.beardDef, "beardDef");
      Scribe_Values.Look<float>(ref this.beardBlightness, "beardBlightness", 0.0f, false);
      Scribe_Defs.Look<TattooDef>(ref this.faceTattooDef, "faceTattooDef");
      Scribe_Defs.Look<TattooDef>(ref this.bodyTattooDef, "bodyTattooDef");
      if (Scribe.mode == 2 && this.raceDef == null)
      {
        this.raceDef = ThingDefOf.Human;
        this.lifeStageDef = DefDatabase<LifeStageDef>.GetNamed("HumanlikeAdult", true);
      }
      Scribe_Collections.Look<GeneData>(ref this.genes, "genes", (LookMode) 2, Array.Empty<object>());
      if (Scribe.mode != 2 || this.genes != null)
        return;
      this.genes = new List<GeneData>();
    }

    public bool CanWearWithoutDroppingAnything(ThingDef apDef)
    {
      for (int index = 0; index < this.wornApparelDefs.Count; ++index)
      {
        if (!ApparelUtility.CanWearTogether(apDef, this.wornApparelDefs[index], ThingDefOf.Human.race.body))
          return false;
      }
      return true;
    }
  }
}
