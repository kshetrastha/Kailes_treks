# Wallet transfer balance mismatch code fix

## Root cause
`senderBalance` is already the **post-debit** balance (it is read after `TryDebitAsync`).

In the admin history branch, this line double-subtracts the transfer amount and causes the mismatch:

```csharp
SenderBalanceAfter = (senderBalance - request.Amount)
```

## Required code update
Apply this exact change in `ExecuteTransferInternalAsync`:

```diff
- SenderBalanceAfter = (senderBalance - request.Amount),
+ SenderBalanceAfter = senderBalance,
```

## Correct admin transfer block

```csharp
if (isAdminTransfer)
{
    await _db.AdminWalletTransferHistories.AddAsync(new AdminWalletTransferHistory
    {
        Reference = reference,
        SenderClientId = sender.Id,
        ReceiverClientId = receiver.Id,
        SenderUserName = sender.ReferralCode,
        ReceiverUserName = receiver.ReferralCode,
        WalletBucket = request.WalletBucket,
        Amount = request.Amount,
        SenderBalanceAfter = senderBalance,
        ReceiverBalanceAfter = receiverBalance
    }, ct);
}
```
