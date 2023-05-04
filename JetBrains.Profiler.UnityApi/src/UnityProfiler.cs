using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using JetBrains.Profiler.UnityApi.Impl;

namespace JetBrains.Profiler.UnityApi
{
  [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
  [SuppressMessage("ReSharper", "UnusedType.Global")]
  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public sealed class UnityProfiler
  {
    private readonly string myBinDirectory;
    private UnityProfilerInterop? myProfiler;

    public UnityProfiler(string binDirectory)
    {
      myBinDirectory = binDirectory;
      myProfiler = new UnityProfilerInterop(myBinDirectory);
    }

    public void StartProfiling()
    {
      if (myProfiler == null)
        myProfiler = new UnityProfilerInterop(myBinDirectory);
      else if (myProfiler.IsProfilerCreated() != 0)
      {
        StopProfiling();

        // Note(kzmn): ???
        Thread.Sleep(1000);

        myProfiler = new UnityProfilerInterop(myBinDirectory);
      }

      var errorMessage = IntPtr.Zero;
      myProfiler.StartProfiling(ref errorMessage);
      if (errorMessage == IntPtr.Zero)
        return;

      try
      {
        throw new ApplicationException(Marshal.PtrToStringAnsi(errorMessage));
      }
      finally
      {
        myProfiler.ReleaseString(errorMessage);
        StopProfiling();
      }
    }

    public void StopProfiling()
    {
      if (myProfiler == null)
        return;
      myProfiler.StopProfiling();
      myProfiler.Dispose();
      myProfiler = null;
    }
  }
}