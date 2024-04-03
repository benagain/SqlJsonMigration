## Demonstration of problems adding new properties to an Entity Framework model using `ToJson`

Adding new properties to a class that is mapped using the `ToJson` conversion prevents any rows that were created before the new property was added from being updated by EF.

We start with a document containing two properties, and add an instance of it to the database:
https://github.com/benagain/SqlJsonMigration/blob/7b2ebf21641735d527f33507f9b4ca63b445ffa4/Context.cs#L27-L31

We then add an additional, nullable, property and attempt to update the original row:
https://github.com/benagain/SqlJsonMigration/blob/b92af88b853d7edc7d6a217068f9ddabc1b63583/Context.cs#L27-L32

This results in an exception when we save:

```
Property cannot be found on the specified JSON path.
```

See dotnet/efcore#32301.

## Steps to reproduce

0. (Optional) Drop the database from previous runs

```
[main]> dotnet ef database drop

Build started...
Build succeeded.
Are you sure you want to drop the database 'SqlJsonMigration' on server '(localdb)\MSSQLLocalDb'? (y/N) y
Dropping database 'SqlJsonMigration' on server '(localdb)\MSSQLLocalDb'.
```

2. Apply the first migration - without the original JSON document

```
[main]> git checkout initial

Note: switching to 'initial'.
HEAD is now at 7b2ebf2 First document

[(initial)]> dotnet ef database update

Build started...
Build succeeded.
Applying migration '20240403060309_Initial'.
Done.
```

3. Run the program to add a document with the original schema

```
[(initial)]> .\bin\Debug\net8.0\SqlJsonMigration.exe

Load existing documents... found 0
Add a new document
```

4. Apply the second migration - with a new optional property on the JSON document
    
```
[(initial)]> git checkout addition

Previous HEAD position was 7b2ebf2 First document
HEAD is now at b92af88 Added optional property to JSON

[(addition)]> dotnet ef database update

Build started...
Build succeeded.
Applying migration '20240403060855_NullableProperty'.
    Done.
```

5. Run the program to update the original document (which has the old schmema) and add the new optional property

```
[(addition)]> .\bin\Debug\net8.0\SqlJsonMigration.exe

Load existing documents... found 1
Set new property

Unhandled exception. Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while saving the entity changes. See the inner exception for details.
---> Microsoft.Data.SqlClient.SqlException (0x80131904): Property cannot be found on the specified JSON path.
at Microsoft.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
at Microsoft.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
at Microsoft.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
at Microsoft.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
at Microsoft.Data.SqlClient.SqlDataReader.TryHasMoreRows(Boolean& moreRows)
at Microsoft.Data.SqlClient.SqlDataReader.TryReadInternal(Boolean setTimeout, Boolean& more)
at Microsoft.Data.SqlClient.SqlDataReader.ReadAsyncExecute(Task task, Object state)
at Microsoft.Data.SqlClient.SqlDataReader.InvokeAsyncCall[T](SqlDataReaderBaseAsyncCallContext`1 context)
--- End of stack trace from previous location ---
at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeResultSetWithRowsAffectedOnlyAsync(Int32 commandIndex, RelationalDataReader reader, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
ClientConnectionId:9603f5a7-5d05-404e-a069-efe1c0b09a6a
Error Number:13608,State:2,Class:16
--- End of inner exception stack trace ---
at Microsoft.EntityFrameworkCore.Update.AffectedCountModificationCommandBatch.ConsumeAsync(RelationalDataReader reader, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.SqlServer.Update.Internal.SqlServerModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(StateManager stateManager, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerExecutionStrategy.ExecuteAsync[TState,TResult](TState state,
Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
at Program.<Main>$(String[] args) in C:\dev\SqlJsonMigration\SqlJsonMigration\Program.cs:line
20
at Program.<Main>(String[] args)
```