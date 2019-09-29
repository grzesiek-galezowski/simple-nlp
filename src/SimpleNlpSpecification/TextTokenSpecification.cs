using FluentAssertions;
using TddXt.SimpleNlp;
using TddXt.XFluentAssertRoot;
using Xunit;

namespace TddXt.SimpleNlpSpecification
{
  public class TextTokenSpecification
  {
    [Fact]
    public void ShouldHaveValueSemantics()
    {
      typeof(TextToken).Should().HaveValueSemantics();
    }
  }
}