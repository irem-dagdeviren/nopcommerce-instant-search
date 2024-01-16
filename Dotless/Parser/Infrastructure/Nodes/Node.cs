// Decompiled with JetBrains decompiler
// Type:
// .Parser.Infrastructure.Nodes.Node
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Plugins;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes
{
  public abstract class Node
  {
    private bool isReference;

    public bool IsReference
    {
      get => this.isReference;
      set => this.isReference = value;
    }

    public NodeLocation Location { get; set; }

    public NodeList PreComments { get; set; }

    public NodeList PostComments { get; set; }

    public static implicit operator bool(Node node) => node != null;

    public static bool operator true(Node n) => n != null;

    public static bool operator false(Node n) => n == null;

    public static bool operator !(Node n) => n == null;

    public static Node operator &(Node n1, Node n2) => n1 == null ? (Node) null : n2;

    public static Node operator |(Node n1, Node n2) => n1 ?? n2;

    public T ReducedFrom<T>(params Node[] nodes) where T : Node
    {
      foreach (Node node in nodes)
      {
        if (node != this)
        {
          this.Location = node.Location;
          if ((bool) (Node) node.PreComments)
          {
            if ((bool) (Node) this.PreComments)
              this.PreComments.AddRange((IEnumerable<Node>) node.PreComments);
            else
              this.PreComments = node.PreComments;
          }
          if ((bool) (Node) node.PostComments)
          {
            if ((bool) (Node) this.PostComments)
              this.PostComments.AddRange((IEnumerable<Node>) node.PostComments);
            else
              this.PostComments = node.PostComments;
          }
          this.IsReference = node.IsReference;
        }
      }
      return (T) this;
    }

    public virtual Node Clone() => this.CloneCore().ReducedFrom<Node>(this);

    protected abstract Node CloneCore();

    public virtual void AppendCSS(Env env) => this.Evaluate(env).AppendCSS(env);

    public virtual string ToCSS(Env env) => env.Output.Push().Append(this).Pop().ToString();

    public virtual Node Evaluate(Env env) => this;

    public virtual bool IgnoreOutput() => this is RegexMatchResult || this is CharMatchResult;

    public virtual void Accept(IVisitor visitor)
    {
    }

    public T VisitAndReplace<T>(T nodeToVisit, IVisitor visitor) where T : Node => this.VisitAndReplace<T>(nodeToVisit, visitor, false);

    public T VisitAndReplace<T>(T nodeToVisit, IVisitor visitor, bool allowNull) where T : Node
    {
      if ((object) nodeToVisit == null)
        return default (T);
      Node node = visitor.Visit((Node) nodeToVisit);
      if (node is T obj || allowNull && node == null)
        return default(T); //
      throw new Exception("Not allowed null for node of type " + typeof (T).ToString());
    }
  }
}
