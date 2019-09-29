using FluentAssertions;
using TddXt.SimpleNlp;

namespace TddXt.SimpleNlpSpecification
{
  public static class EntityExtensions
  {
    public static void ShouldContainOnly(this RecognitionResult result, string entityName, string entityValue)
    {
      ShouldContainOnly(result, entityName, entityValue, entityValue);
    }

    public static void ShouldContainOnly(this RecognitionResult result, string entityName, string entityValue,
      string canonicalForm)
    {
      result.Entities.Should().BeEquivalentTo(
        new[]
        {
          RecognizedEntity.Value(EntityName.Value(entityName), EntityForm.Value(entityValue), EntityForm.Value(canonicalForm))
        },
        options => options.WithStrictOrdering());
    }
  }
}