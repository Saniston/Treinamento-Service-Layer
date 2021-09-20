using System;
using System.Collections.Generic;
using System.Text;

namespace Treinamento03.Models
{
    public class Document
    {
        public DateTime DocDueDate { get; set; }
        public int DocEntry { get; set; }
    }

    public class DraftService
    {
        public Document Document { get; set; }
        public DraftService(int docEntry)
        {
            Document = new Document()
            {
                DocDueDate = DateTime.Today.Date,
                DocEntry = docEntry
            };
        }
    }
}
