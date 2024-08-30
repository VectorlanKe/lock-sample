// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
int count=0;

List<Task> tasks = Enumerable.Range(0, 10).Select(p => Task.Run(AddCount)).ToList();
Task.WaitAll(tasks.ToArray());
Console.WriteLine("Hello, World!");

[MethodImpl(MethodImplOptions.Synchronized)]
void AddCount()
{
    count++;
    Console.Out.WriteLine($"线程id：{Thread.CurrentThread.ManagedThreadId:0000}  Count:{count}");
}