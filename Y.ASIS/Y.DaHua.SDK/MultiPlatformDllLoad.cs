using System;
using System.IO;
using System.Runtime.InteropServices;

public class MultiPlatformDllLoad
{
    [DllImport("kernel32")]
    private static extern IntPtr LoadLibraryA([MarshalAs(UnmanagedType.LPStr)] string fileName);

    public static IntPtr LoadDll(string fileName)
    {
        var dllFile = Path.Combine(AppContext.BaseDirectory, Environment.Is64BitProcess ? "x64" : "x86", fileName);
        var ptr = LoadLibraryA(dllFile);
        return ptr;
    }
}
