using System;

namespace JetBrains.Profiler.UnityApi.Impl
{
  internal interface INativeLibrary : IDisposable
  {
    TDelegate GetNativeFunction<TDelegate>(string functionName) where TDelegate : Delegate;
  }
}