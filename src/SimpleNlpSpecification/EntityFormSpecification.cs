using FluentAssertions;
using TddXt.SimpleNlp;
using TddXt.XFluentAssertRoot;
using Xunit;
using static TddXt.SimpleNlp.TextToken;

namespace TddXt.SimpleNlpSpecification
{
  public class EntityFormSpecification
  {
    [Fact]
    public void ShouldTokenizeStrings()
    {
      EntityForm.Value("a").Tokenize(NotMatched("a b c"))
        .Should().Equal(NotMatched(""), Matched("a"), NotMatched("b c"));
      EntityForm.Value("a").Tokenize(NotMatched("A B C"))
        .Should().Equal(NotMatched(""), Matched("A"), NotMatched("B C"));
      EntityForm.Value("A").Tokenize(NotMatched("a b c"))
        .Should().Equal(NotMatched(""), Matched("a"), NotMatched("b c"));
      EntityForm.Value("b").Tokenize(NotMatched("a b c"))
        .Should().Equal(NotMatched("a"), Matched("b"), NotMatched("c"));
      EntityForm.Value("c").Tokenize(NotMatched("a b c"))
        .Should().Equal(NotMatched("a b"), Matched("c"), NotMatched(""));
      EntityForm.Value("a").Tokenize(NotMatched("a"))
        .Should().Equal(NotMatched(""), Matched("a"), NotMatched(""));
      EntityForm.Value("a").Tokenize(NotMatched("abc"))
        .Should().Equal(NotMatched("abc"));
      EntityForm.Value("b").Tokenize(NotMatched("abc"))
        .Should().Equal(NotMatched("abc"));
      EntityForm.Value("c").Tokenize(NotMatched("abc"))
        .Should().Equal(NotMatched("abc"));
      EntityForm.Value("").Tokenize(NotMatched(""))
        .Should().Equal(Matched(""), Matched(""), Matched(""));
      EntityForm.Value("driver's").Tokenize(NotMatched("driver's license"))
        .Should().Equal(NotMatched(""), Matched("driver's"), NotMatched("license"));
    }

    [Fact]
    public void ShouldHaveValueSemantics()
    {
      typeof(EntityForm).Should().HaveValueSemantics();
    }
  }
}
