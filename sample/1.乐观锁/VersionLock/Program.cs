// See https://aka.ms/new-console-template for more information
using FreeSql;
using VersionLock.Models;

IFreeSql fsql = new FreeSqlBuilder()
    .UseConnectionString(DataType.Sqlite, @"Data Source=freedb.db")
    .UseAutoSyncStructure(true)
    .Build();
string id = "1";
await fsql.InsertOrUpdate<Counter>().SetSource(new Counter
{
    Id = id,
    Count = 0,
    Version = 0,
}).ExecuteAffrowsAsync();
List<Task> tasks = Enumerable.Range(0, 10).Select(p => Task.Run(CounterTask)).ToList();
Task.WaitAll(tasks.ToArray());

Console.WriteLine("Hello, World!");
Counter resultMode = await fsql.Select<Counter>().Where(p => p.Id == id).ToOneAsync();
Console.WriteLine(resultMode.Count);
Console.WriteLine(resultMode.Version);


async Task CounterTask()
{
    Counter model = await fsql.Select<Counter>().Where(p => p.Id == id).ToOneAsync();
    int oldCount =model.Count;
    model.Count = Random.Shared.Next(0, 100);
    int result = await fsql.Update<Counter>()
        .Set(p => p.Count, model.Count)
        .Set(p => p.Version, model.Version + 1)
        .Where(p => p.Id == id && p.Version == model.Version)
        .ExecuteAffrowsAsync();
    await Console.Out.WriteLineAsync($"{result} 线程id：{Thread.CurrentThread.ManagedThreadId:0000}  旧值：{oldCount:00}  新值：{model.Count:00}  当前版本：{model.Version:00}");
}
