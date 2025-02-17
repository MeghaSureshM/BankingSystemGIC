using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Modals
{
    public class InterestRule
    {
        public DateTime RuleDate  { get; set; }
        public string RuleId { get; set; }  
        public double InterestRate { get; set; }

        public InterestRule(DateTime ruleDate, string ruleId, double interestRate)
        {
            RuleDate = ruleDate;
            RuleId = ruleId;
            InterestRate = interestRate;
        }

        public override string ToString()
        {
            return $"Interest rules:\n| Date     | RuleId | Rate (%) |\n| {RuleDate:yyyyMMdd} | {RuleId} | {InterestRate,8:F2} |";

        }
    }

    
}
