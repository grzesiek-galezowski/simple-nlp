using System.Linq;
using FluentAssertions;
using TddXt.SimpleNlp;
using Xunit;
using static TddXt.SimpleNlp.TextToken;

namespace SimpleNlpSpecification
{
  public class StringExtensionsSpecification
  {
    [Fact]
    public void ShouldTokenizeStrings()
    {
      EntityForm.Value("a").Tokenize(NotMatched("a b c"))
        .Should().BeEquivalentTo(new [] {NotMatched(""), Matched("a"), NotMatched("b c")}, options => options.WithStrictOrdering());
      EntityForm.Value("a").Tokenize(NotMatched("A B C"))
        .Should().BeEquivalentTo(new [] {NotMatched(""), Matched("A"), NotMatched("B C")}, options => options.WithStrictOrdering());
      EntityForm.Value("A").Tokenize(NotMatched("a b c"))
        .Should().BeEquivalentTo(new[] { NotMatched(""), Matched("a"), NotMatched("b c")}, options => options.WithStrictOrdering());
      EntityForm.Value("b").Tokenize(NotMatched("a b c"))
        .Should().BeEquivalentTo(new[] { NotMatched("a"), Matched("b"), NotMatched("c")}, options => options.WithStrictOrdering());
      EntityForm.Value("c").Tokenize(NotMatched("a b c"))
        .Should().BeEquivalentTo(new[] { NotMatched("a b"), Matched("c"), NotMatched("")}, options => options.WithStrictOrdering());
      EntityForm.Value("a").Tokenize(NotMatched("a"))
        .Should().BeEquivalentTo(new[] { NotMatched(""), Matched("a"), NotMatched("")}, options => options.WithStrictOrdering());
      EntityForm.Value("a").Tokenize(NotMatched("abc"))
        .Should().BeEquivalentTo(new[] { NotMatched("abc")}, options => options.WithStrictOrdering());
      EntityForm.Value("b").Tokenize(NotMatched("abc"))
        .Should().BeEquivalentTo(new[] { NotMatched("abc")}, options => options.WithStrictOrdering());
      EntityForm.Value("c").Tokenize(NotMatched("abc"))
        .Should().BeEquivalentTo(new[] { NotMatched("abc")}, options => options.WithStrictOrdering());
      EntityForm.Value("").Tokenize(NotMatched(""))
        .Should().BeEquivalentTo(new[] { Matched(""), Matched(""), Matched("") }, options => options.WithStrictOrdering());
      EntityForm.Value("driver's").Tokenize(NotMatched("driver's license"))
        .Should().BeEquivalentTo(new[] { NotMatched(""), Matched("driver's"), NotMatched("license")}, options => options.WithStrictOrdering());
    }

    private static TextToken[] Tokens(params string[] strings)
    {
      return strings.Select(s => Matched(s)).ToArray();
    }
  }
}
