// See https://aka.ms/new-console-template for more information

string testValue = string.Empty;
List<Task> tasks = Enumerable.Range(0, 10).Select(p => Task.Run(UpDataValueTask)).ToList();
Task.WaitAll(tasks.ToArray());
Console.WriteLine("Hello, World!");
Console.WriteLine(testValue);


void UpDataValueTask()
{
    string oldValue = testValue;
    // 计算新值
    string newValue = Random.Shared.Next(100).ToString();
    // CAS 更新数据
    //比较location1和comparand是否相等，如果不相等，什么都不做；如果location1与comparand相等，则用value替换location1的值。无论比较结果相等与否，返回值都是location1中原有的值
    //Interlocked.Increment递增，Interlocked.Decrement递减。
    var result = Interlocked.CompareExchange(ref testValue, newValue, oldValue);
    // 判断 CAS 是否成功
    if (result == oldValue)
    {
        // CAS 成功，执行后续操作
    }
    Console.Out.WriteLine($"{result == oldValue} 线程id：{Thread.CurrentThread.ManagedThreadId:0000}  旧值：{oldValue}  新值：{newValue}");
}