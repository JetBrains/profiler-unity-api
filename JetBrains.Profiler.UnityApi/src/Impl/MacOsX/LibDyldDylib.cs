using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace JetBrains.Profiler.UnityApi.Impl.MacOsX
{
  [SuppressMessage("ReSharper", "IdentifierTypo")]
  internal static class LibDyldDylib
  {
    private const string LibraryName = "/usr/lib/system/libdyld.dylib";

    [DllImport(LibraryName, ExactSpelling = true)]
    internal static extern IntPtr dlopen([MarshalAs(UnmanagedType.LPStr)] string filename, int flags);

    [DllImport(LibraryName, ExactSpelling = true)]
    internal static extern IntPtr dlsym(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string symbol);

    [DllImport(LibraryName, ExactSpelling = true)]
    internal static extern int dlclose(IntPtr handle);
  }
}