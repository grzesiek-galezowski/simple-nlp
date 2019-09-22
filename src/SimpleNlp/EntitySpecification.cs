using System;
using System.Collections.Generic;
using System.Linq;

namespace TddXt.SimpleNlp
{
  public class EntitySpecification
  {
    private readonly EntityName _entityName;
    private readonly EntityForm _canonicalForm;
    private readonly IEnumerable<EntityForm> _allForms;

    public EntitySpecification(EntityName entityName, EntityForm canonicalForm, EntityForm[] synonyms)
    {
      _entityName = entityName;
      _canonicalForm = canonicalForm;
      _allForms = new[] {canonicalForm}.Concat(synonyms);
    }

    public void ApplyTo(TokensUnderPreparation tokensUnderPreparation)
    {
      foreach (var form in _allForms)
      {
        tokensUnderPreparation.PartitionBasedOn(form);
      }
    }

    public void TryToMatch(TextToken token, List<RecognizedEntity> recognizedEntities)
    {
      foreach (var synonym in _allForms)
      {
        if (synonym.IsMatchedBy(token))
        {
          recognizedEntities.Add(RecognizedEntity.Value(_entityName, synonym, _canonicalForm));
        }
      }
    }

    public void AssertDoesNotConflictWith(EntityName entityName, EntityForm value, EntityForm[] otherForms)
    {
      var externalPhrases = new[] {value}.Concat(otherForms);
      ;
      foreach (var internalPhrase in _allForms)
      {
        if(externalPhrases.Contains(internalPhrase))
        {
          throw new ConflictingEntityException(entityName, _entityName, internalPhrase);
        }
      }
    }
  }
}