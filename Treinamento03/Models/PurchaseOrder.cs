using System;
using System.Collections.Generic;

namespace Treinamento03
{
    public class PurchaseOrder
    {
        #region Propriedades
        public string CardCode { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime DocDate { get; set; }
        public string BPL_IDAssignedToInvoice { get; set; }
        #endregion
        public List<DocumentLine> DocumentLines { get; set; }
        public PurchaseOrder()
        {
            CardCode = "V00004";
            DocDueDate = DateTime.Now;
            DocDate = DateTime.Today;
            BPL_IDAssignedToInvoice = "1";
            DocumentLines = new List<DocumentLine>();
            DocumentLines.Add(new DocumentLine());
        }
    }
    public class DocumentLine
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public DocumentLine()
        {
            ItemCode = "A00099";
            Quantity = 1;
            Price = 1.5;
        }
    }
}
