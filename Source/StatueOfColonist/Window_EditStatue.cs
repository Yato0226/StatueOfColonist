// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Window_EditStatue
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
  public class Window_EditStatue : Window
  {
    private Vector2 scrollPosition = Vector2.zero;
    private float scrollViewHeight;
    private static readonly Color ThingLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);
    private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Building_StatueOfColonist statue;
    private string raceName = "";
    private string lifeStageName = "";
    private string hairName = "";
    private string headName = "";
    public List<ThingDef> raceDefs;

    public Building_StatueOfColonist Statue => this.statue;

    public virtual Vector2 InitialSize
    {
      get
      {
        float num = 656f;
        if (ModsConfig.IdeologyActive)
          num += 56f;
        if (ModsConfig.BiotechActive)
          num += 28f;
        return new Vector2(460f, num);
      }
    }

    public Window_EditStatue(Building_StatueOfColonist statue)
    {
      this.optionalTitle = TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.WindowEditStatue"));
      this.doCloseButton = true;
      this.doCloseX = true;
      this.forcePause = true;
      this.absorbInputAroundWindow = true;
      this.statue = statue;
      this.InitRaceDefs();
      this.UpdateButtonLabel();
    }

    public void InitRaceDefs()
    {
      this.raceDefs = new List<ThingDef>();
      this.raceDefs.Add(ThingDefOf.Human);
    }

    public void UpdateButtonLabel()
    {
      this.raceName = TaggedString.op_Implicit(((Def) this.Statue.Data.raceDef).LabelCap);
      this.lifeStageName = TaggedString.op_Implicit(((Def) this.Statue.Data.lifeStageDef).LabelCap);
      HairDef defFromGraphicPath = Util.GetHairDefFromGraphicPath(this.statue.Data.hairGraphicPath);
      if (defFromGraphicPath != null)
        this.hairName = TaggedString.op_Implicit(((Def) defFromGraphicPath).LabelCap);
      this.headName = ((IEnumerable<string>) this.statue.Data.headGraphicPath.Split('/')).LastOrDefault<string>();
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
      float num1 = 0.0f;
      if (Widgets.ButtonText(new Rect(0.0f, num1, 140f, 28f), TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CopyFromColonist")), true, true, true, new TextAnchor?()))
        this.CopyFromColonist();
      if (Widgets.ButtonText(new Rect(145f, num1, 140f, 28f), TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.OpenPresetListDialog")), true, true, true, new TextAnchor?()))
        this.OpenPresetListDialog();
      float num2 = num1 + 40f;
      Widgets.DrawLineHorizontal(0.0f, num2, ((Rect) ref rect4).width);
      float num3 = num2 + 8f;
      if (this.raceDefs.Count >= 2)
      {
        Widgets.Label(new Rect(0.0f, num3 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.RaceOfStatue"));
        if (Widgets.ButtonText(new Rect(150f, num3, 200f, 28f), this.raceName, true, true, true, new TextAnchor?()))
        {
          List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
          foreach (ThingDef raceDef in this.raceDefs)
          {
            ThingDef localRaceDef = raceDef;
            Action action = (Action) (() =>
            {
              this.raceName = TaggedString.op_Implicit(((Def) localRaceDef).LabelCap);
              this.Statue.Data.raceDef = localRaceDef;
              this.UpdateRace();
            });
            FloatMenuOption floatMenuOption = new FloatMenuOption(TaggedString.op_Implicit(((Def) localRaceDef).LabelCap), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
            floatMenuOptionList.Add(floatMenuOption);
          }
          if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
            Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
        }
        float num4 = num3 + 28f;
        Widgets.Label(new Rect(0.0f, num4 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.GenderOfStatue"));
        if (Widgets.ButtonText(new Rect(150f, num4, 200f, 28f), GenderUtility.GetLabel(this.Statue.Data.gender, false), true, true, true, new TextAnchor?()))
        {
          List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
          for (int index = 1; index < 3; ++index)
          {
            Gender gender = (Gender) Enum.ToObject(typeof (Gender), index);
            if (this.IsAvailableGender(gender, this.Statue.Data.raceDef))
            {
              Action action = (Action) (() =>
              {
                this.Statue.Data.gender = gender;
                this.Statue.ResolveGraphics();
              });
              FloatMenuOption floatMenuOption = new FloatMenuOption(GenderUtility.GetLabel(gender, false), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
              floatMenuOptionList.Add(floatMenuOption);
            }
          }
          if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
            Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
        }
        num3 = num4 + 28f;
      }
      Widgets.Label(new Rect(0.0f, num3 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.LifeStageOfStatue"));
      if (Widgets.ButtonText(new Rect(150f, num3, 200f, 28f), this.lifeStageName, true, true, true, new TextAnchor?()))
      {
        List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
        foreach (LifeStageDef lifeStageDef in this.Statue.Data.raceDef.race.lifeStageAges.ConvertAll<LifeStageDef>((Converter<LifeStageAge, LifeStageDef>) (ls => ls.def)))
        {
          LifeStageDef localLifeStageDef = lifeStageDef;
          Action action = (Action) (() =>
          {
            this.lifeStageName = TaggedString.op_Implicit(((Def) localLifeStageDef).LabelCap);
            this.Statue.Data.lifeStageDef = localLifeStageDef;
            this.Statue.ResolveGraphics();
          });
          FloatMenuOption floatMenuOption = new FloatMenuOption(TaggedString.op_Implicit(((Def) localLifeStageDef).LabelCap), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
          floatMenuOptionList.Add(floatMenuOption);
        }
        if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
          Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
      }
      float num5 = num3 + 28f;
      Widgets.Label(new Rect(0.0f, num5 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.HairTypeOfStatue"));
      if (Widgets.ButtonText(new Rect(150f, num5, 200f, 28f), this.hairName, true, true, true, new TextAnchor?()))
      {
        List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
        foreach (HairDef hairDef in DefDatabase<HairDef>.AllDefsListForReading)
        {
          if (this.IsAvailableHair(hairDef, this.Statue.Data.raceDef))
          {
            HairDef localHairDef = hairDef;
            Action action = (Action) (() =>
            {
              this.hairName = TaggedString.op_Implicit(((Def) localHairDef).LabelCap);
              this.Statue.Data.hairGraphicPath = ((StyleItemDef) localHairDef).texPath;
              this.Statue.ResolveGraphics();
            });
            FloatMenuOption floatMenuOption = new FloatMenuOption(TaggedString.op_Implicit(((Def) localHairDef).LabelCap), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
            floatMenuOptionList.Add(floatMenuOption);
          }
        }
        if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
          Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
      }
      float num6 = num5 + 28f;
      Widgets.Label(new Rect(0.0f, num6 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.HeadTypeOfStatue"));
      if (Widgets.ButtonText(new Rect(150f, num6, 200f, 28f), this.headName, true, true, true, new TextAnchor?()))
      {
        List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
        foreach (HeadTypeDef headType in this.GetHeadTypes(this.Statue.Data.raceDef))
          floatMenuOptionList.Add(this.GetHeadTypeItem(headType, this.Statue.Data.raceDef));
        if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
          Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
      }
      float num7 = num6 + 28f;
      Widgets.Label(new Rect(0.0f, num7 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.BodyTypeOfStatue"));
      if (Widgets.ButtonText(new Rect(150f, num7, 200f, 28f), this.Statue.Data.bodyType.GetName(), true, true, true, new TextAnchor?()))
      {
        List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
        foreach (BodyTypeDef bodyType1 in this.GetBodyTypes(this.Statue.Data.raceDef))
        {
          BodyTypeDef bodyType = bodyType1;
          Action action = (Action) (() =>
          {
            this.Statue.Data.bodyType = bodyType;
            this.Statue.ResolveGraphics();
          });
          FloatMenuOption floatMenuOption = new FloatMenuOption(bodyType.GetName(), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
          floatMenuOptionList.Add(floatMenuOption);
        }
        if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
          Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
      }
      float num8 = num7 + 28f;
      Widgets.Label(new Rect(0.0f, num8 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.BeardOfStatue"));
      if (Widgets.ButtonText(new Rect(150f, num8, 200f, 28f), TaggedString.op_Implicit(((Def) this.Statue.Data.beardDef).LabelCap), true, true, true, new TextAnchor?()))
      {
        List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
        foreach (BeardDef beardDef1 in DefDatabase<BeardDef>.AllDefsListForReading)
        {
          BeardDef beardDef = beardDef1;
          Action action = (Action) (() =>
          {
            this.Statue.Data.beardDef = beardDef;
            this.Statue.ResolveGraphics();
          });
          FloatMenuOption floatMenuOption = new FloatMenuOption(TaggedString.op_Implicit(((Def) beardDef).LabelCap), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
          floatMenuOptionList.Add(floatMenuOption);
        }
        if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
          Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
      }
      float num9 = num8 + 28f;
      Widgets.Label(new Rect(0.0f, num9 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.BeardBlightnessOfStatue"));
      Rect rect6;
      // ISSUE: explicit constructor call
      ((Rect) ref rect6).\u002Ector(150f, num9, 200f, 28f);
      double beardBlightness1 = (double) this.Statue.Data.beardBlightness;
      this.Statue.Data.beardBlightness = Widgets.HorizontalSlider(rect6, this.Statue.Data.beardBlightness, 0.0f, 1f, false, this.Statue.Data.beardBlightness.ToString(), (string) null, (string) null, 0.01f);
      double beardBlightness2 = (double) this.Statue.Data.beardBlightness;
      if (beardBlightness1 != beardBlightness2)
        this.Statue.ResolveBoardGraphic();
      float y = num9 + 32f;
      if (ModsConfig.IdeologyActive)
      {
        Widgets.Label(new Rect(0.0f, y + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.FaceTatooOfStatue"));
        if (Widgets.ButtonText(new Rect(150f, y, 200f, 28f), TaggedString.op_Implicit(((Def) this.Statue.Data.faceTattooDef).LabelCap), true, true, true, new TextAnchor?()))
        {
          List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
          foreach (TattooDef tattooDef1 in DefDatabase<TattooDef>.AllDefsListForReading.Where<TattooDef>((Func<TattooDef, bool>) (def => def.tattooType == 0)))
          {
            TattooDef tattooDef = tattooDef1;
            Action action = (Action) (() =>
            {
              this.Statue.Data.faceTattooDef = tattooDef;
              this.Statue.ResolveGraphics();
              this.Statue.ForceResolveWhenRendering();
            });
            FloatMenuOption floatMenuOption = new FloatMenuOption(TaggedString.op_Implicit(((Def) tattooDef).LabelCap), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
            floatMenuOptionList.Add(floatMenuOption);
          }
          if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
            Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
        }
        float num10 = y + 28f;
        Widgets.Label(new Rect(0.0f, num10 + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.BodyTatooOfStatue"));
        if (Widgets.ButtonText(new Rect(150f, num10, 200f, 28f), TaggedString.op_Implicit(((Def) this.Statue.Data.bodyTattooDef).LabelCap), true, true, true, new TextAnchor?()))
        {
          List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
          foreach (TattooDef tattooDef2 in DefDatabase<TattooDef>.AllDefsListForReading.Where<TattooDef>((Func<TattooDef, bool>) (def => def.tattooType == 1)))
          {
            TattooDef tattooDef = tattooDef2;
            Action action = (Action) (() =>
            {
              this.Statue.Data.bodyTattooDef = tattooDef;
              this.Statue.ResolveGraphics();
              this.Statue.ForceResolveWhenRendering();
            });
            FloatMenuOption floatMenuOption = new FloatMenuOption(TaggedString.op_Implicit(((Def) tattooDef).LabelCap), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
            floatMenuOptionList.Add(floatMenuOption);
          }
          if (!GenList.NullOrEmpty<FloatMenuOption>((IList<FloatMenuOption>) floatMenuOptionList))
            Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
        }
        y = num10 + 28f;
      }
      if (this.Statue.Data.raceDef != ThingDefOf.Human)
        this.EditAddons(ref y);
      if (ModsConfig.BiotechActive)
      {
        Widgets.Label(new Rect(0.0f, y + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.GenesOfStatue"));
        if (Widgets.ButtonText(new Rect(150f, y, 200f, 28f), TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.OpenGeneEditWindow")), true, true, true, new TextAnchor?()))
          Find.WindowStack.Add((Window) new Window_EditStatueGene(this.Statue));
        y += 28f;
      }
      Widgets.Label(new Rect(0.0f, y + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.OffsetXOfStatue"));
      if (Widgets.ButtonText(new Rect(355f, y, 40f, 28f), "0", true, true, true, new TextAnchor?()))
        this.Statue.Data.offset.x = 0.0f;
      Rect rect7;
      // ISSUE: explicit constructor call
      ((Rect) ref rect7).\u002Ector(150f, y, 200f, 28f);
      this.Statue.Data.offset.x = Widgets.HorizontalSlider(rect7, this.Statue.Data.offset.x, -2f, 2f, false, this.Statue.Data.offset.x.ToString(), (string) null, (string) null, 0.025f);
      y += 32f;
      Widgets.Label(new Rect(0.0f, y + 2f, 150f, 28f), Translator.Translate("StatueOfColonist.OffsetZOfStatue"));
      if (Widgets.ButtonText(new Rect(355f, y, 40f, 28f), "0", true, true, true, new TextAnchor?()))
        this.Statue.Data.offset.y = 0.0f;
      Rect rect8;
      // ISSUE: explicit constructor call
      ((Rect) ref rect8).\u002Ector(150f, y, 200f, 28f);
      this.Statue.Data.offset.y = Widgets.HorizontalSlider(rect8, this.Statue.Data.offset.y, -2f, 2f, false, this.Statue.Data.offset.y.ToString(), (string) null, (string) null, 0.025f);
      y += 32f;
      Widgets.ListSeparator(ref y, ((Rect) ref rect4).width, TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.Apparel")));
      if (Widgets.ButtonText(new Rect(0.0f, y, 140f, 28f), TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.AddApparel")), true, true, true, new TextAnchor?()))
        this.AddAppearenceClothes();
      y += 32f;
      List<ThingDef> wornApparelDefs = this.Statue.Data.wornApparelDefs;
      if (wornApparelDefs != null)
      {
        for (int index = 0; index < wornApparelDefs.Count; ++index)
        {
          ThingDef thingdef = wornApparelDefs[index];
          this.DrawThingRow(ref y, ((Rect) ref rect4).width, thingdef, index);
        }
      }
      if (Event.current.type == 8)
        this.scrollViewHeight = y + 30f;
      Widgets.EndScrollView();
      GUI.EndGroup();
      GUI.color = Color.white;
      Text.Anchor = (TextAnchor) 0;
    }

    public virtual bool IsAvailableGender(Gender gender, ThingDef raceDef) => true;

    public virtual bool IsAvailableHair(HairDef hairDef, ThingDef raceDef) => true;

    public void UpdateRace() => this.Statue.ResolveGraphics();

    public List<HeadTypeDef> GetHeadTypes(ThingDef raceDef)
    {
      return DefDatabase<HeadTypeDef>.AllDefsListForReading;
    }

    public FloatMenuOption GetHeadTypeItem(HeadTypeDef src, ThingDef raceDef)
    {
      if (raceDef != ThingDefOf.Human)
        return (FloatMenuOption) null;
      HeadTypeDef headType = src;
      TaggedString labelCap = ((Def) headType).LabelCap;
      TaggedString headTypeLabel = ((TaggedString) ref labelCap).NullOrEmpty() ? new TaggedString(((Def) headType).defName) : ((Def) headType).LabelCap;
      Action action = (Action) (() =>
      {
        this.Statue.Data.headGraphicPath = headType.graphicPath;
        this.Statue.Data.crownType = headType;
        this.headName = TaggedString.op_Implicit(headTypeLabel);
        this.Statue.ResolveGraphics();
      });
      return new FloatMenuOption(TaggedString.op_Implicit(headTypeLabel), action, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
    }

    public List<BodyTypeDef> GetBodyTypes(ThingDef raceDef)
    {
      List<BodyTypeDef> bodyTypes = new List<BodyTypeDef>();
      for (int index = 0; index < 5; ++index)
      {
        BodyTypeDef bodyTypeDef = index.ToBodyTypeDef();
        bodyTypes.Add(bodyTypeDef);
      }
      return bodyTypes;
    }

    public virtual void EditAddons(ref float y)
    {
    }

    private void DrawThingRow(
      ref float y,
      float width,
      ThingDef thingdef,
      int index,
      bool showDropButtonIfPrisoner = false)
    {
      Rect rect1;
      // ISSUE: explicit constructor call
      ((Rect) ref rect1).\u002Ector(0.0f, y, width, 28f);
      Rect rect2 = new Rect(((Rect) ref rect1).width - 24f, y, 24f, 24f);
      TooltipHandler.TipRegion(rect2, TipSignal.op_Implicit(Translator.Translate("StatueOfColonist.RemoveApparel")));
      if (Widgets.ButtonImage(rect2, ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true), true))
      {
        SoundStarter.PlayOneShotOnCamera(SoundDefOf.Tick_High, (Map) null);
        this.Statue.RemoveApparel(thingdef);
      }
      ref Rect local = ref rect1;
      ((Rect) ref local).width = ((Rect) ref local).width - 24f;
      if (Mouse.IsOver(rect1))
      {
        GUI.color = Window_EditStatue.HighlightColor;
        GUI.DrawTexture(rect1, (Texture) TexUI.HighlightTex);
      }
      if (Object.op_Inequality((Object) ((BuildableDef) thingdef).DrawMatSingle, (Object) null) && Object.op_Inequality((Object) ((BuildableDef) thingdef).DrawMatSingle.mainTexture, (Object) null))
        Widgets.ThingIcon(new Rect(4f, y, 28f, 28f), thingdef, (ThingDef) null, (ThingStyleDef) null, 1f, new Color?(), new int?());
      Text.Anchor = (TextAnchor) 3;
      GUI.color = Window_EditStatue.ThingLabelColor;
      Rect rect3 = new Rect(36f, y, ((Rect) ref rect1).width - 36f, ((Rect) ref rect1).height);
      Text.WordWrap = false;
      TaggedString labelCap = ((Def) thingdef).LabelCap;
      Widgets.Label(rect3, labelCap);
      Text.WordWrap = true;
      TooltipHandler.TipRegion(rect1, TipSignal.op_Implicit(((Def) thingdef).LabelCap));
      y += 28f;
    }

    private void AddAppearenceClothes()
    {
      List<AddMenuOption> options = new List<AddMenuOption>();
      foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs.Where<ThingDef>((Func<ThingDef, bool>) (def => def.thingClass != (Type) null && (def.thingClass == typeof (Apparel) || def.thingClass.IsSubclassOf(typeof (Apparel))) && this.CanWearWithoutDroppingAnything(def) && Util.CanWear(def, this.Statue.Data.bodyType))))
      {
        ThingDef localDef = thingDef;
        options.Add(new AddMenuOption(localDef, (Action) (() => this.Statue.AddApparel(localDef))));
      }
      Find.WindowStack.Add((Window) new Dialog_AddOptionLister((IEnumerable<AddMenuOption>) options, this.Statue.Data.bodyType));
    }

    private bool CanWearWithoutDroppingAnything(ThingDef apDef)
    {
      for (int index = 0; index < this.Statue.Data.wornApparelDefs.Count; ++index)
      {
        if (!ApparelUtility.CanWearTogether(apDef, this.Statue.Data.wornApparelDefs[index], this.Statue.Data.raceDef.race.body))
          return false;
      }
      return true;
    }

    private void CopyFromColonist()
    {
      List<FloatMenuOption> floatMenuOptionList = new List<FloatMenuOption>();
      List<Pawn> allColonists = this.getAllColonists();
      GenCollection.SortBy<Pawn, bool>(allColonists, (Func<Pawn, bool>) (x => x.Dead));
      foreach (Pawn pawn in allColonists)
      {
        Pawn localColonist = pawn;
        Action action1 = (Action) (() =>
        {
          string str = TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.ConfirmationCopyFromColonist"));
          Action action2 = (Action) (() => this.CopyFromColonistInternal(localColonist));
          Find.WindowStack.Add((Window) Dialog_MessageBox.CreateConfirmation(TaggedString.op_Implicit(str), action2, false, (string) null, (WindowLayer) 1));
        });
        string str1 = ((Entity) localColonist).LabelShort;
        if (localColonist.Dead)
          str1 = TaggedString.op_Implicit(TaggedString.op_Addition(str1, Translator.Translate("StatueOfColonist.PostLabelDeadColonist")));
        FloatMenuOption floatMenuOption = new FloatMenuOption(str1, action1, (MenuOptionPriority) 4, (Action<Rect>) null, (Thing) null, 0.0f, (Func<Rect, bool>) null, (WorldObject) null, true, 0);
        floatMenuOptionList.Add(floatMenuOption);
      }
      Find.WindowStack.Add((Window) new FloatMenu(floatMenuOptionList));
    }

    private void CopyFromColonistInternal(Pawn p)
    {
      this.Statue.Data = new StatueOfColonistData(p, ((Thing) this.Statue).DrawColor, "Map/Cutout");
      this.UpdateRace();
      this.Statue.ResolveArt(p);
      this.UpdateButtonLabel();
      if (!StatueOfColonistMod.Settings.autoName)
        return;
      this.Statue.name = ((Entity) p).LabelShort;
    }

    private List<Pawn> getAllColonists()
    {
      List<Map> mapList = new List<Map>();
      mapList.AddRange((IEnumerable<Map>) Find.Maps);
      GenCollection.SortBy<Map, bool, int>(mapList, (Func<Map, bool>) (x => !x.IsPlayerHome), (Func<Map, int>) (x => x.uniqueID));
      List<Pawn> allColonists = new List<Pawn>();
      for (int index1 = 0; index1 < mapList.Count; ++index1)
      {
        allColonists.AddRange((IEnumerable<Pawn>) mapList[index1].mapPawns.FreeColonists);
        List<Thing> thingList1 = mapList[index1].listerThings.ThingsInGroup((ThingRequestGroup) 8);
        for (int index2 = 0; index2 < thingList1.Count; ++index2)
        {
          if (!RottableUtility.IsDessicated(thingList1[index2]) && thingList1[index2] is Corpse corpse)
          {
            Pawn innerPawn = corpse.InnerPawn;
            if (innerPawn != null && innerPawn.IsColonist)
              allColonists.Add(innerPawn);
          }
        }
        List<Pawn> allPawnsSpawned = mapList[index1].mapPawns.AllPawnsSpawned;
        for (int index3 = 0; index3 < allPawnsSpawned.Count; ++index3)
        {
          if (allPawnsSpawned[index3].carryTracker.CarriedThing is Corpse carriedThing && !RottableUtility.IsDessicated((Thing) carriedThing) && carriedThing.InnerPawn.IsColonist)
            allColonists.Add(carriedThing.InnerPawn);
        }
        List<Thing> thingList2 = mapList[index1].listerThings.ThingsInGroup((ThingRequestGroup) 33);
        for (int index4 = 0; index4 < thingList2.Count; ++index4)
        {
          if (!RottableUtility.IsDessicated(thingList2[index4]) && thingList2[index4] is Building_Grave buildingGrave && ((Building_CorpseCasket) buildingGrave).Corpse != null)
          {
            Pawn innerPawn = ((Building_CorpseCasket) buildingGrave).Corpse.InnerPawn;
            if (innerPawn != null && innerPawn.IsColonist)
              allColonists.Add(innerPawn);
          }
        }
      }
      return allColonists;
    }

    private void OpenPresetListDialog()
    {
      Find.WindowStack.Add((Window) new Dialog_PresetList(this.statue, this));
    }
  }
}
