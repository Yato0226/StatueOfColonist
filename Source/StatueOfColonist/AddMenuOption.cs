// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.AddMenuOption
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using System;
using Verse;

namespace StatueOfColonist
{
    public struct AddMenuOption
    {
        public ThingDef ThingDef { get; }
        public Action Method { get; }

        public AddMenuOption(ThingDef thingDef, Action method)
        {
            ThingDef = thingDef;
            Method = method;
        }
    }
}