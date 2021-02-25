using Prism_App.Models;
using Prism_App.Services.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Prism_App.Services.Field
{
    public class FieldsService : IFieldsService
    {
        private readonly IRequestService _requestService;

        public FieldsService(IRequestService requestService)
        {
            _requestService = requestService;
        }
        public async Task<boundary> CreateBoundary(Position position, CancellationTokenSource cancellationTokenSource = null)
        {
            var builder = new UriBuilder(GlobalSettings.Endpoint);
            var formData = new Dictionary<string, string>
            {
                {
                    "latitude", position.Latitude.ToString()
                },
                {
                    "longitude", position.Longitude.ToString()
                }
            };
            var uri = builder.ToString();
            var authToken = "auth_token";
            var result = await _requestService.PostAsync<boundary>(uri, formData, authToken, cancellationTokenSource);
            return result;
        }
    }
}
