unsafe class Program
{
    [System.Runtime.RuntimeExport("_start")]
    static void Main()
    {
        Console.WriteLine("Hello from a NativeAOT compiled PE!\n.NET Version=6");

        while (true) ;
    }
}
