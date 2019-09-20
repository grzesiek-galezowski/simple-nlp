using FluentAssertions;
using TddXt.SimpleNlp;
using Xunit;

namespace SimpleNlpSpecification
{
  public class SingleEntityRecognitionSpecification
  {
    [Theory]
    [InlineData("DRIVER_LICENSE", "driver license", "driver license")]
    [InlineData("DRIVER_LICENSE", "driver license", "Driver License")] //does it ignore letter casing?
    [InlineData("DRIVER_LICENSE", "driver license", "driver  license")] // does it normalize spaces?
    [InlineData("LICENSE_PLATE", "license plate", "license plate")] //works for other values?
    [InlineData("LICENSE_PLATE", "license plate", "give me a license plate")] //works for prefixed values?
    [InlineData("LICENSE_PLATE", "license plate", "license plate, will ya?")] //works for suffixed values?
    public void ShouldBeAbleToRecognizeSingleEntity(string entityName, string entityValue, string text)
    {
      var model = new RecognitionModel();
      model.AddEntity(entityName, entityValue);

      var result = model.Recognize(text);

      result.ShouldContainOnly(entityName, entityValue);
    }

    [Fact]
    public void ShouldNotRecognizeEntitiesInCenterOfOtherWords()
    {
      //GIVEN
      var model = new RecognitionModel();
      model.AddEntity("PLATE", "plate");

      //WHEN
      var result1 = model.Recognize("plateau");
      var result2 = model.Recognize("unplated");

      //THEN
      result1.Entities.Should().BeEmpty();
      result2.Entities.Should().BeEmpty();
    }
    
    [Fact]
    public void ShouldRecognizeEntitiesWithSynonyms()
    {
      //GIVEN
      var model = new RecognitionModel();
      model.AddEntity("PLATE", "plate", new [] { "steel" });

      //WHEN
      var result1 = model.Recognize("steel");

      //THEN
      result1.ShouldContainOnly("PLATE", "plate");
    }

    [Fact]
    public void ShouldBeAbleToRecognizeSingleEntityWithDifferentPatterns()
    {
      //GIVEN
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");
      model.AddEntity("DRIVER_LICENSE", "driver licence");
      model.AddEntity("DRIVER_LICENSE", "driving license");

      //WHEN
      var result1 = model.Recognize("driver license");
      var result2 = model.Recognize("driver licence");
      var result3 = model.Recognize("driving license");

      //THEN
      result1.ShouldContainOnly("DRIVER_LICENSE", "driver license");
      result2.ShouldContainOnly("DRIVER_LICENSE", "driver licence");
      result3.ShouldContainOnly("DRIVER_LICENSE", "driving license");
    }

    [Fact]
    public void ShouldReturnEmptyListWhenNoEntitiesWereDefined()
    {
      var model = new RecognitionModel();

      var result1 = model.Recognize("driver license");

      result1.Entities.Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnEmptyListWhenNoTextDoesNotMatch()
    {
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");

      var result = model.Recognize("Trolololo");

      result.Entities.Should().BeEmpty();
    }

    [Fact]
    public void ShouldSeparateDigitsWhenDetectingEntities()
    {
      var model = new RecognitionModel();
      model.AddEntity("digit", "1");
      model.AddEntity("digit", "2");
      model.AddEntity("digit", "3");

      var result = model.Recognize("123");

      result.Entities.Should().BeEquivalentTo(new []
      {
        new RecognizedEntity(EntityName.Value("digit"), "1"), 
        new RecognizedEntity(EntityName.Value("digit"), "2"), 
        new RecognizedEntity(EntityName.Value("digit"), "3"), 
      }, options => options.WithStrictOrdering());
    }

  }
}
