// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Plugins.RtlPlugin
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Nop.Plugin.InstantSearch.Dotless.Plugins
{
  [DisplayName("Rtl")]
  [Description("Reverses some css when in rtl mode")]
  public class RtlPlugin : VisitorPlugin
  {
    public RtlPlugin(bool onlyReversePrefixedRules, bool forceRtlTransform)
      : this()
    {
      this.OnlyReversePrefixedRules = onlyReversePrefixedRules;
      this.ForceRtlTransform = forceRtlTransform;
    }

    public RtlPlugin() => this.PropertiesToReverse = (IEnumerable<string>) new List<string>()
    {
      "border-left",
      "border-right",
      "border-top-left-radius",
      "border-top-right-radius",
      "border-bottom-left-radius",
      "border-bottom-right-radius",
      "border-width",
      "margin",
      "padding",
      "float",
      "right",
      "left",
      "text-align"
    };

    public bool OnlyReversePrefixedRules { get; set; }

    public bool ForceRtlTransform { get; set; }

    public IEnumerable<string> PropertiesToReverse { get; set; }

    public override VisitorPluginType AppliesTo => VisitorPluginType.AfterEvaluation;

    public override void OnPreVisiting(Env env)
    {
      base.OnPreVisiting(env);
      bool flag = this.ForceRtlTransform || CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;
      this.PrefixesToProcess = new List<RtlPlugin.Prefix>();
      if (!this.OnlyReversePrefixedRules & flag)
      {
        foreach (string str in this.PropertiesToReverse)
          this.PrefixesToProcess.Add(new RtlPlugin.Prefix()
          {
            KeepRule = true,
            PrefixString = str,
            RemovePrefix = false,
            Reverse = true
          });
      }
      this.PrefixesToProcess.Add(new RtlPlugin.Prefix()
      {
        PrefixString = "-rtl-reverse-",
        RemovePrefix = true,
        KeepRule = true,
        Reverse = flag
      });
      this.PrefixesToProcess.Add(new RtlPlugin.Prefix()
      {
        PrefixString = "-ltr-reverse-",
        RemovePrefix = true,
        KeepRule = true,
        Reverse = !flag
      });
      this.PrefixesToProcess.Add(new RtlPlugin.Prefix()
      {
        PrefixString = "-rtl-ltr-",
        RemovePrefix = true,
        KeepRule = true,
        Reverse = false
      });
      this.PrefixesToProcess.Add(new RtlPlugin.Prefix()
      {
        PrefixString = "-ltr-rtl-",
        RemovePrefix = true,
        KeepRule = true,
        Reverse = false
      });
      this.PrefixesToProcess.Add(new RtlPlugin.Prefix()
      {
        PrefixString = "-rtl-",
        RemovePrefix = true,
        KeepRule = flag
      });
      this.PrefixesToProcess.Add(new RtlPlugin.Prefix()
      {
        PrefixString = "-ltr-",
        RemovePrefix = true,
        KeepRule = !flag
      });
    }

    public override Node Execute(Node node, out bool visitDeeper)
    {
      if (node is Rule rule)
      {
        visitDeeper = false;
        string lowerInvariant = (rule.Name ?? "").ToLowerInvariant();
        foreach (RtlPlugin.Prefix prefix in this.PrefixesToProcess)
        {
          if (lowerInvariant.StartsWith(prefix.PrefixString))
          {
            if (!prefix.KeepRule)
              return (Node) null;
            if (prefix.RemovePrefix)
              rule.Name = rule.Name.Substring(prefix.PrefixString.Length);
            if (!prefix.Reverse)
              return (Node) rule;
            if (rule.Name.IndexOf("right", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
              rule.Name = this.Replace(rule.Name, "right", "left", StringComparison.InvariantCultureIgnoreCase);
              return (Node) rule;
            }
            if (rule.Name.IndexOf("left", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
              rule.Name = this.Replace(rule.Name, "left", "right", StringComparison.InvariantCultureIgnoreCase);
              return (Node) rule;
            }
            return rule.Name.IndexOf("top", StringComparison.InvariantCultureIgnoreCase) >= 0 || rule.Name.IndexOf("bottom", StringComparison.InvariantCultureIgnoreCase) >= 0 ? (Node) rule : (Node) new RtlPlugin.ValuesReverserVisitor().ReverseRule(rule);
          }
        }
      }
      visitDeeper = true;
      return node;
    }

    private string Replace(
      string haystack,
      string needle,
      string replacement,
      StringComparison comparisonType)
    {
      int length = haystack.IndexOf(needle, comparisonType);
      return length < 0 ? haystack : haystack.Substring(0, length) + replacement + haystack.Substring(length + needle.Length);
    }

    private List<RtlPlugin.Prefix> PrefixesToProcess { get; set; }

    private class Prefix
    {
      public string PrefixString { get; set; }

      public bool KeepRule { get; set; }

      public bool Reverse { get; set; }

      public bool RemovePrefix { get; set; }
    }

    private class ValuesReverserVisitor : IVisitor
    {
      private StringBuilder _textContent = new StringBuilder();
      private List<Node> _nodeContent = new List<Node>();

      public Rule ReverseRule(Rule rule)
      {
        rule.Accept((IVisitor) this);
        string contents = this._textContent.ToString();
        string important = "";
        if (rule.Value is Value obj)
          important = obj.Important;
        bool flag = false;
        if (this._nodeContent.Count > 1)
        {
          if (this._nodeContent.Count == 4)
          {
            Node node = this._nodeContent[1];
            this._nodeContent[1] = this._nodeContent[3];
            this._nodeContent[3] = node;
            return new Rule(rule.Name, (Node) new Value((IEnumerable<Node>) new Expression[1]
            {
              new Expression((IEnumerable<Node>) this._nodeContent)
            }, important)).ReducedFrom<Rule>((Node) rule);
          }
        }
        else
        {
          switch (contents)
          {
            case "left":
              contents = ("right " + important).TrimEnd();
              flag = true;
              break;
            case "right":
              contents = ("left " + important).TrimEnd();
              flag = true;
              break;
            default:
              string[] strArray = contents.Split(new char[1]
              {
                ' '
              }, StringSplitOptions.RemoveEmptyEntries);
              if (strArray.Length == 4)
              {
                string str = strArray[1];
                strArray[1] = strArray[3];
                strArray[3] = str;
                contents = string.Join(" ", strArray);
                flag = true;
                break;
              }
              break;
          }
          if (flag)
            return new Rule(rule.Name, (Node) new TextNode(contents)).ReducedFrom<Rule>((Node) rule);
        }
        return rule;
      }

      public Node Visit(Node node)
      {
        switch (node)
        {
          case TextNode textNode:
            this._textContent.Append(textNode.Value);
            this._nodeContent.Add((Node) textNode);
            return node;
          case Number number:
            this._nodeContent.Add((Node) number);
            return node;
          case Keyword keyword:
            this._nodeContent.Add((Node) keyword);
            this._textContent.Append(keyword.Value);
            return node;
          default:
            node.Accept((IVisitor) this);
            return node;
        }
      }
    }
  }
}
