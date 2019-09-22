using System.Collections.Generic;

namespace TddXt.SimpleNlp
{
  public class TokensUnderPreparation
  {
    private TextToken[] _tokens;

    public TokensUnderPreparation(TextToken textToken)
    {
      _tokens = new [] {textToken};
    }

    public void PartitionBasedOn(EntityForm entityForm)
    {
      var newTokens = new List<TextToken>();
      foreach (var token in _tokens)
      {
        var strings = entityForm.Tokenize(token);
        newTokens.AddRange(strings);
      }

      _tokens = newTokens.ToArray();
    }

    public static TokensUnderPreparation CreateInitial(string text)
    {
      return new TokensUnderPreparation(TextToken.NotMatched(text));
    }

    public IEnumerable<RecognizedEntity> TranslateToEntitiesUsing(IEnumerable<EntitySpecification> entitySpecifications)
    {
      var recognizedEntities = new List<RecognizedEntity>();
      foreach (var token in _tokens)
      {
        foreach (var entitySpecification in entitySpecifications)
        {
          entitySpecification.TryToMatch(token, recognizedEntities);
        }
      }

      return recognizedEntities;
    }
  }
}