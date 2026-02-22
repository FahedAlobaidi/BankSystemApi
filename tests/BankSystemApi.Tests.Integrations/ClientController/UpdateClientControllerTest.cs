using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemApi.Tests.Integrations.ClientController
{
    public class UpdateClientControllerTest: IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
    {




        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
