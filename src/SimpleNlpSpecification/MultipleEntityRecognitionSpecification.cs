using FluentAssertions;
using TddXt.SimpleNlp;
using Xunit;

namespace TddXt.SimpleNlpSpecification
{
  public class MultipleEntityRecognitionSpecification
  {
    [Fact]
    public void ShouldBeAbleToRecognizeTheSameEntityMultipleTimes()
    {
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");

      var result = model.Recognize("driver license driver license");

      result.Entities.Should().Equal(new[]
      {
        RecognizedEntity.ByCanonicalForm(EntityName.Value("DRIVER_LICENSE"), EntityForm.Value("driver license")),
        RecognizedEntity.ByCanonicalForm(EntityName.Value("DRIVER_LICENSE"), EntityForm.Value("driver license")),
      });
    }

    [Fact]
    public void ShouldBeAbleToRecognizeTheSameEntityMultipleTimesWithSeveralPatterns()
    {
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");
      model.AddEntity("DRIVER_LICENSE", "driver's license");

      var result = model.Recognize("driver license driver's license");

      result.Entities.Should().Equal(new[]
      {
        RecognizedEntity.ByCanonicalForm(EntityName.Value("DRIVER_LICENSE"), EntityForm.Value("driver license")),
        RecognizedEntity.ByCanonicalForm(EntityName.Value("DRIVER_LICENSE"), EntityForm.Value("driver's license")),
      });
    }

    [Fact]
    public void ShouldBeAbleToRecognizeTheDifferentEntities()
    {
      var model = new RecognitionModel();
      model.AddEntity("DRIVER_LICENSE", "driver license");
      model.AddEntity("LICENSE_PLATE", "license plate");

      var result = model.Recognize("driver license license plate driver license");

      result.Entities.Should().Equal(new[]
      {
        RecognizedEntity.ByCanonicalForm(EntityName.Value("DRIVER_LICENSE"), EntityForm.Value("driver license")),
        RecognizedEntity.ByCanonicalForm(EntityName.Value("LICENSE_PLATE") ,  EntityForm.Value("license plate")),
        RecognizedEntity.ByCanonicalForm(EntityName.Value("DRIVER_LICENSE"), EntityForm.Value("driver license")),
      });
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

      result.Entities.Should().Equal(
        RecognizedEntity.ByCanonicalForm(EntityName.Value("DRIVER_LICENSE"), EntityForm.Value("driver license")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value("state"), EntityForm.Value("New York")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value("digit"), EntityForm.Value("1")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value("digit"), EntityForm.Value("2")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value("nato"),  EntityForm.Value("alpha")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value("nato"),  EntityForm.Value("bravo")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value("digit"), EntityForm.Value("5")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value("digit"), EntityForm.Value("6")));
    }

  }
}
