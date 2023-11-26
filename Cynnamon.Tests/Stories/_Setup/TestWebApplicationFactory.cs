using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Cynnamon.Tests.Stories;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class {
    protected override IHost CreateHost(IHostBuilder builder) {
        builder.ConfigureServices(services => { });
        return base.CreateHost(builder);
    }
}
