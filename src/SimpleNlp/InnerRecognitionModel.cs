using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace TddXt.SimpleNlp
{
  public class InnerRecognitionModel
  {
    private const string IntentNone = "None";
    private readonly List<EntitySpecification> _entitySpecifications = new List<EntitySpecification>();
    private readonly List<IntentSpecification> _intentSpecifications = new List<IntentSpecification>();

    public void AddEntity(EntityName entityName, EntityForm canonicalForm)
    {
      AddEntity(entityName, canonicalForm, new EntityForm[]{ });
    }

    public void AddEntity(EntityName entityName, EntityForm canonicalForm, EntityForm[] otherForms)
    {
      foreach (var entitySpecification in _entitySpecifications)
      {
        entitySpecification.AssertDoesNotConflictWith(entityName, canonicalForm, otherForms);
      }

      _entitySpecifications.Add(new EntitySpecification(entityName, canonicalForm, otherForms));
    }

    public void AddIntent(string intentName, IEnumerable<EntityName> entityNames)
    {
      foreach (var intentSpecification in _intentSpecifications)
      {
        intentSpecification.AssertDoesNotConflictWith(intentName, entityNames);
      }

      _intentSpecifications.Add(new IntentSpecification(intentName, entityNames));
    }

    public RecognitionResult Recognize(string text)
    {
      text = Normalize(text);
      var tokensUnderPreparation = Tokenize(text);
      var recognizedEntities = tokensUnderPreparation.TranslateToEntitiesUsing(_entitySpecifications);

      return new RecognitionResult(recognizedEntities.ToImmutableList(), TopIntent(recognizedEntities));
    }

    private string TopIntent(IEnumerable<RecognizedEntity> recognizedEntities)
    {
      foreach (var intentSpec in _intentSpecifications)
      {
        if (intentSpec.IsMatchedBy(recognizedEntities))
        {
          return intentSpec.IntentName;
        }
      }

      return IntentNone;
    }

    private string Normalize(string text)
    {
      text = SeparateDigits(text);
      text = EliminateMultipleSpaces(text);
      return text;
    }

    private string SeparateDigits(string text)
    {
      return Regex.Replace(text, "([0-9])", m => " " + m.Groups[0] + " ");
    }

    private static string EliminateMultipleSpaces(string text)
    {
      return new Regex(@"\s+").Replace(text, " ");
    }

    private TokensUnderPreparation Tokenize(string text)
    {
      var tokensUnderPreparation = TokensUnderPreparation.CreateInitial(text);

      foreach (var entitySpecification in _entitySpecifications)
      {
        entitySpecification.ApplyTo(tokensUnderPreparation);
      }

      return tokensUnderPreparation;
    }

  }
}