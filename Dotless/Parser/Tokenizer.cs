// Decompiled with JetBrains decompiler
// Type: Nop.Plugin.InstantSearch.Dotless.Parser.Tokenizer
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Nop.Plugin.InstantSearch.Dotless.Parser
{
  [DebuggerDisplay("{Remaining}")]
  public class Tokenizer
  {
    private string _input;
    private List<Tokenizer.Chunk> _chunks;
    private int _i;
    private int _j;
    private int _current;
    private int _lastCommentStart = -1;
    private int _lastCommentEnd = -1;
    private int _inputLength;
    private readonly string _commentRegEx = "(//[^\\n]*|(/\\*(.|[\\r\\n])*?\\*/))";
    private readonly string _quotedRegEx = "(\"((?:[^\"\\\\\\r\\n]|\\\\.)*)\"|'((?:[^'\\\\\\r\\n]|\\\\.)*)')";
    private string _fileName;
    private IDictionary<string, Regex> regexCache = (IDictionary<string, Regex>) new Dictionary<string, Regex>();

    public int Optimization { get; set; }

    public Tokenizer(int optimization) => this.Optimization = optimization;

    public void SetupInput(string input, string fileName)
    {
      this._fileName = fileName;
      this._i = this._j = this._current = 0;
      this._chunks = new List<Tokenizer.Chunk>();
      this._input = input.Replace("\r\n", "\n");
      this._inputLength = this._input.Length;
      if (this.Optimization == 0)
      {
        this._chunks.Add(new Tokenizer.Chunk(this._input));
      }
      else
      {
        Regex regex1 = new Regex("\\G(@\\{[a-zA-Z0-9_-]+\\}|[^\\\"'{}/\\\\\\(\\)]+)");
        Regex regex2 = this.GetRegex(this._commentRegEx, RegexOptions.None);
        Regex regex3 = this.GetRegex(this._quotedRegEx, RegexOptions.None);
        int num1 = 0;
        int index = 0;
        bool flag = false;
        int num2 = 0;
        while (num2 < this._inputLength)
        {
          System.Text.RegularExpressions.Match match1 = regex1.Match(this._input, num2);
          if (match1.Success)
          {
            Tokenizer.Chunk.Append(match1.Value, this._chunks);
            num2 += match1.Length;
          }
          else
          {
            char c = this._input[num2];
            if (num2 < this._inputLength - 1 && c == '/')
            {
              char ch = this._input[num2 + 1];
              if (!flag && ch == '/' || ch == '*')
              {
                System.Text.RegularExpressions.Match match2 = regex2.Match(this._input, num2);
                if (!match2.Success)
                  throw new ParsingException("Missing closing comment", this.GetNodeLocation(num2));
                num2 += match2.Length;
                this._chunks.Add(new Tokenizer.Chunk(match2.Value, Tokenizer.ChunkType.Comment));
                continue;
              }
            }
            if (c == '"' || c == '\'')
            {
              System.Text.RegularExpressions.Match match3 = regex3.Match(this._input, num2);
              if (!match3.Success)
                throw new ParsingException(string.Format("Missing closing quote ({0})", (object) c), this.GetNodeLocation(num2));
              num2 += match3.Length;
              this._chunks.Add(new Tokenizer.Chunk(match3.Value, Tokenizer.ChunkType.QuotedString));
            }
            else
            {
              if (!flag && c == '{')
              {
                ++num1;
                index = num2;
              }
              else if (!flag && c == '}')
              {
                --num1;
                if (num1 < 0)
                  throw new ParsingException("Unexpected '}'", this.GetNodeLocation(num2));
                Tokenizer.Chunk.Append(c, this._chunks, true);
                ++num2;
                continue;
              }
              switch (c)
              {
                case '(':
                  flag = true;
                  break;
                case ')':
                  flag = false;
                  break;
              }
              Tokenizer.Chunk.Append(c, this._chunks);
              ++num2;
            }
          }
        }
        if (num1 > 0)
          throw new ParsingException("Missing closing '}'", this.GetNodeLocation(index));
        this._input = Tokenizer.Chunk.CommitAll(this._chunks);
        this._inputLength = this._input.Length;
      }
      this.Advance(0);
    }

    public string GetComment()
    {
      if (this._i == this._inputLength)
        return (string) null;
      int i = this._i;
      string comment;
      int num;
      if (this.Optimization == 0)
      {
        if (this.CurrentChar != '/')
          return (string) null;
        RegexMatchResult regexMatchResult = this.Match(this._commentRegEx);
        if (regexMatchResult == null)
          return (string) null;
        comment = regexMatchResult.Value;
        num = i + regexMatchResult.Value.Length;
      }
      else
      {
        if (this._chunks[this._j].Type != Tokenizer.ChunkType.Comment)
          return (string) null;
        comment = this._chunks[this._j].Value;
        num = this._i + this._chunks[this._j].Value.Length;
        this.Advance(this._chunks[this._j].Value.Length);
      }
      if (this._lastCommentEnd != i)
        this._lastCommentStart = i;
      this._lastCommentEnd = num;
      return comment;
    }

    public string GetQuotedString()
    {
      if (this._i == this._inputLength)
        return (string) null;
      if (this.Optimization == 0)
        return this.CurrentChar != '"' && this.CurrentChar != '\'' ? (string) null : this.Match(this._quotedRegEx).Value;
      if (this._chunks[this._j].Type != Tokenizer.ChunkType.QuotedString)
        return (string) null;
      string quotedString = this._chunks[this._j].Value;
      this.Advance(this._chunks[this._j].Value.Length);
      return quotedString;
    }

    public string MatchString(char tok) => this.Match(tok)?.Value;

    public string MatchString(string tok) => this.Match(tok)?.Value;

    public CharMatchResult Match(char tok)
    {
      if (this._i == this._inputLength || this._chunks[this._j].Type != Tokenizer.ChunkType.Text)
        return (CharMatchResult) null;
      if ((int) this._input[this._i] != (int) tok)
        return (CharMatchResult) null;
      int i = this._i;
      this.Advance(1);
      CharMatchResult charMatchResult = new CharMatchResult(tok);
      charMatchResult.Location = this.GetNodeLocation(i);
      return charMatchResult;
    }

    public RegexMatchResult Match(string tok) => this.Match(tok, false);

    public RegexMatchResult Match(string tok, bool caseInsensitive)
    {
      if (this._i == this._inputLength || this._chunks[this._j].Type != Tokenizer.ChunkType.Text)
        return (RegexMatchResult) null;
      RegexOptions options = RegexOptions.None;
      if (caseInsensitive)
        options |= RegexOptions.IgnoreCase;
      System.Text.RegularExpressions.Match match = this.GetRegex(tok, options).Match(this._chunks[this._j].Value, this._i - this._current);
      if (!match.Success)
        return (RegexMatchResult) null;
      int i = this._i;
      this.Advance(match.Length);
      RegexMatchResult regexMatchResult = new RegexMatchResult(match);
      regexMatchResult.Location = this.GetNodeLocation(i);
      return regexMatchResult;
    }

    public RegexMatchResult MatchAny(string tok)
    {
      if (this._i == this._inputLength)
        return (RegexMatchResult) null;
      System.Text.RegularExpressions.Match match = this.GetRegex(tok, RegexOptions.None).Match(this._input, this._i);
      if (!match.Success)
        return (RegexMatchResult) null;
      this.Advance(match.Length);
      if (this._i > this._current && this._i < this._current + this._chunks[this._j].Value.Length && this._chunks[this._j].Type == Tokenizer.ChunkType.Comment && this._chunks[this._j].Value.StartsWith("//"))
        this._chunks[this._j].Type = Tokenizer.ChunkType.Text;
      return new RegexMatchResult(match);
    }

    public void Advance(int length)
    {
      if (this._i == this._inputLength)
        return;
      this._i += length;
      int num = this._current + this._chunks[this._j].Value.Length;
      while (this._i != this._inputLength)
      {
        if (this._i >= num)
        {
          if (this._j >= this._chunks.Count - 1)
            break;
          this._current = num;
          num += this._chunks[++this._j].Value.Length;
        }
        else
        {
          if (!char.IsWhiteSpace(this._input[this._i]))
            break;
          ++this._i;
        }
      }
    }

    public bool Peek(char tok) => this._i != this._inputLength && (int) this._input[this._i] == (int) tok;

    public bool Peek(string tok) => this.GetRegex(tok, RegexOptions.None).Match(this._input, this._i).Success;

    public bool PeekAfterComments(char tok)
    {
      Location location = this.Location;
      do
        ;
      while (this.GetComment() != null);
      int num = this.Peek(tok) ? 1 : 0;
      this.Location = location;
      return num != 0;
    }

    private Regex GetRegex(string pattern, RegexOptions options)
    {
      if (!this.regexCache.ContainsKey(pattern))
        this.regexCache.Add(pattern, new Regex("\\G" + pattern, options));
      return this.regexCache[pattern];
    }

    public char GetPreviousCharIgnoringComments()
    {
      if (this._i == 0)
        return char.MinValue;
      if (this._i != this._lastCommentEnd)
        return this.PreviousChar;
      int index = this._lastCommentStart - 1;
      return index < 0 ? char.MinValue : this._input[index];
    }

    public char PreviousChar => this._i != 0 ? this._input[this._i - 1] : char.MinValue;

    public char CurrentChar => this._i != this._inputLength ? this._input[this._i] : char.MinValue;

    public char NextChar => this._i + 1 != this._inputLength ? this._input[this._i + 1] : char.MinValue;

    public bool HasCompletedParsing() => this._i == this._inputLength;

    public Location Location
    {
      get => new Location()
      {
        Index = this._i,
        CurrentChunk = this._j,
        CurrentChunkIndex = this._current
      };
      set
      {
        this._i = value.Index;
        this._j = value.CurrentChunk;
        this._current = value.CurrentChunkIndex;
      }
    }

    public NodeLocation GetNodeLocation(int index) => new NodeLocation(index, this._input, this._fileName);

    public NodeLocation GetNodeLocation() => this.GetNodeLocation(this.Location.Index);

    private string Remaining => this._input.Substring(this._i);

    private enum ChunkType
    {
      Text,
      Comment,
      QuotedString,
    }

    private class Chunk
    {
      private StringBuilder _builder;
      private bool _final;

      public Chunk(string val)
      {
        this.Value = val;
        this.Type = Tokenizer.ChunkType.Text;
      }

      public Chunk(string val, Tokenizer.ChunkType type)
      {
        this.Value = val;
        this.Type = type;
      }

      public Chunk()
      {
        this._builder = new StringBuilder();
        this.Type = Tokenizer.ChunkType.Text;
      }

      public Tokenizer.ChunkType Type { get; set; }

      public string Value { get; set; }

      public void Append(string str) => this._builder.Append(str);

      public void Append(char c) => this._builder.Append(c);

      private static Tokenizer.Chunk ReadyForText(List<Tokenizer.Chunk> chunks)
      {
        Tokenizer.Chunk chunk = chunks.LastOrDefault<Tokenizer.Chunk>();
        if (chunk == null || chunk.Type != Tokenizer.ChunkType.Text || chunk._final)
        {
          chunk = new Tokenizer.Chunk();
          chunks.Add(chunk);
        }
        return chunk;
      }

      public static void Append(char c, List<Tokenizer.Chunk> chunks, bool final)
      {
        Tokenizer.Chunk chunk = Tokenizer.Chunk.ReadyForText(chunks);
        chunk.Append(c);
        chunk._final = final;
      }

      public static void Append(char c, List<Tokenizer.Chunk> chunks) => Tokenizer.Chunk.ReadyForText(chunks).Append(c);

      public static void Append(string s, List<Tokenizer.Chunk> chunks) => Tokenizer.Chunk.ReadyForText(chunks).Append(s);

      public static string CommitAll(List<Tokenizer.Chunk> chunks)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (Tokenizer.Chunk chunk in chunks)
        {
          if (chunk._builder != null)
          {
            string str = chunk._builder.ToString();
            chunk._builder = (StringBuilder) null;
            chunk.Value = str;
          }
          stringBuilder.Append(chunk.Value);
        }
        return stringBuilder.ToString();
      }
    }
  }
}
