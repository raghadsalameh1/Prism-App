using Prism_App.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Prism_App.Services.Field
{
    public interface IFieldsService
    {
        Task<boundary> CreateBoundary(Position position, CancellationTokenSource cancellationTokenSource = null);
    }
}
