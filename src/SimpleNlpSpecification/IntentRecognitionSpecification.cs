﻿using FluentAssertions;
using TddXt.SimpleNlp;
using Xunit;

namespace TddXt.SimpleNlpSpecification
{
  public class IntentRecognitionSpecification
  {
    [Fact]
    public void ShouldRecognizeNoIntentWhenNothingHasBeenConfigured()
    {
      //GIVEN
      var model = new RecognitionModel();
      
      //WHEN
      var recognitionResult = model.Recognize("Trolololo");

      //THEN
      recognitionResult.TopIntent.Should().Be("None");
    }

    [Fact]
    public void ShouldRecognizeNoIntentWhenTextDoesNotContainAnyOfIntentEntities()
    {
      //GIVEN
      var model = new RecognitionModel();
      
      model.AddEntity("NO", "no");
      model.AddIntent("INTENT_REFUSE", new [] { "NO" });

      //WHEN
      var recognitionResult = model.Recognize("Trolololo");

      //THEN
      recognitionResult.TopIntent.Should().Be("None");
    }

    [Theory]
    [InlineData("yes")]
    [InlineData("yes no")]
    [InlineData("no yes")] //does it work when the defining entity is not the first one?
    public void ShouldRecognizeIntentWithSingleEntity(string text)
    {
      //GIVEN
      var model = new RecognitionModel();
      
      model.AddEntity("YES", "yes");
      model.AddEntity("NO", "no");
      var entityNames = new [] { "YES"};
      model.AddIntent("INTENT_YES", entityNames);

      //WHEN
      var recognitionResult = model.Recognize(text);

      //THEN
      recognitionResult.TopIntent.Should().Be("INTENT_YES");
    }

    [Fact]
    public void ShouldRecognizeIntentWithMultipleEntitiesMatchingExactlyTheOnesRecognized()
    {
      //GIVEN
      var model = new RecognitionModel();
      
      model.AddEntity("YES", "yes");
      model.AddEntity("PLEASE", "please");
      model.AddIntent("INTENT_YES", new[] { "YES", "PLEASE"});

      //WHEN
      var recognitionResult = model.Recognize("yes, please");

      //THEN
      recognitionResult.TopIntent.Should().Be("INTENT_YES");
    }

    [Fact]
    public void ShouldAllowAddingWiderMatchingIntentAfterMoreNarrowMatching()
    {
      //GIVEN
      var model = new RecognitionModel();

      model.AddEntity("YES", "yes");
      model.AddEntity("PLEASE", "please");
      model.AddIntent("INTENT1", new[] { "YES", "PLEASE" });
      
      //WHEN - THEN
      model.Invoking(m => m.AddIntent("INTENT2", new[] { "YES" })).Should().NotThrow();
    }

    [Fact]
    public void ShouldThrowWhenAddingIntentWithTheSameEntities() 
    {
      //GIVEN
      var model = new RecognitionModel();

      model.AddEntity("YES", "yes");
      model.AddEntity("PLEASE", "please");
      model.AddIntent("INTENT1", new[] { "YES", "PLEASE" });

      //WHEN - THEN
      model.Invoking(m => m.AddIntent("INTENT2", new[] { "YES", "PLEASE" }))
        .Should().Throw<ConflictingIntentException>()
        .WithMessage("All the phrases matched by 'INTENT2' will be matched by earlier by 'INTENT1'");
    }
    
    [Fact]
    public void ShouldThrowWhenAddingIntentWithTheSupersetOfEntitiesComparedToOneAddedBefore() 
    {
      //GIVEN
      var model = new RecognitionModel();

      model.AddEntity("YES", "yes");
      model.AddEntity("PLEASE", "please");
      model.AddEntity("DONE", "done");
      model.AddIntent("INTENT1", new[] { "YES", "PLEASE" });

      //WHEN - THEN
      model.Invoking(m => m.AddIntent("INTENT2", new[] { "YES", "PLEASE", "DONE" }))
        .Should().Throw<ConflictingIntentException>()
        .WithMessage("All the phrases matched by 'INTENT2' will be matched by earlier by 'INTENT1'");
    }

    [Fact]
    public void ShouldBeAbleToMatchMultipleIntents()
    {
      //GIVEN
      var model = new RecognitionModel();
      
      model.AddEntity("YES", "yes");
      model.AddEntity("NO", "no");
      model.AddEntity("START_OVER", "start over");
      model.AddEntity("GAME_OVER", "game over");
      model.AddIntent("INTENT_YES", new[] { "YES" });
      model.AddIntent("INTENT_NO", new[] { "NO"});
      model.AddIntent("INTENT_START_OVER", new[] { "START_OVER"});
      model.AddIntent("INTENT_GAME_OVER", new[] { "GAME_OVER"});

      //WHEN
      var recognitionResult1 = model.Recognize("yes, please");
      var recognitionResult2 = model.Recognize("no, thank you");
      var recognitionResult3 = model.Recognize("game over, amigo");
      var recognitionResult4 = model.Recognize("shall we start over?");

      //THEN
      recognitionResult1.TopIntent.Should().Be("INTENT_YES");
      recognitionResult2.TopIntent.Should().Be("INTENT_NO");
      recognitionResult3.TopIntent.Should().Be("INTENT_GAME_OVER");
      recognitionResult4.TopIntent.Should().Be("INTENT_START_OVER");
    }
    
    [Fact]
    public void ShouldBeAbleToMatchMultipleIntentPatterns()
    {
      //GIVEN
      var model = new RecognitionModel();
      
      model.AddEntity("YES", "yes");
      model.AddEntity("NO", "no");
      model.AddEntity("START_OVER", "start over");
      model.AddEntity("GAME_OVER", "game over");
      model.AddIntent("INTENT", new[] { "YES" });
      model.AddIntent("INTENT", new[] { "NO"});
      model.AddIntent("INTENT", new[] { "START_OVER"});
      model.AddIntent("INTENT_GAME_OVER", new[] { "GAME_OVER"});

      //WHEN
      var recognitionResult1 = model.Recognize("yes, please");
      var recognitionResult2 = model.Recognize("no, thank you");
      var recognitionResult3 = model.Recognize("game over, amigo");
      var recognitionResult4 = model.Recognize("shall we start over?");

      //THEN
      recognitionResult1.TopIntent.Should().Be("INTENT");
      recognitionResult2.TopIntent.Should().Be("INTENT");
      recognitionResult3.TopIntent.Should().Be("INTENT_GAME_OVER");
      recognitionResult4.TopIntent.Should().Be("INTENT");
    }

    //TODO: regex patterns
    [Fact]
    public void ShouldBeAbleToMatchMultipleIntentPatterns2()
    {
      //GIVEN
      var model = new RecognitionModel();

      var borrowEntity = "Borrow";
      var returnEntity = "Return";
      var bookEntity =   "Book";
      var filmEntity =   "Film";

      model.AddEntity(borrowEntity, "borrow");
      model.AddEntity(returnEntity, "return", new []{"returning"});
      model.AddEntity(bookEntity, "book");
      model.AddEntity(filmEntity, "film", new []{ "movie" });

      model.AddIntent("BORROW_BOOK", new[] { borrowEntity, bookEntity });
      model.AddIntent("RETURN_BOOK", new[] { returnEntity, bookEntity });
      model.AddIntent("BORROW_FILM", new[] { borrowEntity, filmEntity });
      model.AddIntent("RETURN_FILM", new[] { returnEntity, filmEntity });

      //WHEN
      var recognitionResult1 = model.Recognize("I'd like to borrow a book");
      var recognitionResult2 = model.Recognize("I want to return this book");
      var recognitionResult3 = model.Recognize("Can I borrow a film?");
      var recognitionResult4 = model.Recognize("Here, I'm returning the movie");

      //THEN
      recognitionResult1.TopIntent.Should().Be("BORROW_BOOK");
      recognitionResult1.Entities.Should().Equal(new []
      {
        RecognizedEntity.ByCanonicalForm(EntityName.Value(borrowEntity), EntityForm.Value("borrow")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value(bookEntity), EntityForm.Value("book")), 
      });

      recognitionResult2.TopIntent.Should().Be("RETURN_BOOK");
      recognitionResult2.Entities.Should().Equal(new []
      {
        RecognizedEntity.ByCanonicalForm(EntityName.Value(returnEntity), EntityForm.Value("return")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value(bookEntity), EntityForm.Value("book")), 
      });
      recognitionResult3.TopIntent.Should().Be("BORROW_FILM");
      recognitionResult3.Entities.Should().Equal(new []
      {
        RecognizedEntity.ByCanonicalForm(EntityName.Value(borrowEntity), EntityForm.Value("borrow")), 
        RecognizedEntity.ByCanonicalForm(EntityName.Value(filmEntity), EntityForm.Value("film")), 
      });

      recognitionResult4.TopIntent.Should().Be("RETURN_FILM");
      recognitionResult4.Entities.Should().Equal(
        RecognizedEntity.Value(
          EntityName.Value(returnEntity), 
          EntityForm.Value("returning"), 
          EntityForm.Value("return")), 
        RecognizedEntity.Value(
          EntityName.Value(filmEntity), 
          EntityForm.Value("movie"), 
          EntityForm.Value("film")));
    }

    //TODO Intents with counts on entites (e.g. ENTITY_X, ENTITY_X, options => options.ExactCount())
    //TODO Intents with ordered entites (e.g. ENTITY_X, ENTITY_X, options => options.Ordered())
    //TODO Intents with excluded entites (e.g. ENTITY_X, ENTITY_X, options => options.Exclude(ENTITY_Y))
  }
}
