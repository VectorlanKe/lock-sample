using System.ComponentModel.DataAnnotations;

namespace VersionLock.Models;

public class Counter
{
    [Key]
    public string Id { get; set; }

    public int Count { get; set; }

    public long Version{get;set;}
}