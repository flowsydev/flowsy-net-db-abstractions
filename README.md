# Flowsy Db Abstractions

## Unit Of Work

The interface **IUnitOfWork** represents an atomic operation for the underlying data store.
For example, for a SQL database, a class implementing **IUnitOfWork** shall be a wrapper for **IDbTransaction** objects.

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
    private readonly ISalesUnitOfWork _unitOfWork;
    
    public CreateInvoiceCommandHandler(ISalesUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CreateInvoiceCommandResult> HandleAsync(CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        // Validate command
        
        var invoice = new Invoice();
        // Populate invoice object from properties of command object
        
        // Begin operation
        // IUnitOfWork inherits from IDisposable and IAsyncDisposable, if any exception is thrown, the current operation shall be rolled back
        _unitOfWork.BeginWork();
        
        // Create the Invoice entity
        var invoiceId = await _unitOfWork.InvoiceRepository.CreateAsync(invoice, cancellationToken);
        
        // Create all the InvoiceItem entities
        foreach (var item in command.Items)
        {
            var invoiceItem = new InvoiceItem();
            // Populate invoiceItem object from properties of item object
            
            // Create each InvoiceItem entity
            await _unitOfWork.InvoiceItemRepository.CreateAsync(invoiceItem, cancellationToken); 
        }

        // Commit the current operation        
        await _unitOfWork.SaveWorkAsync(cancellationToken);
        
        // Return the result of the operation
        return new CreateInvoiceCommandResult
        {
            InvoiceId = invoiceId
        };
    }
}
```