using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prism_App.Services.Request
{
    public interface IRequestService
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "", CancellationTokenSource cancellationToken = default(CancellationTokenSource));
        Task<TResult> PostAsync<TResult>(string uri, string token = "", CancellationTokenSource cancellationToken = default(CancellationTokenSource));
        Task<TResult> PostAsync<TResult>(string uri, Dictionary<string, string> data, string token = "", CancellationTokenSource cancellationToken = default(CancellationTokenSource));
    }
}
