// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Dialog_PresetList
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
  [StaticConstructorOnStartup]
  public class Dialog_PresetList : Window
  {
    public static readonly Texture2D DeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);
    private const float BoxMargin = 20f;
    private const float EntrySpacing = 3f;
    private const float EntryMargin = 1f;
    private const float NameExtraLeftMargin = 15f;
    private const float InfoExtraLeftMargin = 270f;
    private const float DeleteButtonSpace = 5f;
    private const float EntryHeight = 36f;
    private const float NameTextFieldWidth = 400f;
    private const float NameTextFieldHeight = 35f;
    private const float NameTextFieldButtonSpace = 20f;
    private float bottomAreaHeight = 85f;
    private List<StatueOfColonistPreset> presets = new List<StatueOfColonistPreset>();
    private Vector2 scrollPosition = Vector2.zero;
    private string typingName = string.Empty;
    private bool focusedNameArea;
    private Building_StatueOfColonist statue;
    private Window_EditStatue parent;
    private static readonly Color DefaultPresetTextColor = new Color(1f, 1f, 0.6f);
    private static readonly Color InvalidPresetTextColor = new Color(1f, 0.0f, 0.0f);

    public virtual Vector2 InitialSize => new Vector2(600f, 700f);

    public Dialog_PresetList(Building_StatueOfColonist statue, Window_EditStatue parent)
    {
      this.doCloseButton = true;
      this.doCloseX = true;
      this.forcePause = true;
      this.absorbInputAroundWindow = true;
      this.resizeable = true;
      this.statue = statue;
      this.parent = parent;
      this.LoadSettings();
    }

    public virtual void DoWindowContents(Rect inRect)
    {
      Vector2 vector2_1;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_1).\u002Ector(((Rect) ref inRect).width - 16f, 36f);
      Vector2 vector2_2;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_2).\u002Ector(100f, vector2_1.y - 2f);
      ref Rect local1 = ref inRect;
      ((Rect) ref local1).height = ((Rect) ref local1).height - 45f;
      float num1 = (float) this.presets.Count * (vector2_1.y + 3f);
      Rect rect1;
      // ISSUE: explicit constructor call
      ((Rect) ref rect1).\u002Ector(0.0f, 0.0f, ((Rect) ref inRect).width - 16f, num1);
      Rect rect2;
      // ISSUE: explicit constructor call
      ((Rect) ref rect2).\u002Ector(GenUI.AtZero(inRect));
      ref Rect local2 = ref rect2;
      ((Rect) ref local2).height = ((Rect) ref local2).height - this.bottomAreaHeight;
      Widgets.BeginScrollView(rect2, ref this.scrollPosition, rect1, true);
      float num2 = 0.0f;
      int num3 = 0;
      foreach (StatueOfColonistPreset preset in this.presets)
      {
        StatueOfColonistPreset current = preset;
        Rect rect3;
        // ISSUE: explicit constructor call
        ((Rect) ref rect3).\u002Ector(0.0f, num2, vector2_1.x, vector2_1.y);
        if (num3 % 2 == 0)
          Widgets.DrawAltRect(rect3);
        Rect rect4 = GenUI.ContractedBy(rect3, 1f);
        GUI.BeginGroup(rect4);
        GUI.color = this.PresetNameColor(current);
        Rect rect5 = new Rect(15f, 0.0f, ((Rect) ref rect4).width, ((Rect) ref rect4).height);
        Text.Anchor = (TextAnchor) 3;
        Text.Font = (GameFont) 1;
        Widgets.Label(rect5, current.name);
        TooltipHandler.TipRegion(rect5, TipSignal.op_Implicit(current.GetToolTip()));
        GUI.color = Color.white;
        Rect rect6 = new Rect(270f, 0.0f, 200f, ((Rect) ref rect4).height);
        GUI.color = Color.white;
        Text.Anchor = (TextAnchor) 0;
        Text.Font = (GameFont) 1;
        float num4 = vector2_1.x - 2f - vector2_2.x - vector2_2.y;
        if (Widgets.ButtonText(new Rect(num4, 0.0f, vector2_2.x, vector2_2.y), TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.LoadPreset")), true, false, current.IsValid, new TextAnchor?()))
          this.Load(current);
        string bufferWeight = current.bufferWeight;
        Rect rect7 = new Rect(num4 - 90f, 6f, 80f, 24f);
        TooltipHandler.TipRegion(rect7, TipSignal.op_Implicit(Translator.Translate("StatueOfColonist.WeightInputFormDesc")));
        string str = Widgets.TextField(rect7, bufferWeight);
        current.bufferWeight = str;
        float result;
        if (str.IsFullyTypedFloat() && float.TryParse(str, out result))
          current.weight = result;
        Rect rect8 = new Rect((float) ((double) num4 + (double) vector2_2.x + 5.0), 0.0f, vector2_2.y, vector2_2.y);
        if (Widgets.ButtonImage(rect8, Dialog_PresetList.DeleteX, true))
          Find.WindowStack.Add((Window) Dialog_MessageBox.CreateConfirmation(TranslatorFormattedStringExtensions.Translate("ConfirmDelete", NamedArgument.op_Implicit(current.name)), (Action) (() => this.Remove(current)), true, (string) null, (WindowLayer) 1));
        TooltipHandler.TipRegion(rect8, TipSignal.op_Implicit(Translator.Translate("StatueOfColonist.DeleteThisPreset")));
        GUI.EndGroup();
        num2 += vector2_1.y + 3f;
        ++num3;
      }
      Widgets.EndScrollView();
      this.DoTypeInField(GenUI.AtZero(inRect));
    }

    public void Load(StatueOfColonistPreset preset)
    {
      this.statue.CopyStatueOfColonistFromPreset(preset);
      this.parent.UpdateButtonLabel();
      this.Close(true);
    }

    public void Save(StatueOfColonistPreset preset)
    {
      this.presets.Add(preset);
      this.SaveSettings();
    }

    public void Remove(StatueOfColonistPreset preset)
    {
      this.presets.Remove(preset);
      this.SaveSettings();
    }

    public void LoadSettings()
    {
      this.presets = StatueOfColonistPref.presets;
      if (this.presets != null)
        return;
      Log.Message("StatueOfColonistPreset is null");
      this.presets = new List<StatueOfColonistPreset>();
    }

    public void SaveSettings()
    {
      StatueOfColonistPref.presets = this.presets;
      StatueOfColonistPref.SavePref();
    }

    public void DoTypeInField(Rect rect)
    {
      GUI.BeginGroup(rect);
      bool flag = Event.current.type == 4 && Event.current.keyCode == 13;
      float num = ((Rect) ref rect).height - 52f;
      Text.Font = (GameFont) 1;
      Text.Anchor = (TextAnchor) 3;
      GUI.SetNextControlName("MapNameField");
      string str = Widgets.TextField(new Rect(5f, num, 400f, 35f), this.typingName);
      if (GenText.IsValidFilename(str))
        this.typingName = str;
      if (!this.focusedNameArea)
      {
        UI.FocusControl("MapNameField", (Window) this);
        this.focusedNameArea = true;
      }
      if (Widgets.ButtonText(new Rect(420f, num, (float) ((double) ((Rect) ref rect).width - 400.0 - 20.0), 35f), TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.SavePresetButton")), true, false, true, new TextAnchor?()) | flag)
      {
        if (GenText.NullOrEmpty(this.typingName))
          Messages.Message(TaggedString.op_Implicit(Translator.Translate("NeedAName")), MessageTypeDefOf.RejectInput, true);
        else
          this.Save(new StatueOfColonistPreset(this.typingName, this.statue));
      }
      Text.Anchor = (TextAnchor) 0;
      GUI.EndGroup();
    }

    public Color PresetNameColor(StatueOfColonistPreset preset)
    {
      return preset.IsValid ? Dialog_PresetList.DefaultPresetTextColor : Dialog_PresetList.InvalidPresetTextColor;
    }
  }
}
