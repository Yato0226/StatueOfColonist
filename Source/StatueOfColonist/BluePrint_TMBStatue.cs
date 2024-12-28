// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.BluePrint_TMBStatue
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

 
namespace StatueOfColonist
{
  public class BluePrint_TMBStatue : Blueprint_Install
  {
    private static Color BlueprintColor = new Color(0.5f, 0.5f, 1f, 0.35f);

    public virtual void SpawnSetup(Map map, bool respawningAfterLoad)
    {
      base.SpawnSetup(map, respawningAfterLoad);
      if (map.dynamicDrawManager.DrawThingsForReading.Contains((Thing) this))
        return;
      map.dynamicDrawManager.RegisterDrawable((Thing) this);
    }

    public virtual void DeSpawn(DestroyMode mode = 0)
    {
      if (((Thing) this).Map.dynamicDrawManager.DrawThingsForReading.Contains((Thing) this))
        ((Thing) this).Map.dynamicDrawManager.DeRegisterDrawable((Thing) this);
      base.DeSpawn(mode);
    }

    public virtual void Draw()
    {
      ((ThingWithComps) this).Draw();
      if (!(MinifyUtility.GetInnerIfMinified(this.MiniToInstallOrBuildingToReinstall) is Building_StatueOfColonist innerIfMinified))
        return;
      if (!innerIfMinified.IsValid)
      {
        Pawn pawn = GenCollection.RandomElement<Pawn>((IEnumerable<Pawn>) ((Thing) this).Map.mapPawns.FreeColonistsAndPrisoners);
        innerIfMinified.ResolveGraphics(pawn);
      }
      Vector3 rootLoc = Vector3.op_Addition(((Thing) this).DrawPos, new Vector3(innerIfMinified.Def.offsetX, 0.0f, innerIfMinified.Def.offsetZ));
      innerIfMinified.Render(rootLoc, Quaternion.identity, true, ((Thing) this).Rotation, ((Thing) this).Rotation, (RotDrawMode) 0, false, false, innerIfMinified.Def.scale, Building_StatueOfColonist.RenderMode.Blueprint);
    }
  }
}
