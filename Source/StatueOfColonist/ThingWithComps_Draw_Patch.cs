// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.ThingWithComps_Draw_Patch
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
  [HarmonyPatch(typeof (ThingWithComps))]
  [HarmonyPatch("Draw")]
  internal class ThingWithComps_Draw_Patch
  {
    private static Color BlueprintColor = new Color(0.5f, 0.5f, 1f, 0.35f);

    private static void Postfix(ThingWithComps __instance)
    {
      if (!(__instance is Blueprint_Install blueprintInstall))
        return;
      Thing buildingToReinstall = blueprintInstall.MiniToInstallOrBuildingToReinstall;
      if (!((buildingToReinstall != null ? MinifyUtility.GetInnerIfMinified(buildingToReinstall) : (Thing) null) is Building_StatueOfColonist innerIfMinified))
        return;
      if (!innerIfMinified.IsValid)
      {
        Pawn pawn = GenCollection.RandomElement<Pawn>((IEnumerable<Pawn>) ((Thing) __instance).Map.mapPawns.FreeColonistsAndPrisoners);
        innerIfMinified.ResolveGraphics(pawn);
      }
      Vector3 rootLoc = Vector3.op_Addition(((Thing) __instance).DrawPos, new Vector3(innerIfMinified.Def.offsetX, 0.0f, innerIfMinified.Def.offsetZ));
      if (GenList.NullOrEmpty<Graphic>((IList<Graphic>) innerIfMinified.GetStatueGraphics(Building_StatueOfColonist.RenderMode.Blueprint).bodyAddonGraphics))
        innerIfMinified.GetStatueGraphics(Building_StatueOfColonist.RenderMode.Blueprint).ResolveAllGraphics();
      innerIfMinified.Render(rootLoc, Quaternion.identity, true, ((Thing) __instance).Rotation, ((Thing) __instance).Rotation, (RotDrawMode) 0, false, false, innerIfMinified.Def.scale, Building_StatueOfColonist.RenderMode.Blueprint);
    }
  }
}
