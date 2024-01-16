// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Comment
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Tree
{
  public class Comment : Node
  {
    public string Value { get; set; }

    public bool IsValidCss { get; set; }

    public bool IsSpecialCss { get; set; }

    public bool IsPreSelectorComment { get; set; }

    private bool IsCSSHack { get; set; }

    public Comment(string value)
    {
      this.Value = value;
      this.IsValidCss = !value.StartsWith("//");
      this.IsSpecialCss = value.StartsWith("/**") || value.StartsWith("/*!");
      this.IsCSSHack = value == "/**/" || value == "/*\\*/";
    }

    protected override Node CloneCore() => (Node) new Comment(this.Value);

    public override void AppendCSS(Env env)
    {
      if (this.IsReference || env.IsCommentSilent(this.IsValidCss, this.IsCSSHack, this.IsSpecialCss))
        return;
      env.Output.Append(this.Value);
      if (this.IsCSSHack || !this.IsPreSelectorComment)
        return;
      env.Output.Append("\n");
    }
  }
}
