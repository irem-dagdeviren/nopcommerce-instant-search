// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Output
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  public class Output
  {
    private Env Env { get; set; }

    private StringBuilder Builder { get; set; }

    private Stack<StringBuilder> BuilderStack { get; set; }

    public Output(Env env)
    {
      this.Env = env;
      this.BuilderStack = new Stack<StringBuilder>();
      this.Push();
    }

    public Output Push()
    {
      this.Builder = new StringBuilder();
      this.BuilderStack.Push(this.Builder);
      return this;
    }

    public StringBuilder Pop()
    {
      StringBuilder stringBuilder = this.BuilderStack.Count != 1 ? this.BuilderStack.Pop() : throw new InvalidOperationException();
      this.Builder = this.BuilderStack.Peek();
      return stringBuilder;
    }

    public void Reset(string s)
    {
      this.Builder = new StringBuilder(s);
      this.BuilderStack.Pop();
      this.BuilderStack.Push(this.Builder);
    }

    public Output PopAndAppend() => this.Append(this.Pop());

    public Output Append(Node node)
    {
      if (node != null)
      {
        if ((bool) (Node) node.PreComments)
          node.PreComments.AppendCSS(this.Env);
        node.AppendCSS(this.Env);
        if ((bool) (Node) node.PostComments)
          node.PostComments.AppendCSS(this.Env);
      }
      return this;
    }

    public Output Append(string s)
    {
      this.Builder.Append(s);
      return this;
    }

    public Output Append(char? s)
    {
      this.Builder.Append((object) s);
      return this;
    }

    public Output Append(StringBuilder sb)
    {
      this.Builder.Append((object) sb);
      return this;
    }

    public Output AppendMany<TNode>(IEnumerable<TNode> nodes) where TNode : Node => this.AppendMany<TNode>(nodes, (string) null);

    public Output AppendMany<TNode>(IEnumerable<TNode> nodes, string join) where TNode : Node => this.AppendMany<TNode>(nodes, (Action<TNode>) (n => this.Env.Output.Append((Node) n)), join);

    public Output AppendMany(IEnumerable<string> list, string join) => this.AppendMany<string>(list, (Action<string, StringBuilder>) ((item, sb) => sb.Append(item)), join);

    public Output AppendMany<T>(IEnumerable<T> list, Func<T, string> toString, string join) => this.AppendMany<T>(list, (Action<T, StringBuilder>) ((item, sb) => sb.Append(toString(item))), join);

    public Output AppendMany<T>(IEnumerable<T> list, Action<T> toString, string join) => this.AppendMany<T>(list, (Action<T, StringBuilder>) ((item, sb) => toString(item)), join);

    public Output AppendMany<T>(
      IEnumerable<T> list,
      Action<T, StringBuilder> toString,
      string join)
    {
      bool flag1 = true;
      bool flag2 = !string.IsNullOrEmpty(join);
      foreach (T obj in list)
      {
        if (!flag1 & flag2)
          this.Builder.Append(join);
        flag1 = false;
        toString(obj, this.Builder);
      }
      return this;
    }

    public Output AppendMany(IEnumerable<StringBuilder> buildersToAppend) => this.AppendMany(buildersToAppend, (string) null);

    public Output AppendMany(IEnumerable<StringBuilder> buildersToAppend, string join) => this.AppendMany<StringBuilder>(buildersToAppend, (Action<StringBuilder, StringBuilder>) ((b, output) => output.Append((object) b)), join);

    public Output AppendFormat(string format, params object[] values) => this.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, format, values);

    public Output AppendFormat(
      IFormatProvider formatProvider,
      string format,
      params object[] values)
    {
      this.Builder.AppendFormat(formatProvider, format, values);
      return this;
    }

    public Output Indent(int amount)
    {
      if (amount > 0)
      {
        string str = new string(' ', amount);
        this.Builder.Replace("\n", "\n" + str);
        this.Builder.Insert(0, str);
      }
      return this;
    }

    public Output Trim() => this.TrimLeft(new char?()).TrimRight(new char?());

    public Output TrimLeft(char? c)
    {
      int num = 0;
      int length = this.Builder.Length;
      if (length == 0)
        return this;
      while (num < length && (c.HasValue ? ((int) this.Builder[num] == (int) c.Value ? 1 : 0) : (char.IsWhiteSpace(this.Builder[num]) ? 1 : 0)) != 0)
        ++num;
      if (num > 0)
        this.Builder.Remove(0, num);
      return this;
    }

    public Output TrimRight(char? c)
    {
      int length1 = 0;
      int length2 = this.Builder.Length;
      if (length2 == 0)
        return this;
      while (length1 < length2 && (c.HasValue ? ((int) this.Builder[length2 - (length1 + 1)] == (int) c.Value ? 1 : 0) : (char.IsWhiteSpace(this.Builder[length2 - (length1 + 1)]) ? 1 : 0)) != 0)
        ++length1;
      if (length1 > 0)
        this.Builder.Remove(length2 - length1, length1);
      return this;
    }

    public override string ToString() => this.Builder.ToString();
  }
}
