// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistGraphicSet
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

 
namespace StatueOfColonist
{
  public class StatueOfColonistGraphicSet
  {
    public Building_StatueOfColonist parent;
    public StatueOfColonistData data;
    public Graphic nakedGraphic;
    public Graphic rottingGraphic;
    public Graphic dessicatedGraphic;
    public Graphic headGraphic;
    public Graphic desiccatedHeadGraphic;
    public Graphic skullGraphic;
    public Graphic headStumpGraphic;
    public Graphic desiccatedHeadStumpGraphic;
    public Graphic hairGraphic;
    public Graphic beardGraphic;
    public Graphic bodyTattooGraphic;
    public Graphic faceTattooGraphic;
    public Graphic_Shadow shadowGraphic;
    public List<Graphic> bodyAddonGraphics;
    public List<ApparelGraphicRecord> apparelGraphics = new List<ApparelGraphicRecord>();
    private List<Material> cachedMatsBodyBase = new List<Material>();
    public List<GeneGraphic> geneGraphics = new List<GeneGraphic>();
    public Graphic furCoveredGraphic;
    private int cachedMatsBodyBaseHash = -1;
    public static readonly Color RottingColor = new Color(0.34f, 0.32f, 0.3f);
    public bool forceResolve;

    public bool AllResolved => this.nakedGraphic != null;

    public bool ShouldResolve => !this.AllResolved || this.forceResolve;

    public GraphicMeshSet HairMeshSet
    {
      get
      {
        Vector2 hairMeshSize = this.data.crownType.hairMeshSize;
        return MeshPool.GetMeshSetForWidth(hairMeshSize.x);
      }
    }

    public StatueOfColonistGraphicSet(
      StatueOfColonistData data,
      Building_StatueOfColonist parent,
      bool forceResolve = false)
    {
      this.data = data;
      this.parent = parent;
      this.nakedGraphic = (Graphic) null;
      this.forceResolve = forceResolve;
    }

public List<Material> MatsBodyBaseAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh)
    {
      int num = facing.AsInt + 1000 * (int)bodyCondition;
      if (num != this.cachedMatsBodyBaseHash)
      {
        this.cachedMatsBodyBase.Clear();
        this.cachedMatsBodyBaseHash = num;
        if (bodyCondition == null)
          this.cachedMatsBodyBase.Add(this.nakedGraphic.MatAt(facing, (Thing) null));
        else if (bodyCondition == 1 || this.dessicatedGraphic == null)
          this.cachedMatsBodyBase.Add(this.rottingGraphic.MatAt(facing, (Thing) null));
        else if (bodyCondition == 2)
          this.cachedMatsBodyBase.Add(this.dessicatedGraphic.MatAt(facing, (Thing) null));
        for (int index = 0; index < this.apparelGraphics.Count; ++index)
        {
          if ((((Thing) this.apparelGraphics[index].sourceApparel).def.apparel.shellRenderedBehindHead || ((Thing) this.apparelGraphics[index].sourceApparel).def.apparel.LastLayer != ApparelLayerDefOf.Shell) && !PawnRenderer.RenderAsPack(this.apparelGraphics[index].sourceApparel) && ((Thing) this.apparelGraphics[index].sourceApparel).def.apparel.LastLayer != ApparelLayerDefOf.Overhead)
            this.cachedMatsBodyBase.Add(this.apparelGraphics[index].graphic.MatAt(facing, (Thing) null));
        }
      }
      return this.cachedMatsBodyBase;
    }

    public Material HeadMatAt(Rot4 facing, RotDrawMode bodyCondition = 0, bool stump = false)
    {
      Material material = (Material) null;
      if (bodyCondition == null)
        material = !stump ? this.headGraphic.MatAt(facing, (Thing) null) : this.headStumpGraphic.MatAt(facing, (Thing) null);
      else if (bodyCondition == 1)
        material = !stump ? this.desiccatedHeadGraphic.MatAt(facing, (Thing) null) : this.desiccatedHeadStumpGraphic.MatAt(facing, (Thing) null);
      else if (bodyCondition == 2 && !stump)
        material = this.skullGraphic.MatAt(facing, (Thing) null);
      return material;
    }

    public Material HairMatAt(Rot4 facing)
    {
      return this.hairGraphic == null ? (Material) null : this.hairGraphic.MatAt(facing, (Thing) null);
    }

    public void ClearCache() => this.cachedMatsBodyBaseHash = -1;

    public void ResolveAllGraphics(float scale = 1f)
    {
      Shader shader = ShaderDatabase.LoadShader(this.data.shaderCutoutPath);
      this.ClearCache();
      this.nakedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.data.bodyType.bodyNakedGraphicPath, ShaderDatabase.CutoutSkin, Vector2.one, this.data.color);
      this.rottingGraphic = GraphicDatabase.Get<Graphic_Multi>(this.data.bodyType.bodyNakedGraphicPath, ShaderDatabase.CutoutSkin, Vector2.one, this.data.color);
      this.dessicatedGraphic = GraphicDatabase.Get<Graphic_Multi>(this.data.bodyType.bodyDessicatedGraphicPath, ShaderDatabase.Cutout);
      this.headGraphic = (Graphic) this.data.crownType.GetGraphic(this.data.color, false, false);
      this.desiccatedHeadGraphic = (Graphic) this.data.crownType.GetGraphic(this.data.color, true, false);
      this.skullGraphic = (Graphic) HeadTypeDefOf.Skull.GetGraphic(Color.white, true, false);
      this.headStumpGraphic = (Graphic) HeadTypeDefOf.Stump.GetGraphic(this.data.color, false, false);
      this.desiccatedHeadStumpGraphic = (Graphic) HeadTypeDefOf.Stump.GetGraphic(this.data.color, true, false);
      if (this.data.hairGraphicPath != null)
        this.hairGraphic = GraphicDatabase.Get<Graphic_Multi>(this.data.hairGraphicPath, shader, Vector2.one, this.data.color);
      if (this.bodyAddonGraphics == null)
        this.bodyAddonGraphics = new List<Graphic>();
      this.ResolveTatooGraphic();
      this.ResolveBoardGraphic();
      this.ResolveApparelGraphics();
      this.ResolveGeneGraphics();
      if (ModLister.BiotechInstalled)
      {
        FurDef fur = this.data.Fur;
        if (fur != null)
        {
          string str = (string) null;
          for (int index = 0; index < fur.bodyTypeGraphicPaths.Count; ++index)
          {
            if (fur.bodyTypeGraphicPaths[index].bodyType == this.data.bodyType)
              str = fur.bodyTypeGraphicPaths[index].texturePath;
          }
          Log.Message(str);
          if (str != null)
            this.furCoveredGraphic = GraphicDatabase.Get<Graphic_Multi>(str, ShaderDatabase.CutoutSkinOverlay, Vector2.one, this.data.color);
        }
        else
          this.furCoveredGraphic = (Graphic) null;
      }
      this.forceResolve = false;
    }

    public void ResolveTatooGraphic()
    {
      if (!ModsConfig.IdeologyActive)
        return;
      Color color = this.data.color;
      color.a *= 0.8f;
      this.faceTattooGraphic = this.data.faceTattooDef == null || this.data.faceTattooDef == TattooDefOf.NoTattoo_Face ? (Graphic) null : GraphicDatabase.Get<Graphic_Multi>(((StyleItemDef) this.data.faceTattooDef).texPath, ShaderDatabase.CutoutSkinOverlay, Vector2.one, color, Color.white, (GraphicData) null, this.data.crownType.graphicPath);
      if (this.data.bodyTattooDef != null && this.data.bodyTattooDef != TattooDefOf.NoTattoo_Body)
        this.bodyTattooGraphic = GraphicDatabase.Get<Graphic_Multi>(((StyleItemDef) this.data.bodyTattooDef).texPath, ShaderDatabase.CutoutSkinOverlay, Vector2.one, color, Color.white, (GraphicData) null, this.data.bodyType.bodyNakedGraphicPath);
      else
        this.bodyTattooGraphic = (Graphic) null;
    }

    public void ResolveBoardGraphic()
    {
      if (this.data.beardDef == null)
        return;
      Color color = this.data.color;
      color.r *= this.data.beardBlightness;
      color.g *= this.data.beardBlightness;
      color.b *= this.data.beardBlightness;
      if (((StyleItemDef) this.data.beardDef).noGraphic)
        this.beardGraphic = (Graphic) null;
      else
        this.beardGraphic = GraphicDatabase.Get<Graphic_Multi>(((StyleItemDef) this.data.beardDef).texPath, ShaderDatabase.Transparent, Vector2.one, color);
    }

    public void ResolveApparelGraphics()
    {
      Shader shader = ShaderDatabase.LoadShader(this.data.shaderCutoutPath);
      this.ClearCache();
      this.apparelGraphics.Clear();
      foreach (ThingDef wornApparelDef in this.data.wornApparelDefs)
      {
        ApparelGraphicRecord rec;
        if (StatueOfColonistGraphicSet.TryGetGraphicApparel(StatueOfColonistGraphicSet.MakeApparel(wornApparelDef, this.data.color), this.data.color, this.data.bodyType, shader, out rec))
          this.apparelGraphics.Add(rec);
      }
    }

    public void ResolveGeneGraphics()
    {
      if (!ModsConfig.BiotechActive || this.data.genes == null)
        return;
      Shader skinShader = ShaderUtility.GetSkinShader(false);
      this.geneGraphics.Clear();
      foreach (GeneData gene in this.data.genes)
      {
        string graphicPath = gene.GraphicPath;
        if (!GenText.NullOrEmpty(graphicPath))
        {
          Color color = this.data.color;
          this.geneGraphics.Add(new GeneGraphic(GraphicDatabase.Get<Graphic_Multi>(graphicPath, skinShader, Vector2.one, color, Color.white), gene.geneDef));
        }
      }
    }

    private static Apparel MakeApparel(ThingDef def, Color color)
    {
      Apparel apparel = (Apparel) ThingMaker.MakeThing(def, GenStuff.DefaultStuffFor((BuildableDef) def));
      CompColorableUtility.SetColor((Thing) apparel, color, false);
      return apparel;
    }

    public static bool TryGetGraphicApparel(
      Apparel apparel,
      Color color,
      BodyTypeDef bodyType,
      Shader shader,
      out ApparelGraphicRecord rec)
    {
      if (bodyType == null)
      {
        Log.Error("Getting apparel graphic with undefined body type.");
        bodyType = BodyTypeDefOf.Male;
      }
      if (GenText.NullOrEmpty(((Thing) apparel).def.apparel.wornGraphicPath))
      {
        rec = new ApparelGraphicRecord((Graphic) null, (Apparel) null);
        return false;
      }
      Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(((Thing) apparel).def.apparel.LastLayer == ApparelLayerDefOf.Overhead || PawnRenderer.RenderAsPack(apparel) || apparel.WornGraphicPath == BaseContent.PlaceholderImagePath || apparel.WornGraphicPath == BaseContent.PlaceholderGearImagePath ? ((Thing) apparel).def.apparel.wornGraphicPath : ((Thing) apparel).def.apparel.wornGraphicPath + "_" + bodyType.ToString(), shader, ((Thing) apparel).def.graphicData.drawSize, color);
      rec = new ApparelGraphicRecord(graphic, apparel);
      return true;
    }

    public Material FurMatAt(Rot4 facing)
    {
      return this.furCoveredGraphic == null ? (Material) null : this.furCoveredGraphic.MatAt(facing, (Thing) null);
    }
  }
}
