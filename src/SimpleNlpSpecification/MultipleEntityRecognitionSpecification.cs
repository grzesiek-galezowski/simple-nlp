using FluentAssertions;
using TddXt.SimpleNlp;
using Xunit;

namespace SimpleNlpSpecification
{
  public class MultipleEntityRecognitionSpecification
  {
    //TODO number recognition (e.g. 1234 => one two three four?)
    
    [Fact]
    public void ShouldBeAbleToRecognizeTheSameEntityMultipleTimes()
    {
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");

      var result = model.Recognize("driver license driver license");

      result.Entities.Should().HaveCount(2);
      result.Entities.Should().BeEquivalentTo(new[]
      {
        RecognizedEntity.Value(EntityName.Value("DRIVER_LICENSE"), "driver license"),
        RecognizedEntity.Value(EntityName.Value("DRIVER_LICENSE"), "driver license"),
      }, options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldBeAbleToRecognizeTheSameEntityMultipleTimesWithSeveralPatterns()
    {
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");
      model.AddEntity("DRIVER_LICENSE", "driver's license");

      var result = model.Recognize("driver license driver's license");

      result.Entities.Should().HaveCount(2);
      result.Entities.Should().BeEquivalentTo(new[]
      {
        RecognizedEntity.Value(EntityName.Value("DRIVER_LICENSE"), "driver license"),
        RecognizedEntity.Value(EntityName.Value("DRIVER_LICENSE"), "driver's license"),
      }, options => options.WithStrictOrdering());
    }

    [Fact]
    public void ShouldBeAbleToRecognizeTheDifferentEntities()
    {
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");
      model.AddEntity("LICENSE_PLATE", "license plate");

      var result = model.Recognize("driver license license plate driver license");

      result.Entities.Should().BeEquivalentTo(new[]
      {
        RecognizedEntity.Value(EntityName.Value("DRIVER_LICENSE"), "driver license"),
        RecognizedEntity.Value(EntityName.Value("LICENSE_PLATE"), "license plate"),
        RecognizedEntity.Value(EntityName.Value("DRIVER_LICENSE"), "driver license"),
      }, options => options.WithStrictOrdering());
    }
    
    [Fact]
    public void ShouldBeAbleToRecognizeTheDifferentEntities2()
    {
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");
      model.AddEntity("nato", "alpha");
      model.AddEntity("nato", "bravo");
      model.AddEntity("nato", "charlie");
      model.AddEntity("digit", "1");
      model.AddEntity("digit", "2");
      model.AddEntity("digit", "3");
      model.AddEntity("digit", "4");
      model.AddEntity("digit", "5");
      model.AddEntity("digit", "6");
      model.AddEntity("digit", "7");
      model.AddEntity("digit", "8");
      model.AddEntity("digit", "9");
      model.AddEntity("digit", "0");
      model.AddEntity("state", "New York");

      var result = model.Recognize("Check driver license New York 1 2 alpha bravo 5 6");

      result.Entities.Should().HaveCount(8);
      result.Entities.Should().BeEquivalentTo(new[]
      {
        RecognizedEntity.Value(EntityName.Value("DRIVER_LICENSE"), "driver license"),
        RecognizedEntity.Value(EntityName.Value("state"), "New York"),
        RecognizedEntity.Value(EntityName.Value("digit"), "1"),
        RecognizedEntity.Value(EntityName.Value("digit"), "2"),
        RecognizedEntity.Value(EntityName.Value("nato"), "alpha"),
        RecognizedEntity.Value(EntityName.Value("nato"), "bravo"),
        RecognizedEntity.Value(EntityName.Value("digit"), "5"),
        RecognizedEntity.Value(EntityName.Value("digit"), "6"),
      }, options => options.WithStrictOrdering());
    }

  }
}
