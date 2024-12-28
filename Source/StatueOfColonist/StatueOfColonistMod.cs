﻿// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistMod
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace StatueOfColonist
{
  public class StatueOfColonistMod : Mod
  {
    public StatueOfColonistSettings settings;
    private List<string> buffers = new List<string>()
    {
      "64",
      "1",
      "1",
      "1",
      "1",
      "1",
      "0",
      "0",
      "0",
      "-0.1",
      "0",
      "0.05",
      "0",
      "0",
      "0",
      "-0.1",
      "0.2",
      "0.3",
      "0.3",
      "0.05",
      "0.05",
      "0"
    };
    private bool isInitBuffers;

    public static StatueOfColonistSettings Settings
    {
      get => LoadedModManager.GetMod<StatueOfColonistMod>().settings;
    }

    public StatueOfColonistMod(ModContentPack content)
      : base(content)
    {
      this.settings = this.GetSettings<StatueOfColonistSettings>();
      this.settings.Refresh();
    }

    public virtual void DoSettingsWindowContents(Rect inRect)
    {
      float y = inRect.y;
      Text.Font = (GameFont) 1;
      Widgets.CheckboxLabeled(new Rect(inRect.x, y, 500f, 24f), Translator.Translate("StatueOfColonist.AutoNameStatue").ToString(), ref this.settings.autoName, false, null, null, false);
      float num1 = y + 32f;
      Widgets.CheckboxLabeled(new Rect(inRect.x, num1, 500f, 24f), Translator.Translate("StatueOfColonist.ShowClothForDisplay").ToString(), ref this.settings.showClothForDisplay, false, null, null, false);
      float num2 = num1 + 32f;
      if (!this.isInitBuffers)
      {
        this.buffers[0] = this.settings.clothIconSize.ToString();
        float scaleOfCloth;
        for (int index1 = 0; index1 < 5; ++index1)
        {
          BodyTypeDef bodyTypeDef = index1.ToBodyTypeDef();
          List<string> buffers = this.buffers;
          int index2 = index1 + 1;
          scaleOfCloth = this.settings.GetScaleOfCloth(bodyTypeDef);
          string str = scaleOfCloth.ToString();
          buffers[index2] = str;
        }
        this.buffers[6] = this.settings.possibilityOfStatueFromPreset.ToString();
        for (int index3 = 0; index3 < 15; ++index3)
        {
          List<string> buffers = this.buffers;
          int index4 = index3 + 7;
          scaleOfCloth = this.settings.offsetStatueBody[index3];
          string str = scaleOfCloth.ToString();
          buffers[index4] = str;
        }
        this.isInitBuffers = true;
      }
      string buffer1 = this.buffers[0];
      Rect rect1 = new Rect(0.0f, num2, 400f, 24f);
      TextAnchor anchor1 = Text.Anchor;
      Text.Anchor = (TextAnchor) 3;
      TaggedString taggedString = Translator.Translate("StatueOfColonist.ClothIconSize");
      Widgets.Label(rect1, taggedString);
      Text.Anchor = anchor1;
      string str1 = Widgets.TextField(new Rect(400f, num2, 100f, 24f), buffer1);
      this.buffers[0] = str1;
      bool autoNameValue;
      if (bool.TryParse(str1, out autoNameValue))
        this.settings.autoName = autoNameValue; // Update the setting based on the input
      float result1;
      if (str1.IsFullyTypedFloat() && float.TryParse(str1, out result1))
        this.settings.clothIconSize = result1;
      float num3 = num2 + 36f;
      Rect rect2 = new Rect(inRect.x, num3, 500f, Text.CalcHeight(Translator.Translate("StatueOfColonist.ScaleOfCloth").ToString(), 500f));
      Widgets.Label(rect2, Translator.Translate("StatueOfColonist.ScaleOfCloth"));
      float num4 = num3 + (rect2.height + 4f); // Corrected line
      for (int index = 0; index < 5; ++index)
      {
        BodyTypeDef bodyTypeDef = index.ToBodyTypeDef();
        string buffer2 = this.buffers[index + 1];
        Rect rect3 = new Rect(0.0f, num4, 100f, 24f);
        TextAnchor anchor2 = Text.Anchor;
        Text.Anchor = (TextAnchor) 3;
        string name = bodyTypeDef.GetName();
        Widgets.Label(rect3, name);
        Text.Anchor = anchor2;
        string str2 = Widgets.TextField(new Rect(100f, num4, 100f, 24f), buffer2);
        float result2 = 0; // Declare result2 before usage
        if (str2.IsFullyTypedFloat() && float.TryParse(str2, out result2))
          this.settings.SetScaleOfCloth(bodyTypeDef, result2); // Update the setting based on the input
        num4 += 24f;
        this.buffers[index + 1] = str2;
      }
      float num5 = num4 + 12f;
      string buffer3 = this.buffers[6];
      Rect rect4 = new Rect(0.0f, num5, 400f, 24f);
      TextAnchor anchor3 = Text.Anchor;
      Text.Anchor = (TextAnchor) 3;
      Widgets.Label(rect4, Translator.Translate("StatueOfColonist.StatueFromPreset"));
      TooltipHandler.TipRegion(rect4, Translator.Translate("StatueOfColonist.StatueFromPresetDesc"));
      Text.Anchor = anchor3;
      string str3 = Widgets.TextField(new Rect(400f, num5, 100f, 24f), buffer3);
      this.buffers[6] = str3;
      float result3 = 0; // Declare result3 before usage
      if (str3.IsFullyTypedFloat() && float.TryParse(str3, out result3))
        this.settings.possibilityOfStatueFromPreset = result3; // Update the setting based on the input
      float num6 = num5 + 36f;
      Rect rect5 = new Rect(inRect.x, num6, 500f, Text.CalcHeight(Translator.Translate("StatueOfColonist.ScaleOfCloth").ToString(), 500f));
      Widgets.Label(rect5, Translator.Translate("StatueOfColonist.OffsetStatueBody"));
      float num7 = num6 + (rect5.height + 2f); // Corrected line
      TextAnchor anchor4 = Text.Anchor;
      Text.Anchor = (TextAnchor) 4;
      Rect rect6 = new Rect(inRect.x + 100f, num7, 100f, Text.CalcHeight(Translator.Translate("StatueOfColonist.OffsetStatueBodyNormal").ToString(), 500f));
      Widgets.Label(rect6, Translator.Translate("StatueOfColonist.OffsetStatueBodyNormal"));
      Widgets.Label(new Rect(inRect.x + 205f, num7, 100f, Text.CalcHeight(Translator.Translate("StatueOfColonist.OffsetStatueBodyLarge").ToString(), 500f)), Translator.Translate("StatueOfColonist.OffsetStatueBodyLarge"));
      Widgets.Label(new Rect(inRect.x + 310f, num7, 100f, Text.CalcHeight(Translator.Translate("StatueOfColonist.OffsetStatueBodyGrand").ToString(), 500f)), Translator.Translate("StatueOfColonist.OffsetStatueBodyGrand"));
      Text.Anchor = anchor4;
      float num8 = num7 + rect6.height;
      for (int index5 = 0; index5 < 5; ++index5)
      {
        BodyTypeDef bodyTypeDef = index5.ToBodyTypeDef();
        Rect rect7 = new Rect(0.0f, num8, 100f, 24f);
        TextAnchor anchor5 = Text.Anchor;
        Text.Anchor = (TextAnchor) 3;
        string name = bodyTypeDef.GetName();
        Widgets.Label(rect7, name);
        Text.Anchor = anchor5;
        for (int index6 = 0; index6 < 3; ++index6)
        {
          int index7 = index5 * 3 + index6;
          string buffer4 = this.buffers[index7 + 7];
          string str4 = Widgets.TextField(new Rect((float)(100.0 + (double)index6 * 105.0), num8, 100f, 24f), buffer4);
          this.buffers[index7 + 7] = str4;
          float result4;
          if (str4.IsFullyTypedFloat() && float.TryParse(str4, out result4))
            this.settings.offsetStatueBody[index7] = result4;
        }
        num8 += 24f;
      }
      Text.Font = (GameFont) 2;
    }

    public virtual string SettingsCategory() => "Statue Of Colonist";
  }
}
