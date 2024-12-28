// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.GeneGraphic
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using Verse;

 
namespace StatueOfColonist
{
  public class GeneGraphic
  {
    public Graphic graphic;
    public GeneDef sourceGene;

    public GeneGraphic(Graphic graphic, GeneDef sourceGene)
    {
      this.graphic = graphic;
      this.sourceGene = sourceGene;
    }
  }
}
