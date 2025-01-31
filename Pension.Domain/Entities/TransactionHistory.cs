using System;
using System.Collections.Generic;

namespace Pension.Domain.Entities
{

    public class TransactionHistory
    {
        public Guid Id { get; set; }
        public Guid BenefitId { get; set; }
        public string ChangeDescription { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
    }
}