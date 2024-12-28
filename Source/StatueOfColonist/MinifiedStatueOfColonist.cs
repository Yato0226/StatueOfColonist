// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.MinifiedStatueOfColonist
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;

#nullable disable
namespace StatueOfColonist
{
  internal class MinifiedStatueOfColonist : MinifiedThing
  {
    private Building_StatueOfColonist Statue => this.InnerThing as Building_StatueOfColonist;

    public virtual void SpawnSetup(Map map, bool respawningAfterLoad)
    {
      ((ThingWithComps) this).SpawnSetup(map, respawningAfterLoad);
      Building_StatueOfColonist statue = this.Statue;
      if (statue == null)
        return;
      if (statue.IsValid)
        statue.InitializeStatue();
      else
        statue.ResolveGraphics();
    }

    [DebuggerHidden]
    public virtual IEnumerable<Gizmo> GetGizmos()
    {
      foreach (Gizmo gizmo in base.GetGizmos())
        yield return gizmo;
      Building_StatueOfColonist statue = this.Statue;
      if (statue != null)
      {
        foreach (Gizmo statueGizmo in statue.GetStatueGizmos())
          yield return statueGizmo;
      }
    }

    public virtual void DrawAt(Vector3 drawLoc, bool flip = false)
    {
      base.DrawAt(drawLoc, flip);
      Building_StatueOfColonist statue = this.Statue;
      if (statue == null)
        return;
      if (!statue.IsValid)
      {
        Pawn pawn = GenCollection.RandomElement<Pawn>((IEnumerable<Pawn>) Find.CurrentMap.mapPawns.FreeColonists);
        statue.ResolveGraphics(pawn);
      }
      Vector3 rootLoc = Vector3.op_Addition(drawLoc, new Vector3(statue.Def.offsetXMinified, -0.024f, statue.Def.offsetZMinified));
      statue.Render(rootLoc, Quaternion.identity, true, Rot4.South, Rot4.South, (RotDrawMode) 0, false, false, statue.Def.scaleMinified);
    }
  }
}
