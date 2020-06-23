using Microsoft.Extensions.Options;
using System;

namespace Kentico.Kontent.Boilerplate.Configuration
{
    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
