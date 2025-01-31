using System;
using System.Collections.Generic;
using Pension.Domain.Entities;

namespace Pension.Domain.Entities
{
    public class Benefit
    {
        public Guid Id { get; set; }
        public string BenefitType { get; set; } = string.Empty;
        public DateTime CalculationDate { get; set; }
        public bool EligibilityStatus { get; set; }
        public decimal Amount { get; set; }
        public List<TransactionHistory> Transactions { get; set; } = new();

        public void AddTransaction(string description)
        {
            Transactions.Add(new TransactionHistory
            {
                BenefitId = Id,
                ChangeDescription = description,
                ChangeDate = DateTime.UtcNow
            });
        }
    }

}