using System;
using System.Runtime.InteropServices;

namespace JetBrains.Profiler.UnityApi.Impl.Windows
{
  internal sealed class NativeLibrary : INativeLibrary
  {
    private readonly IntPtr myHandle;

    public NativeLibrary(string libraryPath)
    {
      myHandle = Kernel32Dll.LoadLibraryW(libraryPath);
      if (myHandle == IntPtr.Zero)
        throw new DllNotFoundException("Failed to load shared library " + libraryPath);
    }

    public TDelegate GetNativeFunction<TDelegate>(string functionName) where TDelegate : Delegate
    {
      var ptr = Kernel32Dll.GetProcAddress(myHandle, functionName);
      if (ptr == IntPtr.Zero)
        throw new EntryPointNotFoundException("Failed to get a function entry point " + functionName);
#pragma warning disable CS0618
      return (TDelegate)Marshal.GetDelegateForFunctionPointer(ptr, typeof(TDelegate));
#pragma warning restore CS0618
    }

    void IDisposable.Dispose()
    {
      Kernel32Dll.FreeLibrary(myHandle);
    }
  }
}