using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static DelegateRuntimeMethods.ConsoleNETFrame.ExternalKernelCalls;

namespace DelegateRuntimeMethods.ConsoleNETFrame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string file = "DelegateRuntimeMethods.RandomLibCPP.dll";
            Console.WriteLine("press enter to load assembly");
            Console.ReadLine();

            var handle = ExternalKernelCalls.LoadLibraryEx(file, 0, 0);
            if (handle == IntPtr.Zero)
            {
                Console.WriteLine("Lib handle null: " + new Win32Exception().Message);
                Console.ReadLine();

                ExternalKernelCalls.FreeLibrary(handle);
                return;
            }

            var calculate = FunctionDelegateAccessor.GetMethod<Calculate>(handle, "Calculate");
            Console.WriteLine("10 + 10 = " + calculate(10, 10));

            var multiply = FunctionDelegateAccessor.GetMethod<Multiply>(handle, "Multiply");
            Console.WriteLine("10 * 10 = " + multiply(10, 10));

            ExternalKernelCalls.FreeLibrary(handle);
            Console.ReadLine();
        }

        public delegate int Calculate(int a, int b);
        public delegate int Multiply(int a, int b);
    }

    internal static class FunctionDelegateAccessor
    {
        public static TDelegate GetMethod<TDelegate>(IntPtr moduleHandle, string name) where TDelegate : Delegate
        {
            var methodProcAddress = ExternalKernelCalls.GetProcAddress(moduleHandle, name);
            if (methodProcAddress == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            
            var @delegate = Marshal.GetDelegateForFunctionPointer<TDelegate>(methodProcAddress);
            if (@delegate == null)
            {
                throw new Exception("Function delegate not found!");
            }
            return @delegate;
        }
    }

    internal static class ExternalKernelCalls
    {
        [DllImport(Kernel32LibraryName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(Kernel32LibraryName, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string fileName, int hFile, int flags);

        [DllImport(Kernel32LibraryName, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        private const string Kernel32LibraryName = "kernel32.dll";
    }
}
