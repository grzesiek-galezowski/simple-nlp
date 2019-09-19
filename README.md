# simple-nlp
Simple natural language processing library, designed to be used by simple chat bots. Detects Intents and entities. Based on simple algorithms, not machine learning-based.

# Example

```csharp
//GIVEN
var model = new RecognitionModel();

var borrowEntity = EntityName.Value("Borrow");
var returnEntity = EntityName.Value("Return");
var bookEntity = EntityName.Value("Book");
var filmEntity = EntityName.Value("Film");

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
recognitionResult1.Entities.Should().BeEquivalentTo(new []
{
  new RecognizedEntity(borrowEntity, "borrow"), 
  new RecognizedEntity(bookEntity, "book"), 
}, options => options.WithStrictOrdering());

recognitionResult2.TopIntent.Should().Be("RETURN_BOOK");
recognitionResult2.Entities.Should().BeEquivalentTo(new []
{
  new RecognizedEntity(returnEntity, "return"), 
  new RecognizedEntity(bookEntity, "book"), 
}, options => options.WithStrictOrdering());
recognitionResult3.TopIntent.Should().Be("BORROW_FILM");
recognitionResult3.Entities.Should().BeEquivalentTo(new []
{
  new RecognizedEntity(borrowEntity, "borrow"), 
  new RecognizedEntity(filmEntity, "film"), 
}, options => options.WithStrictOrdering());

recognitionResult4.TopIntent.Should().Be("RETURN_FILM");
recognitionResult4.Entities.Should().BeEquivalentTo(new []
{
  new RecognizedEntity(returnEntity, "return"), 
  new RecognizedEntity(filmEntity, "film"), 
}, options => options.WithStrictOrdering());
```
