// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.AddMenuOption
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using System;
using Verse;

#nullable disable
namespace StatueOfColonist
{
  public struct AddMenuOption(ThingDef thingDef, Action method)
  {
    public ThingDef thingDef = thingDef;
    public Action method = method;
  }
}
