// Decompiled with JetBrains decompiler
// Type: StatueOfColonist.Dialog_NameStatue
// Assembly: StatueOfColonist, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D39CEE1-1E34-4063-B520-8223C22194A1
// Assembly location: C:\Users\louiz\source\repos\Statue of colonist\1.5\Assemblies\StatueOfColonist.dll

using UnityEngine;
using Verse;

#nullable disable
namespace StatueOfColonist
{
  public class Dialog_NameStatue : Window
  {
    private Building_StatueOfColonist statue;
    private string curName;

    public virtual Vector2 InitialSize => new Vector2(500f, 120f);

    public Dialog_NameStatue(Building_StatueOfColonist statue)
    {
      this.statue = statue;
      if (statue.name == null)
        statue.name = "";
      this.optionalTitle = TaggedString.op_Implicit(Translator.Translate("StatueOfColonist.CommandRenameLabel"));
      this.curName = statue.name;
      this.forcePause = true;
      this.closeOnAccept = false;
      this.closeOnCancel = false;
      this.absorbInputAroundWindow = true;
    }

    public virtual void DoWindowContents(Rect rect)
    {
      Text.Font = (GameFont) 1;
      bool flag = false;
      if (Event.current.type == 4 && Event.current.keyCode == 13)
      {
        flag = true;
        Event.current.Use();
      }
      this.curName = Widgets.TextField(new Rect(0.0f, 0.0f, (float) ((double) ((Rect) ref rect).width / 2.0 - 20.0), 35f), this.curName);
      if (!(Widgets.ButtonText(new Rect((float) ((double) ((Rect) ref rect).width / 2.0 + 20.0), 0.0f, (float) ((double) ((Rect) ref rect).width / 2.0 - 20.0), 35f), TaggedString.op_Implicit(Translator.Translate("OK")), true, false, true, new TextAnchor?()) | flag))
        return;
      this.statue.name = this.curName;
      Find.WindowStack.TryRemove((Window) this, true);
      Event.current.Use();
    }
  }
}
