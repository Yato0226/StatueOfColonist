// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Blueprint_Install_DeSpawn_Patch
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using HarmonyLib;
using RimWorld;
using Verse;

 
namespace StatueOfColonist
{
  [HarmonyPatch(typeof (Blueprint_Install))]
  [HarmonyPatch("DeSpawn")]
  internal class Blueprint_Install_DeSpawn_Patch
  {
    private static void Prefix(Blueprint_Install __instance)
    {
      if (!(MinifyUtility.GetInnerIfMinified(__instance.MiniToInstallOrBuildingToReinstall) is Building_StatueOfColonist) || !((Thing) __instance).Map.dynamicDrawManager.DrawThingsForReading.Contains((Thing) __instance))
        return;
      ((Thing) __instance).Map.dynamicDrawManager.DeRegisterDrawable((Thing) __instance);
    }
  }
}
