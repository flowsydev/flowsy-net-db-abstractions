# Flowsy Db Abstractions


## Connection Factory
The interface **IDbConnectionFactory** represents a mechanism to create database
connections from a list of registered configurations identified by unique keys.


## Unit Of Work

The interface **IUnitOfWork** represents an atomic operation for the underlying data store.
For example, for a SQL database, a class implementing **IUnitOfWork** shall be a wrapper for **IDbTransaction** objects.

On the other hand, implementations of **IUnitOfWorkFactory** shall create instances of the requested type of unit of work.
A unit of work shall group a set of repositories or other kinds of objects designed to access data, which are related in
the context of a given operation.

For example, to create an invoice, we may need to create two kinds of entities:
* Invoice
    * InvoiceId
    * CustomerId
    * CreateDate
    * Total
    * Taxes
    * GrandTotal
* InvoiceItem
    * InvoiceItemId
    * InvoiceId
    * ProductId
    * Quantity
    * Amount

The way of completing such operation from an application-level command handler could be:
```csharp
public class CreateInvoiceCommandHandler
{
    public async Task<CreateInvoiceCommandResult> HandleAsync(CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        // Begin operation
        // IUnitOfWork inherits from IDisposable and IAsyncDisposable, if any exception is thrown, the current operation shall be rolled back
        await using var unitOfWork = unitOfWorkFactory.Create<ICreateInvoiceUnitOfWork>();

        var invoice = new Invoice();
        // Populate invoice object from properties of command object 
        
        // Create the Invoice entity
        var invoiceId = await unitOfWork.InvoiceRepository.CreateAsync(invoice, cancellationToken);
        
        // Create all the InvoiceItem entities
        foreach (var item in command.Items)
        {
            var invoiceItem = new InvoiceItem();
            // Populate invoiceItem object from properties of item object
            
            // Create each InvoiceItem entity
            await unitOfWork.InvoiceItemRepository.CreateAsync(invoiceItem, cancellationToken); 
        }

        // Commit the current operation        
        await unitOfWork.CommitAsync(cancellationToken);
        
        // Return the result of the operation
        return new CreateInvoiceCommandResult
        {
            InvoiceId = invoiceId
        };
    }
}
```