using System.Collections.Generic;

namespace Treinamento03.Models
{
    public class ApprovalRequests
    {
        //ApprovalRequests Code é o WddCode da tabela OWDD
        public List<ApprovalRequestDecision> ApprovalRequestDecisions { get; set; }
        public ApprovalRequests()
        {
            ApprovalRequestDecisions = new List<ApprovalRequestDecision>();
            ApprovalRequestDecisions.Add(new ApprovalRequestDecision());
        }
    }
    public class ApprovalRequestDecision
    {
        public string Status { get; set; }
        public string Remarks { get; set; }
        public ApprovalRequestDecision()
        {
            Status = "ardApproved";
            Remarks = "Tudo certo!";
        }
    }
}
