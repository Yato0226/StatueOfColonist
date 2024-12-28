// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.GeneData
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using System.Collections.Generic;
using Verse;

 
namespace StatueOfColonist
{
  public class GeneData : IExposable
  {
    public GeneDef geneDef;
    public int geneGraphicIndex;
    public Gender gender;

    public GeneData()
    {
    }

    public GeneData(GeneDef geneDef, int geneGraphicIndex, Gender gender)
    {
      this.geneDef = geneDef;
      this.geneGraphicIndex = geneGraphicIndex;
      this.gender = gender;
    }

    public string GraphicPath
    {
      get
      {
        GeneGraphicData graphicData = this.geneDef.graphicData;
        if (!GenList.NullOrEmpty<string>((IList<string>) graphicData.graphicPaths) && this.geneGraphicIndex >= 0)
          return graphicData.graphicPaths[this.geneGraphicIndex];
        return this.gender == 2 && !GenText.NullOrEmpty(graphicData.graphicPathFemale) ? graphicData.graphicPathFemale : graphicData.graphicPath;
      }
    }

    public TaggedString Label
    {
      get
      {
        TaggedString labelCap = ((Def) this.geneDef).LabelCap;
        GeneGraphicData graphicData = this.geneDef.graphicData;
        if (!GenList.NullOrEmpty<string>((IList<string>) graphicData.graphicPaths) && this.geneGraphicIndex >= 0)
          return TaggedString.op_Implicit(TaggedString.op_Implicit(TaggedString.op_Addition(labelCap, "_")) + this.geneGraphicIndex.ToString());
        Log.Message(graphicData.graphicPathFemale + "/" + this.gender.ToString());
        return this.gender == 2 && !GenText.NullOrEmpty(graphicData.graphicPathFemale) ? TaggedString.op_Addition(labelCap, Translator.Translate("StatueOfColonist.FemaleGeneTexture")) : labelCap;
      }
    }

    public void ExposeData()
    {
      Scribe_Defs.Look<GeneDef>(ref this.geneDef, "geneDef");
      Scribe_Values.Look<int>(ref this.geneGraphicIndex, "geneGraphicIndex", 0, false);
      Scribe_Values.Look<Gender>(ref this.gender, "gender", (Gender) 0, false);
    }
  }
}
