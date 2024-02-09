# Flowsy Db Abstractions

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
    // The fictitious ISalesUnitOfWorkFactory interface shall inherit from IUnitOfWorkFactory and the corresponding implementation shall be provided.
    private readonly ISalesUnitOfWorkFactory _unitOfWorkFactory;
    
    public CreateInvoiceCommandHandler(ISalesUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }
    
    public async Task<CreateInvoiceCommandResult> HandleAsync(CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        await using var unitOfWork = _unitOfWorkFactory.Create<ISalesUnitOfWork>();
        
        // Validate command
        
        var invoice = new Invoice();
        // Populate invoice object from properties of command object
        
        // Begin operation
        // IUnitOfWork inherits from IDisposable and IAsyncDisposable, if any exception is thrown, the current operation shall be rolled back
        unitOfWork.BeginWork();
        
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
        await unitOfWork.SaveWorkAsync(cancellationToken);
        
        // Return the result of the operation
        return new CreateInvoiceCommandResult
        {
            InvoiceId = invoiceId
        };
    }
}
```