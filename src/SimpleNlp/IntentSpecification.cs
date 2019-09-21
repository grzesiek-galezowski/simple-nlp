using System.Collections.Generic;
using System.Linq;

namespace TddXt.SimpleNlp
{
  public class IntentSpecification
  {
    private readonly IEnumerable<EntityName> _entityNames;
    public string IntentName { get; }

    public IntentSpecification(string intentName, IEnumerable<EntityName> entityNames)
    {
      _entityNames = entityNames;
      IntentName = intentName;
    }

    public bool IsMatchedBy(IEnumerable<RecognizedEntity> recognizedEntities)
    {
      return _entityNames.All(name => recognizedEntities.Select(e => e.Entity).Contains(name));
    }

    public void AssertDoesNotConflictWith(string intentName, IEnumerable<EntityName> entityNames)
    {
      if(_entityNames.All(entityNames.Contains))
      {
        throw new ConflictingIntentException(intentName, IntentName);
      }
    }
  }
}