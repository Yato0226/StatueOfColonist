// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Util
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

 
namespace StatueOfColonist
{
  public static class Util
  {
    public static string GetName(this BodyTypeDef bodyType) => ((Def) bodyType).defName;

    public static BodyTypeDef ToBodyTypeDef(this int value)
    {
      return new BodyTypeDef[5]
      {
        BodyTypeDefOf.Male,
        BodyTypeDefOf.Female,
        BodyTypeDefOf.Thin,
        BodyTypeDefOf.Hulk,
        BodyTypeDefOf.Fat
      }[value];
    }

    public static int ToInt(this BodyTypeDef def)
    {
      return GenCollection.FirstIndexOf<BodyTypeDef>((IEnumerable<BodyTypeDef>) new BodyTypeDef[5]
      {
        BodyTypeDefOf.Male,
        BodyTypeDefOf.Female,
        BodyTypeDefOf.Thin,
        BodyTypeDefOf.Hulk,
        BodyTypeDefOf.Fat
      }, (Func<BodyTypeDef, bool>) (d => d == def));
    }

    public static HairDef GetHairDefFromGraphicPath(string graphicPath)
    {
      return DefDatabase<HairDef>.AllDefsListForReading.Find((Predicate<HairDef>) (h => ((StyleItemDef) h).texPath == graphicPath));
    }

    public static bool IsFullyTypedFloat(this string str)
    {
      if (str == string.Empty)
        return false;
      string[] strArray = str.Split('.');
      return strArray.Length <= 2 && strArray.Length >= 1 && Util.ContainsOnlyCharacters(strArray[0], "-0123456789") && (strArray.Length != 2 || Util.ContainsOnlyCharacters(strArray[1], "0123456789"));
    }

    private static bool ContainsOnlyCharacters(string str, string allowedChars)
    {
      for (int index = 0; index < str.Length; ++index)
      {
        if (!allowedChars.Contains<char>(str[index]))
          return false;
      }
      return true;
    }

    public static bool CanWear(ThingDef apDef, BodyTypeDef bodyType)
    {
      return Object.op_Inequality((Object) ContentFinder<Texture2D>.Get((apDef.apparel.LastLayer == ApparelLayerDefOf.Overhead || apDef.apparel.LastLayer.IsUtilityLayer && (apDef.apparel.wornGraphicData == null || apDef.apparel.wornGraphicData.renderUtilityAsPack) ? apDef.apparel.wornGraphicPath : apDef.apparel.wornGraphicPath + "_" + ((Def) bodyType).defName) + "_north", false), (Object) null);
    }

    public static bool TryGetApparelTexture(
      this ThingDef def,
      BodyTypeDef bodyType,
      out Texture2D tex,
      Rot4 rot,
      out bool flipped)
    {
      tex = (Texture2D) null;
      flipped = false;
      if (def.apparel == null)
        return false;
      if (!GenText.NullOrEmpty(def.apparel.wornGraphicPath) || GenList.NullOrEmpty<string>((IList<string>) def.apparel.wornGraphicPaths))
        return Util.HasApparelTexture(def.apparel.wornGraphicPath, bodyType, out tex, rot, out flipped);
      foreach (string wornGraphicPath in def.apparel.wornGraphicPaths)
      {
        if (Util.HasApparelTexture(wornGraphicPath, bodyType, out tex, rot, out flipped))
          return true;
      }
      return false;
    }

    private static bool HasApparelTexture(
      string wornGraphicPath,
      BodyTypeDef bodyType,
      out Texture2D tex,
      Rot4 rot,
      out bool flipped)
    {
      tex = (Texture2D) null;
      flipped = false;
      if (GenText.NullOrEmpty(wornGraphicPath))
        return false;
      return Util.HasTexture(wornGraphicPath, out tex, rot, out flipped) || Util.HasTexture(wornGraphicPath + "_" + bodyType.ToString(), out tex, rot, out flipped);
    }

    private static bool HasTexture(string path, out Texture2D tex, Rot4 rot, out bool flipped)
    {
      flipped = false;
      string str = "";
      if (Rot4.op_Equality(rot, Rot4.North))
        str = "_north";
      else if (Rot4.op_Equality(rot, Rot4.East))
        str = "_east";
      else if (Rot4.op_Equality(rot, Rot4.South))
        str = "_south";
      else if (Rot4.op_Equality(rot, Rot4.West))
        str = "_west";
      tex = ContentFinder<Texture2D>.Get(path + str, false);
      if (Object.op_Equality((Object) tex, (Object) null))
      {
        if (Rot4.op_Equality(rot, Rot4.East))
        {
          tex = ContentFinder<Texture2D>.Get(path + "_west", false);
          flipped = true;
        }
        else if (Rot4.op_Equality(rot, Rot4.West))
        {
          tex = ContentFinder<Texture2D>.Get(path + "_east", false);
          flipped = true;
        }
      }
      return Object.op_Inequality((Object) tex, (Object) null);
    }
  }
}
