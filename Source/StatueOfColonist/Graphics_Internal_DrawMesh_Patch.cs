// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Graphics_Internal_DrawMesh_Patch
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using HarmonyLib;
using UnityEngine;

 
namespace StatueOfColonist
{
  [HarmonyPatch(typeof (Graphics))]
  [HarmonyPatch("Internal_DrawMesh")]
  public class Graphics_Internal_DrawMesh_Patch
  {
    public static float scale = 1f;

    public static void Prefix(ref Matrix4x4 matrix)
    {
      matrix.m00 *= Graphics_Internal_DrawMesh_Patch.scale;
      matrix.m22 *= Graphics_Internal_DrawMesh_Patch.scale;
    }

    public static void Reset() => Graphics_Internal_DrawMesh_Patch.scale = 1f;
  }
}
