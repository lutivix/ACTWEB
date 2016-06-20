using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public static class CppInterop
    {
        [DllImport("SampleDLL.dll", EntryPoint = "sum", CallingConvention = CallingConvention.StdCall)]
        public static extern int sum(int a, int b);

        [DllImport("SampleDLL.dll", EntryPoint = "otherSum", CallingConvention = CallingConvention.StdCall)]
        public static extern void otherSum(int a, int b, ref int result);

    }
}