using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace JetBrains.Profiler.UnityApi.Impl.Linux
{
  [SuppressMessage("ReSharper", "IdentifierTypo")]
  internal static class LibDlSo2
  {
    private const string LibraryName = "libdl.so.2"; // Note: Don't use libdl.so because no such library in clean system!

    [DllImport(LibraryName, ExactSpelling = true)]
    internal static extern IntPtr dlopen([MarshalAs(UnmanagedType.LPStr)] string filename, int flags);

    [DllImport(LibraryName, ExactSpelling = true)]
    internal static extern IntPtr dlsym(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string symbol);

    [DllImport(LibraryName, ExactSpelling = true)]
    internal static extern int dlclose(IntPtr handle);
  }
}