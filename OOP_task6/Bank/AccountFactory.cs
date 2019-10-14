using System;
using System.Reflection;

namespace Bank
{
    public class AccountFactory
    {
        public Account CreateAccount(Type type, double rate, double commission)
        {
            Account account = (Account) type
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,null, Type.EmptyTypes, null)
                    ?.Invoke(null);
            // ReSharper disable once PossibleNullReferenceException
            account.Commission = commission;
            account.Rate = rate;
            return account;
        }
    }
}