using Microsoft.EntityFrameworkCore;
using SqlJsonMigration;

var context = new Context();

Console.Write("Load existing documents... ");
var existing = await context.Documents.ToListAsync();
Console.WriteLine($"found {existing.Count}");

if (existing.Count == 0)
{ 
    Console.WriteLine($"Add a new document");
    context.Documents.Add(new MyDocument { Json = new() { Property1 = "hello", Property2 = 43 } });
    await context.SaveChangesAsync();
}
else
{
}
