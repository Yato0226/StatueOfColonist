// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueGraphicGetter
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using UnityEngine;
using Verse;

#nullable disable
namespace StatueOfColonist
{
  public class StatueGraphicGetter
  {
    public static Graphic GetNakedBodyGraphic(
      BodyTypeDef bodyType,
      Shader shader,
      Color skinColor,
      float scale)
    {
      if (bodyType == null)
      {
        Log.Error("Getting naked body graphic with undefined body type.");
        bodyType = BodyTypeDefOf.Male;
      }
      return GraphicDatabase.Get<Graphic_Multi>("Things/Pawn/Humanlike/Bodies/" + ("Naked_" + bodyType.ToString()), shader, Vector2.op_Multiply(Vector2.one, scale), skinColor);
    }
  }
}
