// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Designator_Install_DrawGhost_Patch
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

 
namespace StatueOfColonist
{
  [HarmonyPatch(typeof (Designator_Install))]
  [HarmonyPatch("DrawGhost")]
  internal class Designator_Install_DrawGhost_Patch
  {
    private static bool Prefix(Designator_Install __instance, Color ghostCol)
    {
      Traverse traverse = Traverse.Create((object) __instance);
      Thing thing = traverse.Property("ThingToInstall", (object[]) null).GetValue<Thing>();
      if (thing == null || !(thing is Building_StatueOfColonist statueOfColonist) || statueOfColonist.Def == null)
        return true;
      if (!statueOfColonist.IsValid)
      {
        Pawn pawn = GenCollection.RandomElement<Pawn>((IEnumerable<Pawn>) ((Thing) statueOfColonist).Map.mapPawns.FreeColonists);
        statueOfColonist.ResolveGraphics(pawn);
      }
      if (statueOfColonist.Data == null)
        return true;
      Rot4 rot4 = traverse.Field("placingRot").GetValue<Rot4>();
      Vector3 vector3 = GenThing.TrueCenter(UI.MouseCell(), rot4, ((BuildableDef) statueOfColonist.Def).Size, Altitudes.AltitudeFor((AltitudeLayer) 24));
      Vector3 rootLoc = Vector3.op_Addition(vector3, new Vector3(statueOfColonist.Def.offsetX, 0.0f, statueOfColonist.Def.offsetZ));
      AcceptanceReport acceptanceReport = ((Designator) __instance).CanDesignateCell(UI.MouseCell());
      int num = ((AcceptanceReport) ref acceptanceReport).Accepted ? 1 : 0;
      Building_StatueOfColonist.RenderMode mode = Building_StatueOfColonist.RenderMode.CanDesignateGhost;
      if (num == 0)
        mode = Building_StatueOfColonist.RenderMode.CanNotDesignateGhost;
      statueOfColonist.Render(rootLoc, Quaternion.identity, true, rot4, rot4, (RotDrawMode) 0, false, false, statueOfColonist.Def.scale, mode);
      if (thing.Graphic != null)
      {
        Graphic innerGraphicFor = GraphicUtility.ExtractInnerGraphicFor(thing.Graphic, thing, new int?());
        IntVec3 intVec3 = IntVec3Utility.ToIntVec3(vector3);
        if (IntVec2.op_Equality(statueOfColonist.Def.size, new IntVec2(2, 2)))
          intVec3 = IntVec3.op_Addition(intVec3, new IntVec3(-1, 0, -1));
        GhostDrawer.DrawGhostThing(intVec3, Rot4.North, (ThingDef) ((Designator_Place) __instance).PlacingDef, innerGraphicFor, ghostCol, (AltitudeLayer) 24, (Thing) null, true, (ThingDef) null);
      }
      return false;
    }
  }
}
