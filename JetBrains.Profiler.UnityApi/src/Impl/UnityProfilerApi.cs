using System;
using System.Threading;

namespace JetBrains.Profiler.UnityApi.Impl;

public class UnityProfilerApi
{
  private readonly string myBinDirectory;

  private UnityProfilerInterop? myUnityProf;

  private static UnityProfilerInterop CreateProfiler(string binDir)
  {
    return new UnityProfilerInterop(binDir);
  }

  public UnityProfilerApi(string binDirectory)
  {
    myBinDirectory = binDirectory;
    myUnityProf = CreateProfiler(myBinDirectory);
  }

  public void StartProfiling()
  {
    if (myUnityProf != null && myUnityProf.IsProfilerCreated() != 0)
    {
      StopProfiling();
      Thread.Sleep(1000);
    }

    myUnityProf = CreateProfiler(myBinDirectory);

    var createErr = IntPtr.Zero;
    myUnityProf.StartProfiling(ref createErr);
    myUnityProf.CheckError(createErr);
  }

  public void StopProfiling()
  {
    if (myUnityProf != null)
    {
      myUnityProf.StopProfiling();
      myUnityProf.Dispose();
      myUnityProf = null;
    }
  }
}