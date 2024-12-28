// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Window_EditStatueGene
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

 
namespace StatueOfColonist
{
  public class Window_EditStatueGene : Window
  {
    private Vector2 scrollPosition = Vector2.zero;
    private float scrollViewHeight;
    private static readonly Color ThingLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);
    private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Building_StatueOfColonist statue;

    public Building_StatueOfColonist Statue => this.statue;

    public virtual Vector2 InitialSize
    {
      get
      {
        float num = 656f;
        if (ModsConfig.IdeologyActive)
          num += 56f;
        return new Vector2(460f, num);
      }
    }

    public Window_EditStatueGene(Building_StatueOfColonist statue)
    {
      this.optionalTitle = TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.WindowEditStatueGene"));
      this.doCloseButton = true;
      this.doCloseX = true;
      this.forcePause = true;
      this.absorbInputAroundWindow = true;
      this.statue = statue;
    }

    public virtual void DoWindowContents(Rect inRect)
    {
      Text.Font = (GameFont) 1;
      Rect rect1 = GenUI.ContractedBy(inRect, 10f);
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(((Rect) ref rect1).x, ((Rect) ref rect1).y, ((Rect) ref rect1).width, ((Rect) ref rect1).height);
      GUI.BeginGroup(rect2);
      Text.Font = (GameFont) 1;
      GUI.color = Color.white;
      Rect rect3 = new Rect(0.0f, 0.0f, ((Rect) ref rect2).width, ((Rect) ref rect2).height);
      Rect rect4;
      // ISSUE: explicit constructor call
      ((Rect) ref rect4).\u002Ector(0.0f, 0.0f, ((Rect) ref rect2).width - 16f, this.scrollViewHeight);
      ref Vector2 local = ref this.scrollPosition;
      Rect rect5 = rect4;
      Widgets.BeginScrollView(rect3, ref local, rect5, true);
      float num = 0.0f;
      Widgets.ListSeparator(ref num, ((Rect) ref rect4).width, TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.Gene")));
      if (Widgets.ButtonText(new Rect(0.0f, num, 140f, 28f), TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.AddGene")), true, true, true, new TextAnchor?()))
        this.AddGenes();
      float y = num + 32f;
      List<GeneData> genes = this.Statue.Data.genes;
      if (genes != null)
      {
        for (int index = 0; index < genes.Count; ++index)
        {
          GeneData gene = genes[index];
          this.DrawGeneRow(ref y, ((Rect) ref rect4).width, gene, index);
        }
      }
      if (Event.current.type == 8)
        this.scrollViewHeight = y + 30f;
      Widgets.EndScrollView();
      GUI.EndGroup();
      GUI.color = Color.white;
      Text.Anchor = (TextAnchor) 0;
    }

    private void DrawGeneRow(
      ref float y,
      float width,
      GeneData gene,
      int index,
      bool showDropButtonIfPrisoner = false)
    {
      Rect rect1;
      // ISSUE: explicit constructor call
      ((Rect) ref rect1).\u002Ector(0.0f, y, width, 28f);
      Rect rect2 = new Rect(((Rect) ref rect1).width - 24f, y, 24f, 24f);
      TooltipHandler.TipRegion(rect2, TipSignal.op_Implicit(Translator.Translate("StatueOfColonist.RemoveGene")));
      if (Widgets.ButtonImage(rect2, ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true), true))
      {
        SoundStarter.PlayOneShotOnCamera(SoundDefOf.Tick_High, (Map) null);
        this.Statue.RemoveGene(gene);
      }
      ref Rect local = ref rect1;
      ((Rect) ref local).width = ((Rect) ref local).width - 24f;
      if (Mouse.IsOver(rect1))
      {
        GUI.color = Window_EditStatueGene.HighlightColor;
        GUI.DrawTexture(rect1, (Texture) TexUI.HighlightTex);
      }
      if (Object.op_Inequality((Object) gene.geneDef.Icon, (Object) null))
        GUI.DrawTexture(new Rect(4f, y, 28f, 28f), (Texture) gene.geneDef.Icon);
      Text.Anchor = (TextAnchor) 3;
      GUI.color = Window_EditStatueGene.ThingLabelColor;
      Rect rect3 = new Rect(36f, y, ((Rect) ref rect1).width - 36f, ((Rect) ref rect1).height);
      Text.WordWrap = false;
      TaggedString label = gene.Label;
      Widgets.Label(rect3, label);
      Text.WordWrap = true;
      TooltipHandler.TipRegion(rect1, TipSignal.op_Implicit(gene.Label));
      y += 28f;
    }

    private void AddGenes()
    {
      List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
      foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefs.Where<GeneDef>((Func<GeneDef, bool>) (def => def.HasGraphic || def.graphicData?.fur != null)))
      {
        GeneGraphicData graphicData = geneDef.graphicData;
        if (!GenList.NullOrEmpty<string>((IList<string>) graphicData.graphicPaths))
        {
          for (int index = 0; index < graphicData.graphicPaths.Count; ++index)
          {
            string graphicPath = graphicData.graphicPaths[index];
            GeneData geneData = new GeneData(geneDef, index, (Gender) 1);
            Log.Message(TaggedString.op_Implicit(geneData.Label));
            floatMenuOptionList.Add(new FloatMenuOption(TaggedString.op_Implicit(geneData.Label), (Action) (() => this.Statue.AddGene(geneData)), geneDef.Icon, Color.white, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0, (HorizontalJustification) 0, false));
          }
        }
        if (!GenText.NullOrEmpty(graphicData.graphicPathFemale))
        {
          GeneData geneData = new GeneData(geneDef, -1, (Gender) 2);
          Log.Message(TaggedString.op_Implicit(geneData.Label));
          floatMenuOptionList.Add(new FloatMenuOption(TaggedString.op_Implicit(geneData.Label), (Action) (() => this.Statue.AddGene(geneData)), geneDef.Icon, Color.white, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0, (HorizontalJustification) 0, false));
        }
        if (!GenText.NullOrEmpty(graphicData.graphicPath))
        {
          GeneData geneData = new GeneData(geneDef, -1, (Gender) 1);
          Log.Message(TaggedString.op_Implicit(geneData.Label));
          floatMenuOptionList.Add(new FloatMenuOption(TaggedString.op_Implicit(geneData.Label), (Action) (() => this.Statue.AddGene(geneData)), geneDef.Icon, Color.white, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0, (HorizontalJustification) 0, false));
        }
      }
      Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
    }
  }
}