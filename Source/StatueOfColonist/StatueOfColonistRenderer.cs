﻿﻿﻿﻿﻿// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.StatueOfColonistRenderer
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using StatueOfColonist; // Remove duplicate

// Explicitly specify the Object namespace to resolve ambiguity
using Object = UnityEngine.Object;

namespace StatueOfColonist
{
if ((double) horizontalOffset != 0.0 && head.narrow && headFacing.IsHorizontal)
=======
if ((double) horizontalOffset != 0.0 && head.narrow && headFacing.IsHorizontal)

// Explicitly specify the Object namespace to resolve ambiguity
using Object = UnityEngine.Object;

 
namespace StatueOfColonist
{
  public class StatueOfColonistRenderer
  {
    private const float YOffset_Behind = 0.00390625f;
    private const float YOffset_Body = 0.0078125f;
    private const float YOffset_Wounds = 0.01953125f;
    private const float YOffset_Shell = 0.0234375f;
    private const float YOffset_Head = 0.02734375f;
    private const float YOffset_OnHead = 0.03125f;
    public Building_StatueOfColonist parent;

    private RotDrawMode CurRotDrawMode => RotDrawMode.Default; // Ensure this is a valid default value

    public StatueOfColonistRenderer(Building_StatueOfColonist parent) => this.parent = parent;

    public void Render( // Ensure this method signature is correct
      StatueOfColonistGraphicSet graphics,
      Vector3 rootLoc,
      Quaternion quat,
      bool renderBody,
      Rot4 bodyFacing,
      Rot4 headFacing,
      RotDrawMode bodyDrawType,
      bool portrait,
      bool headStump,
      float scale,
      Building_StatueOfColonist.HeadRenderMode headRenderMode,
      ThingDef raceDef,
      BodyTypeDef bodyTypeDef,
      LifeStageDef lifeStageDef)
    {
      Graphics_Internal_DrawMesh_Patch.scale = scale;
      if (graphics.ShouldResolve)
        graphics.ResolveAllGraphics();
      Mesh mesh = (Mesh) null;
      if (renderBody)
      {
        Vector3 vector3 = rootLoc;
        vector3.y += 1f / 128f;
        mesh = this.GetBodyMesh(portrait, raceDef, bodyFacing, graphics.data.lifeStageDef);
        List<Material> materialList = graphics.MatsBodyBaseAt(bodyFacing, bodyDrawType);
        for (int index = 0; index < materialList.Count; ++index)
        {
          GenDraw.DrawMeshNowOrLater(mesh, vector3, quat, materialList[index], portrait);
          vector3.y += 1f / 256f;
        }
        if (bodyDrawType == null)
          rootLoc.y += 5f / 256f;
      }
      Vector3 vector3_1 = rootLoc;
      Vector3 vector3_2 = rootLoc;
        if (!bodyFacing.Equals(Rot4.North))
      {
        vector3_2.y += 7f / 256f;
        vector3_1.y += 3f / 128f;
      }
      else
      {
        vector3_2.y += 3f / 128f;
        vector3_1.y += 7f / 256f;
      }
      Vector3 headOffset = Vector3.zero;
      if (graphics.headGraphic != null)
      {
        Vector3 vector3_3 = Quaternion.op_Multiply(quat, this.BaseHeadOffsetAt(headFacing, graphics.data.bodyType, raceDef, lifeStageDef));
        headOffset = new Vector3(vector3_3.x, vector3_3.y, vector3_3.z);
        vector3_3 = new Vector3(vector3_3.x * scale, vector3_3.y, vector3_3.z * scale);
        Material material1 = graphics.HeadMatAt(headFacing, bodyDrawType, headStump);
        if (material1 != null)
          GenDraw.DrawMeshNowOrLater(this.GetHeadMesh(portrait, raceDef, headFacing, graphics.data.lifeStageDef), Vector3.op_Addition(vector3_2, vector3_3), quat, material1, portrait);
        Vector3 beardLoc = Vector3.op_Addition(rootLoc, vector3_3);
        beardLoc.y += 1f / 32f;
        bool flag = false;
        if (!portrait || !Prefs.HatsOnlyOnMap)
        {
          Mesh hairMesh = this.GetHairMesh(graphics, portrait, raceDef, headFacing, graphics.data.lifeStageDef, graphics.data.crownType);
          List<ApparelGraphicRecord> apparelGraphics = graphics.apparelGraphics;
          for (int index = 0; index < apparelGraphics.Count; ++index)
          {
            if (((Thing) apparelGraphics[index].sourceApparel).def.apparel.LastLayer == ApparelLayerDefOf.Overhead)
            {
              switch (headRenderMode)
              {
                case Building_StatueOfColonist.HeadRenderMode.Default:
                  flag = true;
                  break;
                case Building_StatueOfColonist.HeadRenderMode.HairOnly:
                  continue;
              }
              Material material2 = apparelGraphics[index].graphic.MatAt(bodyFacing, (Thing) null);
              Vector3 vector3_4 = beardLoc;
              vector3_4.y += 1f / 1000f * (float) (index + 1) * scale;
              GenDraw.DrawMeshNowOrLater(hairMesh, vector3_4, quat, material2, portrait);
            }
          }
        }
        PawnRenderFlags flags = (PawnRenderFlags) 0;
        TryDrawGenes(GeneDrawLayer.Base);
        if (ModsConfig.IdeologyActive && graphics.faceTattooGraphic != null && bodyDrawType != 2 && (Rot4.op_Inequality(bodyFacing, Rot4.North) || this.parent.Data.faceTattooDef.visibleNorth))
        {
          Vector3 vector3_5 = Vector3.op_Addition(rootLoc, headOffset);
          vector3_5.y += 0.0289575271f;
          vector3_5.y -= 0.00144787633f;
          GenDraw.DrawMeshNowOrLater(this.GetHairMesh(graphics, portrait, raceDef, headFacing, graphics.data.lifeStageDef, graphics.data.crownType), vector3_5, quat, graphics.faceTattooGraphic?.MatAt(headFacing, (Thing) null), PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
        }
        TryDrawGenes(GeneDrawLayer.Head);
        if ((Rot4.op_Equality(bodyFacing, Rot4.North) ? 1 : (this.parent.Data.beardDef == BeardDefOf.NoBeard ? 1 : 0)) == 0 && bodyDrawType != 2 && !PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 2) && this.parent.Data.beardDef != null)
        {
          Vector3 vector3_6 = Vector3.op_Addition(this.OffsetBeardLocationForCrownType(this.parent.Data.beardDef, this.parent.Data.crownType, headFacing, beardLoc), this.parent.Data.beardDef.GetOffset(this.parent.Data.crownType, headFacing));
          Mesh beardMesh = this.GetBeardMesh(graphics, portrait, raceDef, headFacing, graphics.data.lifeStageDef, graphics.data.crownType);
          Material material3 = graphics.beardGraphic?.MatAt(headFacing, (Thing) null);
        if (material3 != null)
            GenDraw.DrawMeshNowOrLater(beardMesh, vector3_6, quat, material3, PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
        }
        if (!flag && bodyDrawType != 2 && !headStump && graphics != null)
        {
          Mesh hairMesh = this.GetHairMesh(graphics, portrait, raceDef, headFacing, graphics.data.lifeStageDef, graphics.data.crownType);
          Material material4 = graphics.HairMatAt(headFacing);
        if (hairMesh != null && material4 != null)
            GenDraw.DrawMeshNowOrLater(hairMesh, beardLoc, quat, material4, portrait);
        }
        TryDrawGenes(GeneDrawLayer.Body);
        TryDrawGenes(GeneDrawLayer.Hair);

        void DrawGene(GeneGraphic geneRecord, GeneDrawLayer layer)
        {
          Vector3 vector3 = this.HeadGeneDrawLocation(geneRecord.sourceGene, this.parent.Data.crownType, headFacing, Vector3.op_Addition(rootLoc, headOffset), layer);
          Material material = geneRecord.graphic.MatAt(headFacing, (Thing) null);
          GenDraw.DrawMeshNowOrLater(graphics.HairMeshSet.MeshAt(headFacing), vector3, quat, material, PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
        }

        void DrawGeneEyes(GeneGraphic geneRecord)
        {
          DrawExtraEyeGraphic(geneRecord.graphic, geneRecord.sourceGene.graphicData.drawScale * this.parent.Data.lifeStageDef.eyeSizeFactor.GetValueOrDefault(1f), 0.0012f, true, true);
        }

        void TryDrawGenes(GeneDrawLayer layer)
        {
          if (!ModLister.BiotechInstalled || PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 2))
            return;
          List<GeneGraphic> geneGraphics = graphics.geneGraphics;
          for (int index = 0; index < geneGraphics.Count; ++index)
          {
            if (geneGraphics[index].sourceGene.CanDrawNow(bodyFacing, layer))
            {
              if (geneGraphics[index].sourceGene.graphicData.drawOnEyes)
                DrawGeneEyes(geneGraphics[index]);
              else
                DrawGene(geneGraphics[index], layer);
            }
          }
        }

        void DrawExtraEyeGraphic(
          Graphic graphic,
          float scale_,
          float yOffset,
          bool drawLeft,
          bool drawRight)
        {
          bool narrowCrown = this.parent.Data.crownType.narrow;
          Vector3? eyeOffsetEastWest = this.parent.Data.crownType.eyeOffsetEastWest;
          Vector3 vector3_1 = Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(rootLoc, headOffset), new Vector3(0.0f, 0.0260617733f + yOffset, 0.0f)), Quaternion.op_Multiply(quat, new Vector3(0.0f, 0.0f, -0.25f)));
          BodyTypeDef.WoundAnchor woundAnchor1 = GenCollection.FirstOrDefault<BodyTypeDef.WoundAnchor>(this.parent.Data.bodyType.woundAnchors, (Predicate<BodyTypeDef.WoundAnchor>) (wa =>
          {
            if (wa.tag == "LeftEye")
            {
              Rot4? rotation = wa.rotation;
              Rot4 rot4 = headFacing;
              if ((rotation.HasValue ? (rotation.HasValue ? (Rot4.op_Equality(rotation.GetValueOrDefault(), rot4) ? 1 : 0) : 1) : 0) != 0)
                return Rot4.op_Equality(headFacing, Rot4.South) || wa.narrowCrown.GetValueOrDefault() == narrowCrown;
            }
            return false;
          }));
          BodyTypeDef.WoundAnchor woundAnchor2 = GenCollection.FirstOrDefault<BodyTypeDef.WoundAnchor>(this.parent.Data.bodyType.woundAnchors, (Predicate<BodyTypeDef.WoundAnchor>) (wa =>
          {
            if (wa.tag == "RightEye")
            {
              Rot4? rotation = wa.rotation;
              Rot4 rot4 = headFacing;
              if ((rotation.HasValue ? (rotation.HasValue ? (Rot4.op_Equality(rotation.GetValueOrDefault(), rot4) ? 1 : 0) : 1) : 0) != 0)
                return Rot4.op_Equality(headFacing, Rot4.South) || wa.narrowCrown.GetValueOrDefault() == narrowCrown;
            }
            return false;
          }));
          Material material = graphic.MatAt(headFacing, (Thing) null);
        if (headFacing.Equals(Rot4.South))
          {
            if (woundAnchor1 == null || woundAnchor2 == null)
              return;
            if (drawLeft)
              GenDraw.DrawMeshNowOrLater(MeshPool.GridPlaneFlip(Vector2.op_Multiply(Vector2.one, scale_)), Matrix4x4.TRS(Vector3.op_Addition(vector3_1, Quaternion.op_Multiply(quat, woundAnchor1.offset)), quat, Vector3.one), material, PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
            if (drawRight)
              GenDraw.DrawMeshNowOrLater(MeshPool.GridPlane(Vector2.op_Multiply(Vector2.one, scale_)), Matrix4x4.TRS(Vector3.op_Addition(vector3_1, Quaternion.op_Multiply(quat, woundAnchor2.offset)), quat, Vector3.one), material, PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
          }
        if (headFacing.Equals(Rot4.East) && drawRight)
          {
            if (woundAnchor2 == null)
              return;
            Vector3 vector3_2 = eyeOffsetEastWest ?? woundAnchor2.offset;
            GenDraw.DrawMeshNowOrLater(MeshPool.GridPlane(Vector2.op_Multiply(Vector2.one, scale_)), Matrix4x4.TRS(Vector3.op_Addition(vector3_1, Quaternion.op_Multiply(quat, vector3_2)), quat, Vector3.one), material, PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
          }
          if (!(Rot4.op_Equality(headFacing, Rot4.West) & drawLeft) || woundAnchor1 == null)
            return;
          Vector3 vector3_3 = woundAnchor1.offset;
          if (eyeOffsetEastWest.HasValue)
            vector3_3 = Vector2Utility.ScaledBy(eyeOffsetEastWest.Value, new Vector3(-1f, 1f, 1f));
          GenDraw.DrawMeshNowOrLater(MeshPool.GridPlaneFlip(Vector2.op_Multiply(Vector2.one, scale_)), Matrix4x4.TRS(Vector3.op_Addition(vector3_1, Quaternion.op_Multiply(quat, vector3_3)), quat, Vector3.one), material, PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
        }
      }
      if (renderBody)
      {
        Vector3 vector3_7 = rootLoc;
        float angle = 0.0f;
        vector3_7.y += Rot4.op_Equality(bodyFacing, Rot4.South) ? 0.00579150533f : 0.0289575271f;
        Vector3 vector3_8 = vector3_1;
        PawnRenderFlags flags = (PawnRenderFlags) 0;
        if (ModsConfig.IdeologyActive && graphics.bodyTattooGraphic != null && bodyDrawType != 2 && (Rot4.op_Inequality(bodyFacing, Rot4.North) || this.parent.Data.bodyTattooDef.visibleNorth))
        {
          Vector3 vector3_9 = rootLoc;
          vector3_9.y += 0.0101351338f;
          GenDraw.DrawMeshNowOrLater(this.GetBodyOverlayMeshSet(portrait)?.MeshAt(bodyFacing), vector3_9, quat, graphics.bodyTattooGraphic?.MatAt(bodyFacing, (Thing) null), PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
        }
        if (graphics.furCoveredGraphic != null)
        {
          Vector3 shellLoc = rootLoc;
          shellLoc.y += 0.009187258f;
          this.DrawPawnFur(graphics, shellLoc, bodyFacing, quat, mesh, flags);
        }
        Quaternion quat1 = Quaternion.AngleAxis(angle, Vector3.up);
        for (int index = 0; index < graphics.apparelGraphics.Count; ++index)
        {
          ApparelGraphicRecord apparelGraphic = graphics.apparelGraphics[index];
          if (((Thing) apparelGraphic.sourceApparel).def.apparel.LastLayer == ApparelLayerDefOf.Shell && !((Thing) apparelGraphic.sourceApparel).def.apparel.shellRenderedBehindHead)
          {
            Material material = apparelGraphic.graphic?.MatAt(bodyFacing, (Thing) null);
            Vector3 vector3_10 = vector3_8;
            if (((Thing) apparelGraphic.sourceApparel).def.apparel.shellCoversHead)
              vector3_10.y += 0.00289575267f;
            GenDraw.DrawMeshNowOrLater(mesh, vector3_10, quat1, material, portrait);
          }
          if (PawnRenderer.RenderAsPack(apparelGraphic.sourceApparel))
          {
            Material material = apparelGraphic.graphic.MatAt(bodyFacing, (Thing) null);
            if (((Thing) apparelGraphic.sourceApparel).def.apparel.wornGraphicData != null)
            {
              Vector2 vector2_1 = ((Thing) apparelGraphic.sourceApparel).def.apparel.wornGraphicData.BeltOffsetAt(bodyFacing, this.parent.Data.bodyType);
              Vector2 vector2_2 = ((Thing) apparelGraphic.sourceApparel).def.apparel.wornGraphicData.BeltScaleAt(bodyFacing, this.parent.Data.bodyType);
              Matrix4x4 matrix4x4 = Matrix4x4.op_Multiply(Matrix4x4.op_Multiply(Matrix4x4.Translate(vector3_7), Matrix4x4.Translate(new Vector3(vector2_1.x, 0.0f, vector2_1.y))), Matrix4x4.Scale(new Vector3(vector2_2.x, 1f, vector2_2.y)));
              GenDraw.DrawMeshNowOrLater(mesh, matrix4x4, material, true);
            }
            else
              GenDraw.DrawMeshNowOrLater(mesh, vector3_8, quat1, material, true);
          }
        }
        if (ModLister.BiotechInstalled && !GenList.NullOrEmpty<GeneData>((IList<GeneData>) this.parent.Data.genes))
          this.DrawBodyGenes(graphics, vector3_1, quat1, angle, bodyFacing, bodyDrawType, flags);
      }
      if (!((Thing) this.parent).def.castEdgeShadows && this.parent.Data.raceDef?.race?.specialShadowData != null)
      {
        if (graphics.shadowGraphic == null)
          graphics.shadowGraphic = new Graphic_Shadow(this.parent.Data.raceDef.race.specialShadowData);
        ((Graphic) graphics.shadowGraphic).Draw(rootLoc, Rot4.North, (Thing) this.parent, 0.0f);
      }
      this.DrawAddons(graphics, portrait, vector3_1, headOffset, raceDef, quat, bodyFacing, bodyTypeDef, scale);
      Graphics_Internal_DrawMesh_Patch.Reset();
    }

    public virtual Mesh GetBodyMesh(
      bool portrait,
      ThingDef raceDef,
      Rot4 bodyFacing,
      LifeStageDef lifeStageDef)
    {
      return lifeStageDef.bodyWidth.HasValue ? MeshPool.GetMeshSetForWidth(lifeStageDef.bodyWidth.Value).MeshAt(bodyFacing) : MeshPool.humanlikeBodySet.MeshAt(bodyFacing);
    }

    public virtual GraphicMeshSet GetBodyMeshSet(
      bool portrait,
      ThingDef raceDef,
      LifeStageDef lifeStageDef)
    {
      return lifeStageDef.bodyWidth.HasValue ? MeshPool.GetMeshSetForWidth(lifeStageDef.bodyWidth.Value) : MeshPool.humanlikeBodySet;
    }

    public virtual Mesh GetHeadMesh(
      bool portrait,
      ThingDef raceDef,
      Rot4 headFacing,
      LifeStageDef lifeStageDef)
    {
      return lifeStageDef.bodyWidth.HasValue ? MeshPool.GetMeshSetForWidth(lifeStageDef.bodyWidth.Value).MeshAt(headFacing) : MeshPool.humanlikeHeadSet.MeshAt(headFacing);
    }

    public virtual Mesh GetHairMesh(
      StatueOfColonistGraphicSet graphics,
      bool portrait,
      ThingDef raceDef,
      Rot4 headFacing,
      LifeStageDef lifeStageDef,
      HeadTypeDef headTypeDef)
    {
      Vector2 vector2 = headTypeDef.hairMeshSize;
      if (lifeStageDef.headSizeFactor.HasValue)
        vector2 = Vector2.op_Multiply(vector2, lifeStageDef.headSizeFactor.Value);
      return MeshPool.GetMeshSetForWidth(vector2.x, vector2.y).MeshAt(headFacing);
    }

    public virtual Mesh GetBeardMesh(
      StatueOfColonistGraphicSet graphics,
      bool portrait,
      ThingDef raceDef,
      Rot4 headFacing,
      LifeStageDef lifeStageDef,
      HeadTypeDef headTypeDef)
    {
      Vector2 vector2 = headTypeDef.beardMeshSize;
      if (lifeStageDef.headSizeFactor.HasValue)
        vector2 = Vector2.op_Multiply(vector2, lifeStageDef.headSizeFactor.Value);
      return MeshPool.GetMeshSetForWidth(vector2.x, vector2.y).MeshAt(headFacing);
    }

    public virtual void DrawAddons(
      StatueOfColonistGraphicSet graphics,
      bool portrait,
      Vector3 vector,
      Vector3 headOffset,
      ThingDef raceDef,
      Quaternion quat,
      Rot4 rotation,
      BodyTypeDef bodyTypeDef,
      float scale)
    {
    }

    public Vector3 BaseHeadOffsetAt(
      Rot4 rotation,
      BodyTypeDef bodyType,
      ThingDef raceDef,
      LifeStageDef lifeStageDef)
    {
      Vector2 headOffset = this.GetHeadOffset(rotation, bodyType, raceDef, lifeStageDef);
      Vector3 zero;
switch (rotation.AsInt)
      {
        case 0:
        zero = new Vector3(0.0f, 0.0f, headOffset.y);
          break;
        case 1:
        zero = new Vector3(headOffset.x, 0.0f, headOffset.y);
          break;
        case 2:
        zero = new Vector3(0.0f, 0.0f, headOffset.y);
          break;
        case 3:
        zero = new Vector3(-headOffset.x, 0.0f, headOffset.y);
          break;
        default:
          zero = Vector3.zero;
          break;
      }
      return zero;
    }

    public virtual Vector2 GetHeadOffset(
      Rot4 rotation,
      BodyTypeDef bodyType,
      ThingDef raceDef,
      LifeStageDef lifeStageDef)
    {
      return Vector2.op_Multiply(bodyType.headOffset, Mathf.Sqrt(lifeStageDef.bodySizeFactor));
    }

    private Vector3 OffsetBeardLocationForCrownType(
      BeardDef beardDef,
      HeadTypeDef head,
      Rot4 headFacing,
      Vector3 beardLoc)
    {
      if (Rot4.op_Equality(headFacing, Rot4.East))
        beardLoc = Vector3.op_Addition(beardLoc, Vector3.op_Multiply(Vector3.right, head.beardOffsetXEast));
      else if (Rot4.op_Equality(headFacing, Rot4.West))
        beardLoc = Vector3.op_Addition(beardLoc, Vector3.op_Multiply(Vector3.left, head.beardOffsetXEast));
      beardLoc.y += 0.0260617733f;
      beardLoc = Vector3.op_Addition(beardLoc, head.beardOffset);
      beardLoc = Vector3.op_Addition(beardLoc, beardDef.GetOffset(head, headFacing));
      return beardLoc;
    }

    private GraphicMeshSet GetBodyOverlayMeshSet(bool portrait)
    {
      if (!this.parent.Data.raceDef.race.Humanlike)
        return MeshPool.humanlikeBodySet;
      BodyTypeDef bodyType = this.parent.Data.bodyType;
      if (bodyType == BodyTypeDefOf.Male)
        return MeshPool.humanlikeBodySet_Male;
      if (bodyType == BodyTypeDefOf.Female)
        return MeshPool.humanlikeBodySet_Female;
      if (bodyType == BodyTypeDefOf.Thin)
        return MeshPool.humanlikeBodySet_Thin;
      if (bodyType == BodyTypeDefOf.Fat)
        return MeshPool.humanlikeBodySet_Fat;
      return bodyType == BodyTypeDefOf.Hulk ? MeshPool.humanlikeBodySet_Hulk : this.GetBodyMeshSet(portrait, this.parent.Data.raceDef, this.parent.Data.lifeStageDef);
    }

    private void DrawBodyGenes(
      StatueOfColonistGraphicSet graphics,
      Vector3 rootLoc,
      Quaternion quat,
      float angle,
      Rot4 bodyFacing,
      RotDrawMode bodyDrawType,
      PawnRenderFlags flags)
    {
      Vector2 bodyGraphicScale = this.parent.Data.bodyType.bodyGraphicScale;
      float num = (float) (((double) bodyGraphicScale.x + (double) bodyGraphicScale.y) / 2.0);
      foreach (GeneGraphic geneGraphic in graphics.geneGraphics)
      {
        GeneGraphicData graphicData = geneGraphic.sourceGene.graphicData;
        if (graphicData.drawLoc == 3 && (bodyDrawType != 2 || geneGraphic.sourceGene.graphicData.drawWhileDessicated))
        {
          Vector3 vector3_1 = graphicData.DrawOffsetAt(bodyFacing);
          vector3_1.x *= bodyGraphicScale.x;
          vector3_1.z *= bodyGraphicScale.y;
          Vector3 vector3_2;
          // ISSUE: explicit constructor call
vector3_2 = new Vector3(graphicData.drawScale * num, 1f, graphicData.drawScale * num);
          Matrix4x4 matrix4x4 = Matrix4x4.TRS(Vector3.op_Addition(rootLoc, Vector3Utility.RotatedBy(vector3_1, angle)), quat, vector3_2);
          Material material = geneGraphic.graphic.MatAt(bodyFacing, (Thing) null);
          GenDraw.DrawMeshNowOrLater(Rot4.op_Equality(bodyFacing, Rot4.West) ? MeshPool.GridPlaneFlip(Vector2.one) : MeshPool.GridPlane(Vector2.one), matrix4x4, material, PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
        }
      }
    }

    private Vector3 HeadGeneDrawLocation(
      GeneDef geneDef,
      HeadTypeDef head,
      Rot4 headFacing,
      Vector3 geneLoc,
      GeneDrawLayer layer)
    {
      if (layer != 1)
      {
        if (layer - 3 <= 1)
          geneLoc.y += 0.03335328f;
        else
          geneLoc.y += 0.0289575271f;
      }
      else
        geneLoc.y += 0.0260617733f;
      geneLoc = Vector3.op_Addition(geneLoc, geneDef.graphicData.DrawOffsetAt(headFacing));
      float horizontalOffset = geneDef.graphicData.narrowCrownHorizontalOffset;
      if ((double) horizontalOffset != 0.0 && head.narrow && ((Rot4) ref headFacing).IsHorizontal)
      {
        if (Rot4.op_Equality(headFacing, Rot4.East))
          geneLoc = Vector3.op_Addition(geneLoc, Vector3.op_Multiply(Vector3.right, 0.0f - horizontalOffset));
        else if (Rot4.op_Equality(headFacing, Rot4.West))
          geneLoc = Vector3.op_Addition(geneLoc, Vector3.op_Multiply(Vector3.right, horizontalOffset));
        geneLoc = Vector3.op_Addition(geneLoc, Vector3.op_Multiply(Vector3.forward, 0.0f - horizontalOffset));
      }
      return geneLoc;
    }

    private void DrawPawnFur(
      StatueOfColonistGraphicSet graphics,
      Vector3 shellLoc,
      Rot4 facing,
      Quaternion quat,
      Mesh mesh,
      PawnRenderFlags flags)
    {
      Material material = graphics.FurMatAt(facing);
      GenDraw.DrawMeshNowOrLater(mesh, shellLoc, quat, material, PawnRenderFlagsExtension.FlagSet(flags, (PawnRenderFlags) 8));
    }
  }
}
