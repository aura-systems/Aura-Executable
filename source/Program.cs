namespace SampleProgram
{
    public class Program
    {
        [System.Runtime.RuntimeExport("Main")]
        static void Main()
        {
            //Console.Clear();
            Console.WriteLine("Hello from a Portable Executable!\n");
            Console.WriteLine("Compiled using NativeAOT and .NET 6\n");
        }
    }
}