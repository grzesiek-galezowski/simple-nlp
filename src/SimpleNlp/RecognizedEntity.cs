using System.Collections.Generic;
using Value;

namespace TddXt.SimpleNlp
{
  public class RecognizedEntity : ValueType<RecognizedEntity>
  {

    public RecognizedEntity(EntityName entityName, string recognizedValue)
    {
      Entity = entityName;
      RecognizedValue = recognizedValue;
    }

    public string RecognizedValue { get; }
    public EntityName Entity { get; }

    public static RecognizedEntity Value(EntityName name, string recognizedValue)
    {
      return new RecognizedEntity(name, recognizedValue);
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return Entity;
      yield return RecognizedValue;
    }

    public override string ToString()
    {
      return $"{nameof(Entity)}: {Entity}, {nameof(RecognizedValue)}: {RecognizedValue}";
    }
  }
}