using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Value;

namespace TddXt.SimpleNlp
{
  public class EntityForm : ValueType<EntityForm>
  {
    private readonly string _value;

    private EntityForm(string value)
    {
      _value = value;
    }

    public static EntityForm Value(string pattern)
    {
      return new EntityForm(pattern);
    }

    public override string ToString()
    {
      return _value;
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return _value.ToLowerInvariant();
    }
    public bool IsMatchedBy(string token)
    {
      return _value.Equals(token, StringComparison.InvariantCultureIgnoreCase);
    }

    public IEnumerable<string> Tokenize(string text)
    {
      var forbiddenDelimiter = "[^0-9a-zA-Z'`]";
      return Regex.Split(text, $"(?:{forbiddenDelimiter}|^)({this})(?:{forbiddenDelimiter}|$)", RegexOptions.IgnoreCase);
    }
  }
}