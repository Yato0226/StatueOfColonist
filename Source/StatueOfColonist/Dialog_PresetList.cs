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

    public override void DoWindowContents(Rect inRect)
    {
      Vector2 vector2_1 = new Vector2(inRect.width - 16f, 36f);
      Vector2 vector2_2 = new Vector2(100f, vector2_1.y - 2f);
      inRect.height -= 45f;
      float num1 = this.presets.Count * (vector2_1.y + 3f);
      Rect rect1 = new Rect(0.0f, 0.0f, inRect.width - 16f, num1);
      Rect rect2 = GenUI.AtZero(inRect);
      rect2.height -= this.bottomAreaHeight;
      Widgets.BeginScrollView(rect2, ref this.scrollPosition, rect1, true);
      float num2 = 0.0f;
      int num3 = 0;
      foreach (StatueOfColonistPreset preset in this.presets)
      {
        StatueOfColonistPreset current = preset;
        Rect rect3 = new Rect(0.0f, num2, vector2_1.x, vector2_1.y);
        if (num3 % 2 == 0)
          Widgets.DrawAltRect(rect3);
        Rect rect4 = GenUI.ContractedBy(rect3, 1f);
        GUI.BeginGroup(rect4);
        GUI.color = this.PresetNameColor(current);
        Rect rect5 = new Rect(15f, 0.0f, rect4.width, rect4.height);
        Text.Anchor = TextAnchor.UpperCenter;
        Text.Font = GameFont.Medium;
        Widgets.Label(rect5, current.name);
        TooltipHandler.TipRegion(rect5, current.GetToolTip());
        GUI.color = Color.white;
        Rect rect6 = new Rect(270f, 0.0f, 200f, rect4.height);
        GUI.color = Color.white;
        Text.Anchor = TextAnchor.UpperLeft;
        Text.Font = GameFont.Medium;
        float num4 = vector2_1.x - 2f - vector2_2.x - vector2_2.y;
        if (Widgets.ButtonText(new Rect(num4, 0.0f, vector2_2.x, vector2_2.y), Translator.Translate("StatueOfColonist.LoadPreset"), true, false, current.IsValid, null))
          this.Load(current);
        string bufferWeight = current.bufferWeight;
        Rect rect7 = new Rect(num4 - 90f, 6f, 80f, 24f);
        TooltipHandler.TipRegion(rect7, Translator.Translate("StatueOfColonist.WeightInputFormDesc"));
        string str = Widgets.TextField(rect7, bufferWeight);
        current.bufferWeight = str;
        float result;
        if (str.IsFullyTypedFloat() && float.TryParse(str, out result))
          current.weight = result;
        Rect rect8 = new Rect(num4 + vector2_2.x + 5f, 0.0f, vector2_2.y, vector2_2.y);
        if (Widgets.ButtonImage(rect8, Dialog_PresetList.DeleteX, true))
          Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(TranslatorFormattedStringExtensions.Translate("ConfirmDelete", current.name), () => this.Remove(current), true, null, WindowLayer.Dialog));
        TooltipHandler.TipRegion(rect8, Translator.Translate("StatueOfColonist.DeleteThisPreset"));
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
      bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
      float num = rect.height - 52f;
      Text.Font = GameFont.Medium;
      Text.Anchor = TextAnchor.UpperCenter;
      GUI.SetNextControlName("MapNameField");
      string str = Widgets.TextField(new Rect(5f, num, 400f, 35f), this.typingName);
      if (GenText.IsValidFilename(str))
        this.typingName = str;
      if (!this.focusedNameArea)
      {
        UI.FocusControl("MapNameField", this);
        this.focusedNameArea = true;
      }
      if (Widgets.ButtonText(new Rect(420f, num, rect.width - 400f - 20f, 35f), Translator.Translate("StatueOfColonist.SavePresetButton"), true, false, true) || flag)
      {
        if (GenText.NullOrEmpty(this.typingName))
          Messages.Message(Translator.Translate("NeedAName"), MessageTypeDefOf.RejectInput, true);
        else
          this.Save(new StatueOfColonistPreset(this.typingName, this.statue));
      }
      Text.Anchor = TextAnchor.UpperLeft;
      GUI.EndGroup();
    }

    public Color PresetNameColor(StatueOfColonistPreset preset)
    {
      return preset.IsValid ? DefaultPresetTextColor : InvalidPresetTextColor;
    }
  }
}
