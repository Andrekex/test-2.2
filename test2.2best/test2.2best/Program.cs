public interface ITransaction
{
    void Execute();
    bool CheckStatus();
}

public abstract class Transaction : ITransaction
{
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public bool Status { get; protected set; }

    public Transaction(decimal amount, DateTime date)
    {
        Amount = amount;
        TransactionDate = date;
        Status = false;
    }

    public abstract void Execute();

    public bool CheckStatus()
    {
        return Status;
    }
}

public class DepositTransaction : Transaction
{
    public DepositTransaction(decimal amount, DateTime date) : base(amount, date) { }

    public override void Execute()
    {
        Console.WriteLine($"depositing {Amount} on {TransactionDate}");
        Status = true;
    }
}

public class WithdrawalTransaction : Transaction
{
    public WithdrawalTransaction(decimal amount, DateTime date) : base(amount, date) { }

    public override void Execute()
    {
        Console.WriteLine($"withdrawing {Amount} on {TransactionDate}");
        Status = true;
    }
}

public delegate void TransactionCompleted(Transaction transaction);

public class TransactionProcessor
{
    public event TransactionCompleted OnTransactionCompleted;

    public void ProcessTransaction(Transaction transaction)
    {
        transaction.Execute();
        OnTransactionCompleted?.Invoke(transaction);
    }
}

public class TransactionException : Exception
{
    public TransactionException(string message) : base(message) { }
}

class Program
{
    static void Main()
    {
        var deposit = new DepositTransaction(100, DateTime.Now);
        var processor = new TransactionProcessor();
        processor.OnTransactionCompleted += transaction =>
            Console.WriteLine($"tranaction completed with this status: {transaction.CheckStatus()}");

        processor.ProcessTransaction(deposit);
    }
}
