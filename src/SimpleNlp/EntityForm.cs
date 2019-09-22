using System.Collections.Generic;
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

    public bool IsMatchedBy(TextToken token)
    {
      return token.Matches(_value);
    }

    public IEnumerable<TextToken> Tokenize(TextToken token)
    {
      return token.Split(_value, "[^0-9a-zA-Z'`]");
    }
  }
}