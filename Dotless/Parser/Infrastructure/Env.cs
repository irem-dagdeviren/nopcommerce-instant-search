// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Env
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Loggers;
using Nop.Plugin.InstantSearch.Dotless.Parser.Functions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Plugins;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure
{
  public class Env
  {
    private Dictionary<string, Type> _functionTypes;
    private readonly List<IPlugin> _plugins;
    private readonly List<Extender> _extensions;
    private static readonly Dictionary<string, Type> CoreFunctions = Env.GetCoreFunctions();

    public Stack<Ruleset> Frames { get; protected set; }

    public bool Compress { get; set; }

    public bool Debug { get; set; }

    public Node Rule { get; set; }

    public ILogger Logger { get; set; }

    public Output Output { get; private set; }

    public Stack<Media> MediaPath { get; private set; }

    public List<Media> MediaBlocks { get; private set; }

    [Obsolete("The Variable Redefines feature has been removed to align with less.js")]
    public bool DisableVariableRedefines { get; set; }

    [Obsolete("The Color Compression feature has been removed to align with less.js")]
    public bool DisableColorCompression { get; set; }

    public bool KeepFirstSpecialComment { get; set; }

    public bool IsFirstSpecialCommentOutput { get; set; }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Parser Parser { get; set; }

    public Env()
      : this(new Nop.Plugin.InstantSearch.Dotless.Parser.Parser())
    {
    }

    public Env(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser)
      : this(parser, (Stack<Ruleset>) null, (Dictionary<string, Type>) null)
    {
    }

    protected Env(Nop.Plugin.InstantSearch.Dotless.Parser.Parser parser, Stack<Ruleset> frames, Dictionary<string, Type> functions)
      : this(frames, functions)
    {
      this.Parser = parser;
    }

    protected Env(Stack<Ruleset> frames, Dictionary<string, Type> functions)
    {
      this.Frames = frames ?? new Stack<Ruleset>();
      this.Output = new Output(this);
      this.MediaPath = new Stack<Media>();
      this.MediaBlocks = new List<Media>();
      this.Logger = (ILogger) new NullLogger(LogLevel.Info);
      this._plugins = new List<IPlugin>();
      this._functionTypes = functions ?? new Dictionary<string, Type>();
      this._extensions = new List<Extender>();
      this.ExtendMediaScope = new Stack<Media>();
      if (this._functionTypes.Count != 0)
        return;
      this.AddCoreFunctions();
    }

    [Obsolete("Argument is ignored as of version 1.4.3.0. Use the parameterless overload of CreateChildEnv instead.", false)]
    public virtual Env CreateChildEnv(Stack<Ruleset> ruleset) => this.CreateChildEnv();

    public virtual Env CreateChildEnv() => new Env((Stack<Ruleset>) null, this._functionTypes)
    {
      Parser = this.Parser,
      Parent = this,
      Debug = this.Debug,
      Compress = this.Compress,
      DisableVariableRedefines = this.DisableVariableRedefines
    };

    private Env Parent { get; set; }

    public virtual Env CreateVariableEvaluationEnv(string variableName)
    {
      Env childEnv = this.CreateChildEnv();
      childEnv.EvaluatingVariable = variableName;
      return childEnv;
    }

    private string EvaluatingVariable { get; set; }

    public bool IsEvaluatingVariable(string variableName)
    {
      if (string.Equals(variableName, this.EvaluatingVariable, StringComparison.InvariantCulture))
        return true;
      return this.Parent != null && this.Parent.IsEvaluatingVariable(variableName);
    }

    public virtual Env CreateChildEnvWithClosure(Closure closure)
    {
      Env childEnv = this.CreateChildEnv();
      childEnv.Rule = this.Rule;
      childEnv.ClosureEnvironment = new Env(new Stack<Ruleset>((IEnumerable<Ruleset>) closure.Context), this._functionTypes);
      return childEnv;
    }

    private Env ClosureEnvironment { get; set; }

    public void AddPlugin(IPlugin plugin)
    {
      if (plugin == null)
        throw new ArgumentNullException(nameof (plugin));
      this._plugins.Add(plugin);
      if (!(plugin is IFunctionPlugin functionPlugin))
        return;
      foreach (KeyValuePair<string, Type> function in functionPlugin.GetFunctions())
      {
        string lowerInvariant = function.Key.ToLowerInvariant();
        if (this._functionTypes.ContainsKey(lowerInvariant))
          throw new InvalidOperationException(string.Format("Function '{0}' already exists in environment but is added by plugin {1}", (object) lowerInvariant, (object) plugin.GetName()));
        this.AddFunction(lowerInvariant, function.Value);
      }
    }

    public IEnumerable<IVisitorPlugin> VisitorPlugins => this._plugins.OfType<IVisitorPlugin>();

    public Stack<Media> ExtendMediaScope { get; set; }

    public bool IsCommentSilent(bool isValidCss, bool isCssHack, bool isSpecialComment)
    {
      if (!isValidCss)
        return true;
      if (isCssHack)
        return false;
      if (((!this.Compress || !this.KeepFirstSpecialComment ? 0 : (!this.IsFirstSpecialCommentOutput ? 1 : 0)) & (isSpecialComment ? 1 : 0)) == 0)
        return this.Compress;
      this.IsFirstSpecialCommentOutput = true;
      return false;
    }

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule FindVariable(string name) => this.FindVariable(name, this.Rule);

    public Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule FindVariable(string name, Node rule)
    {
      foreach (Ruleset frame in this.Frames)
      {
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule variable = frame.Variable(name, (Node) null);
        if ((bool) (Node) variable)
          return variable;
      }
      Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule variable1 = (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule) null;
      if (this.Parent != null)
        variable1 = this.Parent.FindVariable(name, rule);
      if (variable1 != null)
        return variable1;
      return this.ClosureEnvironment != null ? this.ClosureEnvironment.FindVariable(name, rule) : (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Rule) null;
    }

    [Obsolete("This method will be removed in a future release.", false)]
    public IEnumerable<Closure> FindRulesets<TRuleset>(Selector selector) where TRuleset : Ruleset => this.FindRulesets(selector).Where<Closure>((Func<Closure, bool>) (c => c.Ruleset is TRuleset));

    public IEnumerable<Closure> FindRulesets(Selector selector)
    {
      List<Closure> list = this.Frames.Reverse<Ruleset>().SelectMany<Ruleset, Closure>((Func<Ruleset, IEnumerable<Closure>>) (frame => (IEnumerable<Closure>) frame.Find<Ruleset>(this, selector, (Ruleset) null))).Where<Closure>((Func<Closure, bool>) (matchedClosure =>
      {
        if (!this.Frames.Any<Ruleset>((Func<Ruleset, bool>) (frame => frame.IsEqualOrClonedFrom(matchedClosure.Ruleset))))
          return true;
        return matchedClosure.Ruleset is MixinDefinition ruleset2 && ruleset2.Condition != null;
      })).ToList<Closure>();
      if (list.Any<Closure>())
        return (IEnumerable<Closure>) list;
      if (this.Parent != null)
      {
        IEnumerable<Closure> rulesets = this.Parent.FindRulesets(selector);
        if (rulesets != null)
          return rulesets;
      }
      return this.ClosureEnvironment != null ? this.ClosureEnvironment.FindRulesets(selector) : (IEnumerable<Closure>) null;
    }

    public void AddFunction(string name, Type type)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      this._functionTypes[name] = !(type == (Type) null) ? type : throw new ArgumentNullException(nameof (type));
    }

    public void AddFunctionsFromAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        throw new ArgumentNullException(nameof (assembly));
      this.AddFunctionsToRegistry((IEnumerable<KeyValuePair<string, Type>>) Env.GetFunctionsFromAssembly(assembly));
    }

    private void AddFunctionsToRegistry(IEnumerable<KeyValuePair<string, Type>> functions)
    {
      foreach (KeyValuePair<string, Type> function in functions)
        this.AddFunction(function.Key, function.Value);
    }

    private static Dictionary<string, Type> GetFunctionsFromAssembly(Assembly assembly)
    {
      Type functionType = typeof (Function);
      return ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => functionType.IsAssignableFrom(t) && t != functionType)).Where<Type>((Func<Type, bool>) (t => !t.IsAbstract)).SelectMany<Type, KeyValuePair<string, Type>>(new Func<Type, IEnumerable<KeyValuePair<string, Type>>>(Env.GetFunctionNames)).ToDictionary<KeyValuePair<string, Type>, string, Type>((Func<KeyValuePair<string, Type>, string>) (kvp => kvp.Key), (Func<KeyValuePair<string, Type>, Type>) (kvp => kvp.Value));
    }

    private static Dictionary<string, Type> GetCoreFunctions()
    {
      Dictionary<string, Type> functionsFromAssembly = Env.GetFunctionsFromAssembly(Assembly.GetExecutingAssembly());
      functionsFromAssembly["%"] = typeof (CFormatString);
      return functionsFromAssembly;
    }

    private void AddCoreFunctions() => this._functionTypes = new Dictionary<string, Type>((IDictionary<string, Type>) Env.CoreFunctions);

    public virtual Function GetFunction(string name)
    {
      Function function = (Function) null;
      name = name.ToLowerInvariant();
      if (this._functionTypes.ContainsKey(name))
      {
        function = (Function) Activator.CreateInstance(this._functionTypes[name]);
        function.Logger = this.Logger;
      }
      return function;
    }

    private static IEnumerable<KeyValuePair<string, Type>> GetFunctionNames(Type t)
    {
      string name = t.Name;
      if (name.EndsWith("function", StringComparison.InvariantCultureIgnoreCase))
        name = name.Substring(0, name.Length - 8);
      name = Regex.Replace(name, "\\B[A-Z]", "-$0");
      name = name.ToLowerInvariant();
      yield return new KeyValuePair<string, Type>(name, t);
      if (name.Contains("-"))
        yield return new KeyValuePair<string, Type>(name.Replace("-", ""), t);
    }

    public void AddExtension(Selector selector, Extend extends, Env env)
    {
      foreach (Selector selector1 in extends.Exact)
      {
        Selector extending = selector1;
        Extender extender;
        if ((ExactExtender) (extender = (Extender) this._extensions.OfType<ExactExtender>().FirstOrDefault<ExactExtender>((Func<ExactExtender, bool>) (e => e.BaseSelector.ToString().Trim() == extending.ToString().Trim()))) == null)
        {
          extender = (Extender) new ExactExtender(extending, extends);
          this._extensions.Add(extender);
        }
        extender.AddExtension(selector, env);
      }
      foreach (Selector selector2 in extends.Partial)
      {
        Selector extending = selector2;
        Extender extender;
        if ((PartialExtender) (extender = (Extender) this._extensions.OfType<PartialExtender>().FirstOrDefault<PartialExtender>((Func<PartialExtender, bool>) (e => e.BaseSelector.ToString().Trim() == extending.ToString().Trim()))) == null)
        {
          extender = (Extender) new PartialExtender(extending, extends);
          this._extensions.Add(extender);
        }
        extender.AddExtension(selector, env);
      }
      if (this.Parent == null)
        return;
      this.Parent.AddExtension(selector, extends, env);
    }

    public void RegisterExtensionsFrom(Env child) => this._extensions.AddRange((IEnumerable<Extender>) child._extensions);

    public IEnumerable<Extender> FindUnmatchedExtensions() => this._extensions.Where<Extender>((Func<Extender, bool>) (e => !e.IsMatched));

    public ExactExtender FindExactExtension(string selection)
    {
      if (this.ExtendMediaScope.Any<Media>())
      {
        ExactExtender exactExtension = this.ExtendMediaScope.Select<Media, ExactExtender>((Func<Media, ExactExtender>) (media => media.FindExactExtension(selection))).FirstOrDefault<ExactExtender>((Func<ExactExtender, bool>) (result => result != null));
        if (exactExtension != null)
          return exactExtension;
      }
      return this._extensions.OfType<ExactExtender>().FirstOrDefault<ExactExtender>((Func<ExactExtender, bool>) (e => e.BaseSelector.ToString().Trim() == selection));
    }

    public PartialExtender[] FindPartialExtensions(Context selection)
    {
      if (this.ExtendMediaScope.Any<Media>())
      {
        PartialExtender[] partialExtensions = this.ExtendMediaScope.Select<Media, PartialExtender[]>((Func<Media, PartialExtender[]>) (media => media.FindPartialExtensions(selection))).FirstOrDefault<PartialExtender[]>((Func<PartialExtender[], bool>) (result => ((IEnumerable<PartialExtender>) result).Any<PartialExtender>()));
        if (partialExtensions != null)
          return partialExtensions;
      }
      return this._extensions.OfType<PartialExtender>().WhereExtenderMatches(selection).ToArray<PartialExtender>();
    }

    [Obsolete("This method doesn't return the correct results. Use FindPartialExtensions(Context) instead.", false)]
    public PartialExtender[] FindPartialExtensions(string selection)
    {
      if (this.ExtendMediaScope.Any<Media>())
      {
        PartialExtender[] partialExtensions = this.ExtendMediaScope.Select<Media, PartialExtender[]>((Func<Media, PartialExtender[]>) (media => media.FindPartialExtensions(selection))).FirstOrDefault<PartialExtender[]>((Func<PartialExtender[], bool>) (result => ((IEnumerable<PartialExtender>) result).Any<PartialExtender>()));
        if (partialExtensions != null)
          return partialExtensions;
      }
      return this._extensions.OfType<PartialExtender>().Where<PartialExtender>((Func<PartialExtender, bool>) (e => selection.Contains(e.BaseSelector.ToString().Trim()))).ToArray<PartialExtender>();
    }

    public override string ToString() => this.Frames.Select<Ruleset, string>((Func<Ruleset, string>) (f => f.ToString())).JoinStrings(" <- ");
  }
}
