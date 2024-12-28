// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.ListingUtil
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

 
namespace StatueOfColonist
{
  public static class ListingUtil
  {
    public static bool ButtonThing(
      this Listing_Standard listing,
      ThingDef thingDef,
      float width,
      float height,
      Color color,
      BodyTypeDef bodyType)
    {
      Traverse traverse = Traverse.Create((object) listing);
      traverse.Method("NewColumnIfNeeded", new object[1]
      {
        (object) height
      }).GetValue();
      Rect butRect = new Rect(traverse.Field("curX").GetValue<float>(), traverse.Field("curY").GetValue<float>(), width, height);
      Texture2D tex = (Texture2D) null;
      float texScale = 1f;
      if (!StatueOfColonistMod.Settings.showClothForDisplay || !thingDef.TryGetApparelTexture(bodyType, out tex))
        tex = ((BuildableDef) thingDef).uiIcon;
      else if (thingDef.apparel.LastLayer != ApparelLayerDefOf.Overhead)
        texScale = StatueOfColonistMod.Settings.GetScaleOfCloth(bodyType);
      bool flag = ListingUtil.ButtonScaledImage(butRect, tex, color, texScale);
      TooltipHandler.TipRegion(butRect, TipSignal.op_Implicit(((Def) thingDef).LabelCap));
      ((Listing) listing).Gap(height + ((Listing) listing).verticalSpacing);
      return flag;
    }

    public static bool TryGetApparelTexture(
      this ThingDef def,
      BodyTypeDef bodyType,
      out Texture2D tex,
      string postfix = "")
    {
      tex = (Texture2D) null;
      if (bodyType == null)
        bodyType = BodyTypeDefOf.Male;
      if (def.apparel == null || GenText.NullOrEmpty(def.apparel.wornGraphicPath))
        return false;
      return ListingUtil.HasTexture(def.apparel.wornGraphicPath, out tex, postfix) || ListingUtil.HasTexture(def.apparel.wornGraphicPath + "_" + ((Def) bodyType).defName, out tex, postfix);
    }

    public static bool HasTexture(string path, out Texture2D tex, string postfix = "")
    {
      tex = ContentFinder<Texture2D>.Get(path + postfix, false);
      if (!Object.op_Equality((Object) tex, (Object) null))
        return true;
      Log.Message(path + " is not found.");
      return false;
    }

    public static bool ButtonScaledImage(Rect butRect, Texture2D tex, Color color, float texScale)
    {
      GUI.color = !Mouse.IsOver(butRect) ? color : GenUI.MouseoverColor;
      GUI.DrawTexture(GenUI.ScaledBy(new Rect(butRect), texScale), (Texture) tex);
      GUI.color = color;
      return Widgets.ButtonInvisible(butRect, false);
    }
  }
}
