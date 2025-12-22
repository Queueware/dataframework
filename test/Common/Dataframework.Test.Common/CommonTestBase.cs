using AutoFixture;

namespace Dataframework.Test.Common;

public abstract class CommonTestBase
{
    private readonly IFixture _fixture = new Fixture();

    protected CommonTestBase()
    {
        FluentAssertions.License.Accepted = true;
    }
    
    protected T Create<T>() => _fixture.Create<T>();

    protected IEnumerable<T> CreateMany<T>(int count = 3) => _fixture.CreateMany<T>(count);
}
