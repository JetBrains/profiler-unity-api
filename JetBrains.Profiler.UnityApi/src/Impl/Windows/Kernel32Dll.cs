using System;
using System.Runtime.InteropServices;

namespace JetBrains.Profiler.UnityApi.Impl.Windows
{
  internal static class Kernel32Dll
  {
    private const string LibraryName = "kernel32.dll";

    [DllImport(LibraryName, CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr LoadLibraryW(string lpFileName);

    [DllImport(LibraryName, CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
    public static extern int FreeLibrary(IntPtr hModule);

    [DllImport(LibraryName, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);
  }
}