using System;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.HabitatDetector;
using JetBrains.Profiler.UnityApi.Impl.Linux;

namespace JetBrains.Profiler.UnityApi.Impl
{
  internal sealed class UnityProfilerInterop : IDisposable
  {
    internal readonly IsProfilerCreatedDelegate IsProfilerCreated;
    private readonly INativeLibrary myLibrary;
    internal readonly ReleaseStringDelegate ReleaseString;
    internal readonly StartProfilingDelegate StartProfiling;
    internal readonly StopProfilingDelegate StopProfiling;

    internal UnityProfilerInterop(string binDir)
    {
      myLibrary = CreateNativeLibrary(Path.Combine(binDir, Path.Combine(HabitatInfo.OSRuntimeIdString, MakeLibraryName("mono-profiler-jb"))));

      IsProfilerCreated = myLibrary.GetNativeFunction<IsProfilerCreatedDelegate>("IsProfilerCreated");
      ReleaseString = myLibrary.GetNativeFunction<ReleaseStringDelegate>("ReleaseString");
      StartProfiling = myLibrary.GetNativeFunction<StartProfilingDelegate>("StartProfiling");
      StopProfiling = myLibrary.GetNativeFunction<StopProfilingDelegate>("StopProfiling");
    }

    public void Dispose() => myLibrary.Dispose();

    private static string MakeLibraryName(string libraryName) => HabitatInfo.Platform switch
      {
        JetPlatform.Linux => "lib" + libraryName + ".so",
        JetPlatform.MacOsX => "lib" + libraryName + ".dylib",
        JetPlatform.Windows => libraryName + ".dll",
        _ => throw new PlatformNotSupportedException()
      };

    private static INativeLibrary CreateNativeLibrary(string libraryFullPath) => HabitatInfo.Platform switch
      {
        JetPlatform.Linux => new NativeLibrary(libraryFullPath),
        JetPlatform.MacOsX => new MacOsX.NativeLibrary(libraryFullPath),
        JetPlatform.Windows => new Windows.NativeLibrary(libraryFullPath),
        _ => throw new PlatformNotSupportedException()
      };

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate byte IsProfilerCreatedDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ReleaseStringDelegate(IntPtr str);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void StartProfilingDelegate(ref IntPtr errorMessage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void StopProfilingDelegate();
  }
}