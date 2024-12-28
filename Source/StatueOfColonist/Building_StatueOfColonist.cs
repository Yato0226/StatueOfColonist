// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Building_StatueOfColonist
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;

#nullable disable
namespace StatueOfColonist
{
  public class Building_StatueOfColonist : Building_Art
  {
    public string name;
    private StatueOfColonistRenderer renderer;
    private StatueOfColonistData dataStatue;
    private StatueOfColonistGraphicSet graphicsStatue;
    private StatueOfColonistGraphicSet graphicsBlueprint;
    private StatueOfColonistGraphicSet graphicsCanDesignateGhost;
    private StatueOfColonistGraphicSet graphicsCanNotDesignateGhost;
    private Building_StatueOfColonist.HeadRenderMode headRenderMode;
    [TweakValue("Graphics", -5f, 5f)]
    private static float StatueYOffset = 0.0385f;
    private static readonly Color ColorBlueprint = new Color(0.5f, 0.5f, 1f, 0.35f);
    private static readonly Color ColorCanDesignateGhost = new Color(0.5f, 1f, 0.6f, 0.4f);
    private static readonly Color ColorCanNotDesignateGhost = new Color(1f, 0.0f, 0.0f, 0.4f);

    public virtual string LabelNoCount
    {
      get
      {
        string str1 = GenLabel.ThingLabel((BuildableDef) ((Thing) this).def, ((Thing) this).Stuff, 1);
        string str2 = "";
        QualityCategory qualityCategory;
        if (QualityUtility.TryGetQuality((Thing) this, ref qualityCategory))
          str2 = QualityUtility.GetLabel(qualityCategory);
        return !GenText.NullOrEmpty(this.name) ? TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.StatueNameFormat", NamedArgument.op_Implicit(str1), NamedArgument.op_Implicit(this.name), NamedArgument.op_Implicit(str2))) : TaggedString.op_Implicit(TranslatorFormattedStringExtensions.Translate("StatueOfColonist.StatueNameFormatIfNoStatueName", NamedArgument.op_Implicit(str1), NamedArgument.op_Implicit(this.name), NamedArgument.op_Implicit(str2)));
      }
    }

    public bool ShouldBeNamed
    {
      get => GenText.NullOrEmpty(this.name) && StatueOfColonistMod.Settings.autoName;
    }

    public bool IsValid => this.dataStatue != null;

    public StatueOfColonistDef Def => ((Thing) this).def as StatueOfColonistDef;

    public StatueOfColonistData Data
    {
      get
      {
        StatueOfColonistData dataStatue = this.dataStatue;
        return this.dataStatue;
      }
      set => this.dataStatue = value;
    }

    public int Size
    {
      get
      {
        int size = 0;
        if (((Verse.Def) ((Thing) this).def).defName == "TMB_StatueOfColonistKLarge")
          size = 1;
        else if (((Verse.Def) ((Thing) this).def).defName == "TMB_StatueOfColonistKGrand")
          size = 2;
        return size;
      }
    }

    public virtual void SpawnSetup(Map map, bool respawningAfterLoad)
    {
      ((Building) this).SpawnSetup(map, respawningAfterLoad);
      if (!map.dynamicDrawManager.DrawThingsForReading.Contains((Thing) this))
        map.dynamicDrawManager.RegisterDrawable((Thing) this);
      if (this.IsValid)
        return;
      this.InitializeStatue();
    }

    public virtual void DeSpawn(DestroyMode mode = 0)
    {
      if (((Thing) this).Map.dynamicDrawManager.DrawThingsForReading.Contains((Thing) this))
        ((Thing) this).Map.dynamicDrawManager.DeRegisterDrawable((Thing) this);
      ((Building) this).DeSpawn(mode);
    }

    public void InitializeStatue()
    {
      bool flag = false;
      if ((double) Random.value < (double) StatueOfColonistMod.Settings.possibilityOfStatueFromPreset / 100.0)
      {
        List<StatueOfColonistPreset> all = StatueOfColonistPref.presets.FindAll((Predicate<StatueOfColonistPreset>) (p => p.IsValid));
        if (!GenList.NullOrEmpty<StatueOfColonistPreset>((IList<StatueOfColonistPreset>) all))
        {
          StatueOfColonistPreset preset = GenCollection.RandomElementByWeight<StatueOfColonistPreset>((IEnumerable<StatueOfColonistPreset>) all, (Func<StatueOfColonistPreset, float>) (p => p.weight));
          this.CopyStatueOfColonistFromPreset(preset);
          this.name = !this.ShouldBeNamed ? "" : preset.name;
          flag = true;
        }
      }
      if (flag)
        return;
      Map map = ((Thing) this).Map;
      Pawn pawn1;
      if (map == null)
      {
        pawn1 = (Pawn) null;
      }
      else
      {
        MapPawns mapPawns = map.mapPawns;
        if (mapPawns == null)
        {
          pawn1 = (Pawn) null;
        }
        else
        {
          List<Pawn> freeColonists = mapPawns.FreeColonists;
          pawn1 = freeColonists != null ? GenCollection.RandomElement<Pawn>((IEnumerable<Pawn>) freeColonists) : (Pawn) null;
        }
      }
      Pawn pawn2 = pawn1;
      if (pawn2 == null)
        return;
      this.ResolveGraphics(pawn2);
      this.ResolveArt(pawn2);
      if (this.ShouldBeNamed)
        this.name = ((Entity) pawn2).LabelShort;
      else
        this.name = "";
    }

    public void CopyStatueOfColonistFromPreset(StatueOfColonistPreset preset)
    {
      if (this.dataStatue == null)
        this.dataStatue = new StatueOfColonistData(((Thing) this).DrawColor, "Map/Cutout");
      this.dataStatue.CopyFromPreset(preset);
      this.ResolveGraphics();
    }

    public virtual void ExposeData()
    {
      ((Building) this).ExposeData();
      if (Scribe.mode == 1 && this.name == null)
        this.name = "";
      Scribe_Values.Look<string>(ref this.name, "name", "", false);
      Scribe_Deep.Look<StatueOfColonistData>(ref this.dataStatue, "dataStatue", Array.Empty<object>());
      Scribe_Values.Look<Building_StatueOfColonist.HeadRenderMode>(ref this.headRenderMode, "headRenderMode", Building_StatueOfColonist.HeadRenderMode.Default, false);
      if (Scribe.mode != 4)
        return;
      this.ResolveGraphics();
    }

    public virtual void Draw()
    {
      this.Render(Vector3.op_Addition(((Thing) this).DrawPos, new Vector3(this.Def.offsetX, Building_StatueOfColonist.StatueYOffset, this.Def.offsetZ)), Quaternion.identity, true, ((Thing) this).Rotation, ((Thing) this).Rotation, (RotDrawMode) 0, false, false, this.Def.scale);
      ((ThingWithComps) this).Comps_PostDraw();
    }

    public void ResolveGraphics(Pawn pawn)
    {
      this.dataStatue = new StatueOfColonistData(pawn, ((Thing) this).DrawColor, "Map/Cutout");
      this.PreResolveGraphicsFromPawn(pawn);
      this.ResolveGraphics();
    }

    public void ResolveGraphics()
    {
      this.renderer = new StatueOfColonistRenderer(this);
      this.graphicsStatue = new StatueOfColonistGraphicSet(this.dataStatue, this);
      this.graphicsBlueprint = new StatueOfColonistGraphicSet(new StatueOfColonistData(this.dataStatue, Building_StatueOfColonist.ColorBlueprint, "Map/CutoutSkin"), this);
      this.graphicsCanDesignateGhost = new StatueOfColonistGraphicSet(new StatueOfColonistData(this.dataStatue, Building_StatueOfColonist.ColorCanDesignateGhost, "Map/CutoutSkin"), this);
      this.graphicsCanNotDesignateGhost = new StatueOfColonistGraphicSet(new StatueOfColonistData(this.dataStatue, Building_StatueOfColonist.ColorCanNotDesignateGhost, "Map/CutoutSkin"), this);
    }

    public virtual void PreResolveGraphicsFromPawn(Pawn pawn)
    {
    }

    public void ForceResolveWhenRendering()
    {
      this.graphicsStatue.forceResolve = true;
      this.graphicsBlueprint.forceResolve = true;
      this.graphicsCanDesignateGhost.forceResolve = true;
      this.graphicsCanNotDesignateGhost.forceResolve = true;
    }

    public void ResolveBoardGraphic()
    {
      this.graphicsStatue.ResolveBoardGraphic();
      this.graphicsBlueprint.ResolveBoardGraphic();
      this.graphicsCanDesignateGhost.ResolveBoardGraphic();
      this.graphicsCanNotDesignateGhost.ResolveBoardGraphic();
    }

    public void Render(
      Vector3 rootLoc,
      Quaternion quat,
      bool renderBody,
      Rot4 bodyFacing,
      Rot4 headFacing,
      RotDrawMode bodyDrawType,
      bool portrait,
      bool headStump,
      float scale,
      Building_StatueOfColonist.RenderMode mode = Building_StatueOfColonist.RenderMode.Normal)
    {
      this.renderer.Render(this.GetStatueGraphics(mode), Vector3.op_Addition(Vector3.op_Addition(rootLoc, new Vector3(0.0f, 0.0f, StatueOfColonistMod.Settings.GetOffsetStatueBody(this.Data.bodyType, this.Size))), new Vector3(this.Data.offset.x, 0.0f, this.Data.offset.y)), quat, renderBody, bodyFacing, headFacing, bodyDrawType, portrait, headStump, scale, this.headRenderMode, this.Data.raceDef, this.Data.bodyType, this.Data.lifeStageDef);
    }

    [DebuggerHidden]
    public virtual IEnumerable<Gizmo> GetGizmos()
    {
      // ISSUE: reference to a compiler-generated method
      foreach (Gizmo gizmo in this.\u003C\u003En__0())
        yield return gizmo;
      foreach (Gizmo statueGizmo in this.GetStatueGizmos())
        yield return statueGizmo;
    }

    public IEnumerable<Gizmo> GetStatueGizmos()
    {
      Command_Action commandAction1 = new Command_Action();
      ((Command) commandAction1).defaultDesc = this.GetGizmoDesc();
      ((Command) commandAction1).icon = (Texture) this.GetGizmoIcon();
      ((Command) commandAction1).defaultLabel = this.GetGizmoLabel();
      Command_Action gizmoHeadRenderMode = commandAction1;
      gizmoHeadRenderMode.action = (Action) (() =>
      {
        this.headRenderMode = (Building_StatueOfColonist.HeadRenderMode) Enum.ToObject(typeof (Building_StatueOfColonist.HeadRenderMode), (int) (this.headRenderMode + 1) % Enum.GetNames(typeof (Building_StatueOfColonist.HeadRenderMode)).Length);
        ((Command) gizmoHeadRenderMode).defaultDesc = this.GetGizmoDesc();
        ((Command) gizmoHeadRenderMode).icon = (Texture) this.GetGizmoIcon();
        ((Command) gizmoHeadRenderMode).defaultLabel = this.GetGizmoLabel();
      });
      yield return (Gizmo) gizmoHeadRenderMode;
      Command_Action commandAction2 = new Command_Action();
      ((Command) commandAction2).defaultDesc = TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandRenameDesc"));
      ((Command) commandAction2).icon = (Texture) ContentFinder<Texture2D>.Get("UI/Buttons/Rename", true);
      ((Command) commandAction2).defaultLabel = TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandRenameLabel"));
      Command_Action statueGizmo1 = commandAction2;
      statueGizmo1.action = (Action) (() => Find.WindowStack.Add((Window) new Dialog_NameStatue(this)));
      yield return (Gizmo) statueGizmo1;
      Command_Action commandAction3 = new Command_Action();
      ((Command) commandAction3).defaultDesc = TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandEditStatueDesc"));
      ((Command) commandAction3).icon = (Texture) ContentFinder<Texture2D>.Get("UI/Commands/LaunchReport", true);
      ((Command) commandAction3).defaultLabel = TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandEditStatueLabel"));
      Command_Action statueGizmo2 = commandAction3;
      statueGizmo2.action = (Action) (() => Find.WindowStack.Add((Window) new Window_EditStatue(this)));
      yield return (Gizmo) statueGizmo2;
    }

    private string GetGizmoLabel()
    {
      if (this.headRenderMode == Building_StatueOfColonist.HeadRenderMode.Default)
        return TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandDefaultLabel"));
      return this.headRenderMode == Building_StatueOfColonist.HeadRenderMode.HairOnly ? TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandHairOnlyLabel")) : TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandHatAndHairLabel"));
    }

    private string GetGizmoDesc()
    {
      if (this.headRenderMode == Building_StatueOfColonist.HeadRenderMode.Default)
        return TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandDefaultDesc"));
      return this.headRenderMode == Building_StatueOfColonist.HeadRenderMode.HairOnly ? TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandHairOnlyDesc")) : TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandHatAndHairDesc"));
    }

    private Texture2D GetGizmoIcon()
    {
      if (this.headRenderMode == Building_StatueOfColonist.HeadRenderMode.Default)
        return ContentFinder<Texture2D>.Get("UI/Commands/CommandToggleHeadRenderModeDefault", true);
      return this.headRenderMode == Building_StatueOfColonist.HeadRenderMode.HairOnly ? ContentFinder<Texture2D>.Get("UI/Commands/CommandToggleHeadRenderModeHairOnly", true) : ContentFinder<Texture2D>.Get("UI/Commands/CommandToggleHeadRenderModeHatAndHair", true);
    }

    public void AddApparel(ThingDef apparel)
    {
      this.Data.wornApparelDefs.Add(apparel);
      this.ResolveGraphics();
    }

    public void AddGene(GeneData geneData)
    {
      this.Data.genes.Add(geneData);
      this.ResolveGraphics();
    }

    public void RemoveApparel(ThingDef apparel)
    {
      this.Data.wornApparelDefs.Remove(apparel);
      this.ResolveGraphics();
    }

    public void RemoveGene(GeneData geneData)
    {
      this.Data.genes.Remove(geneData);
      this.ResolveGraphics();
    }

    public bool ResolveArt(Pawn p)
    {
      bool flag = false;
      CompArt comp1 = ((ThingWithComps) this).GetComp<CompArt>();
      if (comp1 != null)
      {
        for (int index = 0; index < 100; ++index)
        {
          comp1.InitializeArt((Thing) p);
          if (comp1 != null && comp1.GenerateImageDescription().ToString().Contains(((Entity) p).LabelShort))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          foreach (Thing thing in Find.CurrentMap.listerThings.ThingsOfDef(ThingDef.Named("Sarcophagus")))
          {
            if (thing is Building_Sarcophagus buildingSarcophagus && ((Building_CorpseCasket) buildingSarcophagus).Corpse?.InnerPawn == p)
            {
              CompArt comp2 = ((ThingWithComps) buildingSarcophagus).GetComp<CompArt>();
              if (comp2 != null)
              {
                comp2.GenerateImageDescription();
                if (comp2.GenerateImageDescription().ToString().Contains(((Entity) p).LabelShort))
                {
                  this.InitializeArt(comp1, comp2.TaleRef);
                  flag = true;
                  break;
                }
              }
            }
          }
        }
      }
      return flag;
    }

    private void InitializeArt(CompArt comp, TaleReference taleRef)
    {
      Traverse traverse1 = Traverse.Create((object) comp).Field(nameof (taleRef));
      Traverse traverse2 = Traverse.Create((object) comp).Field("titleInt");
      Traverse traverse3 = Traverse.Create((object) comp).Method("GenerateTitle", Array.Empty<object>());
      if (comp.TaleRef != null)
      {
        comp.TaleRef.ReferenceDestroyed();
        traverse1.SetValue((object) null);
      }
      else
      {
        Traverse traverse4 = Traverse.Create((object) taleRef).Field("tale");
        traverse1.SetValue((object) new TaleReference(traverse4.GetValue<Tale>()));
        traverse2.SetValue(traverse3.GetValue());
      }
    }

    public StatueOfColonistGraphicSet GetStatueGraphics(Building_StatueOfColonist.RenderMode mode)
    {
      StatueOfColonistGraphicSet statueGraphics = this.graphicsStatue;
      switch (mode)
      {
        case Building_StatueOfColonist.RenderMode.Blueprint:
          statueGraphics = this.graphicsBlueprint;
          break;
        case Building_StatueOfColonist.RenderMode.CanDesignateGhost:
          statueGraphics = this.graphicsCanDesignateGhost;
          break;
        case Building_StatueOfColonist.RenderMode.CanNotDesignateGhost:
          statueGraphics = this.graphicsCanNotDesignateGhost;
          break;
      }
      return statueGraphics;
    }

    public enum RenderMode
    {
      Normal,
      Blueprint,
      CanDesignateGhost,
      CanNotDesignateGhost,
    }

    public enum HeadRenderMode
    {
      Default,
      HairOnly,
      HatAndHair,
    }
  }
}
