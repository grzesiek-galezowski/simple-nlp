using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Value;

namespace TddXt.SimpleNlp
{
  public sealed class TextToken : ValueType<TextToken>
  {
    public static TextToken NotMatched(string text)
    {
      return new TextToken(text, false);
    }

    public static TextToken Matched(string text)
    {
      return new TextToken(text, true);
    }

    private readonly string _text;
    private readonly bool _isMatched;

    private TextToken(string text, bool isMatched)
    {
      _text = text;
      _isMatched = isMatched;
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return _text;
      yield return _isMatched;
    }

    public IEnumerable<TextToken> Split(string value, string forbiddenDelimiter)
    {
      if(_isMatched)
      {
        return new[] { this };
      }
      else
      {
        var subTokens = Regex.Split(_text, $"(?:{forbiddenDelimiter}|^)({value})(?:{forbiddenDelimiter}|$)",
          RegexOptions.IgnoreCase);
        return subTokens.Select(t => new TextToken(t, t.Equals(value, StringComparison.InvariantCultureIgnoreCase)));
      }
    }

    public bool Matches(string value)
    {
      return value.Equals(_text, StringComparison.InvariantCultureIgnoreCase);
    }

    public override string ToString()
    {
      return $"{nameof(_text)}: {_text}, {nameof(_isMatched)}: {_isMatched}";
    }
  }
}