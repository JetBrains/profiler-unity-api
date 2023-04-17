using System;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.HabitatDetector;

namespace JetBrains.Profiler.UnityApi.Impl
{
  internal sealed class UnityProfilerInterop : IDisposable
  {
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate byte IsProfilerCreatedDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void StartProfilingDelegate(ref IntPtr errorMessage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void StopProfilingDelegate();

    private readonly INativeLibrary myLibrary;

    public readonly IsProfilerCreatedDelegate IsProfilerCreated;
    public readonly StartProfilingDelegate StartProfiling;
    public readonly StopProfilingDelegate StopProfiling;
    private readonly ReleaseStringDelegate myReleaseString;

    public UnityProfilerInterop(string binDir)
    {
      myLibrary = CreateNativeLibrary(Path.Combine(binDir, Path.Combine(HabitatInfo.OSRuntimeIdString, MakeLibraryName("mono-profiler-jb"))));

      StartProfiling = myLibrary.GetNativeFunction<StartProfilingDelegate>("StartProfiling");
      StopProfiling = myLibrary.GetNativeFunction<StopProfilingDelegate>("StopProfiling");
      IsProfilerCreated = myLibrary.GetNativeFunction<IsProfilerCreatedDelegate>("IsProfilerCreated");
      myReleaseString = myLibrary.GetNativeFunction<ReleaseStringDelegate>("ReleaseString");
    }

    public void Dispose()
    {
      myLibrary.Dispose();
    }

    private static string MakeLibraryName(string libraryName) => HabitatInfo.Platform switch
      {
        JetPlatform.Linux => "lib" + libraryName + ".so",
        JetPlatform.MacOsX => "lib" + libraryName + ".dylib",
        JetPlatform.Windows => libraryName + ".dll",
        _ => throw new PlatformNotSupportedException()
      };

    private static INativeLibrary CreateNativeLibrary(string libraryFullPath) => HabitatInfo.Platform switch
      {
        JetPlatform.Linux => new Linux.NativeLibrary(libraryFullPath),
        JetPlatform.MacOsX => new MacOsX.NativeLibrary(libraryFullPath),
        JetPlatform.Windows => new Windows.NativeLibrary(libraryFullPath),
        _ => throw new PlatformNotSupportedException()
      };

    public void CheckError(IntPtr errorMessage)
    {
      if (errorMessage == IntPtr.Zero) return;
      var errorStr = Marshal.PtrToStringAnsi(errorMessage);
      myReleaseString(errorMessage);
      throw new ApplicationException(errorStr);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void ReleaseStringDelegate(IntPtr str);
  }
}