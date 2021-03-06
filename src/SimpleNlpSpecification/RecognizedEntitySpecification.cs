﻿using FluentAssertions;
using TddXt.SimpleNlp;
using TddXt.XFluentAssertRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.SimpleNlpSpecification
{
  public class RecognizedEntitySpecification
  {
    [Fact]
    public void ShouldAllowAccessingDataItWasCreatedWith()
    {
      //GIVEN
      var entityName = Any.Instance<EntityName>();
      var recognizedValue = Any.Instance<EntityForm>();
      var canonicalForm = Any.Instance<EntityForm>();
      var recognizedEntity = RecognizedEntity.Value(entityName, recognizedValue, canonicalForm);
      
      //WHEN
      var retrievedEntityName = recognizedEntity.Entity;
      var retrievedRecognizedValue = recognizedEntity.RecognizedValue;
      var retrievedCanonicalForm = recognizedEntity.CanonicalForm;

      //THEN
      retrievedEntityName.Should().Be(entityName);
      retrievedRecognizedValue.Should().Be(recognizedValue);
      retrievedCanonicalForm.Should().Be(canonicalForm);
    }


    [Fact]
    public void ShouldIncludeItsDataInItsStringRepresentation()
    {
      //GIVEN
      var entityName = Any.Instance<EntityName>();
      var recognizedValue = Any.Instance<EntityForm>();
      var canonicalForm = Any.Instance<EntityForm>();
      var recognizedEntity = RecognizedEntity.Value(entityName, recognizedValue, canonicalForm);

      //WHEN
      var stringRepresentation = recognizedEntity.ToString();

      //THEN
      stringRepresentation.Should()
        .ContainAll(
          entityName.ToString(), 
          recognizedValue.ToString(), 
          canonicalForm.ToString());
    }

    [Fact]
    public void ShouldHaveValueSemantics()
    {
      typeof(RecognizedEntity).Should().HaveValueSemantics();
    }
  }
}
