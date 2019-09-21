using System.Collections.Generic;
using Value;

namespace TddXt.SimpleNlp
{
  public class RecognizedEntity : ValueType<RecognizedEntity>
  {

    public RecognizedEntity(EntityName entityName, EntityForm recognizedValue, EntityForm canonicalForm)
    {
      Entity = entityName;
      RecognizedValue = recognizedValue;
      CanonicalForm = canonicalForm;
    }

    public EntityForm RecognizedValue { get; }
    public EntityForm CanonicalForm { get; }
    public EntityName Entity { get; }

    public static RecognizedEntity Value(EntityName name, EntityForm recognizedValue, EntityForm canonicalForm)
    {
      return new RecognizedEntity(name, recognizedValue, canonicalForm);
    }

    public static RecognizedEntity ByCanonicalForm(EntityName entityName, EntityForm canonicalForm)
    {
      return new RecognizedEntity(entityName, canonicalForm, canonicalForm);
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return Entity;
      yield return RecognizedValue;
      yield return CanonicalForm;
    }

    public override string ToString()
    {
      return $"{nameof(Entity)}: {Entity}, {nameof(RecognizedValue)}: {RecognizedValue}";
    }


  }
}