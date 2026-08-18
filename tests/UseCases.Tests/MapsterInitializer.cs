using System.Runtime.CompilerServices;
using HospitalSaoJose.Application.Mappings;

namespace UseCases.Tests;

internal static class MapsterInitializer
{
    /// <summary>
    /// Os use cases usam <c>.Adapt&lt;T&gt;()</c>; sem a configuração registrada os
    /// <c>Ignore</c> não valem e o mapeamento falha em runtime.
    /// </summary>
    [ModuleInitializer]
    internal static void Initialize() => MapsterConfiguration.Configure();
}
