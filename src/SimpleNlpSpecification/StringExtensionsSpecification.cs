using FluentAssertions;
using TddXt.SimpleNlp;
using Xunit;

namespace SimpleNlpSpecification
{
  public class StringExtensionsSpecification
  {
    [Fact]
    public void ShouldTokenizeStrings()
    {
      EntityForm.Value("a").Tokenize("a b c").Should().BeEquivalentTo(new [] {"", "a", "b c"}, options => options.WithStrictOrdering());
      EntityForm.Value("a").Tokenize("A B C").Should().BeEquivalentTo(new [] {"", "A", "B C"}, options => options.WithStrictOrdering());
      EntityForm.Value("A").Tokenize("a b c").Should().BeEquivalentTo(new [] {"", "a", "b c"}, options => options.WithStrictOrdering());
      EntityForm.Value("b").Tokenize("a b c").Should().BeEquivalentTo(new [] {"a", "b", "c"}, options => options.WithStrictOrdering());
      EntityForm.Value("c").Tokenize("a b c").Should().BeEquivalentTo(new [] {"a b", "c", ""}, options => options.WithStrictOrdering());
      EntityForm.Value("a").Tokenize("a").Should().BeEquivalentTo(new [] {"", "a", ""}, options => options.WithStrictOrdering());
      EntityForm.Value("a").Tokenize("abc").Should().BeEquivalentTo(new [] {"abc"}, options => options.WithStrictOrdering());
      EntityForm.Value("b").Tokenize("abc").Should().BeEquivalentTo(new [] {"abc"}, options => options.WithStrictOrdering());
      EntityForm.Value("c").Tokenize("abc").Should().BeEquivalentTo(new [] {"abc"}, options => options.WithStrictOrdering());
      EntityForm.Value("").Tokenize("").Should().BeEquivalentTo(new [] {"", "", ""}, options => options.WithStrictOrdering());
      EntityForm.Value("driver's").Tokenize("driver's license").Should().BeEquivalentTo(new [] {"", "driver's", "license"}, options => options.WithStrictOrdering());
    }
  }
}
