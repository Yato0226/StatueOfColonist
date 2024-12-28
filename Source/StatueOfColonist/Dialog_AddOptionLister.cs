// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Dialog_AddOptionLister
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace StatueOfColonist
{
    internal class Dialog_AddOptionLister : Window
    {
        protected List<AddMenuOption> options;
        protected const float ButSpacing = 0.0f;
        protected Vector2 scrollPosition;
        protected Listing_Standard listing;
        private BodyTypeDef bodyType;
        protected static readonly Vector2 ButSize = new Vector2(230f, 27f);
        protected readonly float ColumnSpacing = 20f;
        protected readonly float SectSpacing = 8f;

        public virtual Vector2 InitialSize => new Vector2(640f, 480f);

        public virtual bool IsDebug => true;

        public Dialog_AddOptionLister(IEnumerable<AddMenuOption> options, BodyTypeDef bodyType)
        {
            this.optionalTitle = Translator.Translate("StatueOfColonist.DialogAddOptionListerTitle");
            this.doCloseX = true;
            this.onlyOneOfTypeAllowed = true;
            this.absorbInputAroundWindow = true;
            this.bodyType = bodyType;
            this.options = options.ToList<AddMenuOption>();
        }

        public override void DoWindowContents(Rect inRect)
        {
            float clothIconSize = StatueOfColonistMod.Settings.clothIconSize;
            Rect rect1 = new Rect(inRect.x, inRect.y, inRect.width, inRect.height);
            this.listing = new Listing_Standard();
            this.listing.ColumnWidth = clothIconSize;
            float num = (float)(((double)clothIconSize + (double)this.listing.verticalSpacing) * 6.0);
            if ((double)num < (double)rect1.height)
                num = rect1.height;
            Rect rect2 = new Rect(0.0f, 0.0f, (clothIconSize + 17f) * (float)(this.options.Count / 6 + 1), num);
            Widgets.BeginScrollView(rect1, ref this.scrollPosition, rect2, true);
            this.listing.Begin(rect2);
            this.DoListingItems();
            this.listing.End();
            Widgets.EndScrollView();
        }

        public virtual void PostClose()
        {
            base.PostClose();
            UI.UnfocusCurrentControl();
        }

        protected void DoListingItems()
        {
            foreach (AddMenuOption option in this.options)
                this.AddAction(option.ThingDef, option.Method);
        }

        private void AddAction(ThingDef thingDef, Action action)
        {
            float clothIconSize = StatueOfColonistMod.Settings.clothIconSize;
            if (this.listing.ButtonThing(thingDef, clothIconSize, clothIconSize, Color.white, this.bodyType))
            {
                this.Close(true);
                action();
            }
            GUI.color = Color.white;
        }
    }
}
