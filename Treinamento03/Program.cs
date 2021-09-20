using B1SLayer;
using System;
using System.Collections.Generic;
using Treinamento03.Models;

namespace Treinamento03
{
    class Program
    {
        static readonly string baseURI = "http://DESKTOP-VM39M7U:50001/b1s/v1/";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SessionController controller = new SessionController(baseURI);
            var currentSession = controller.Login<Session>("manager2");
            //controller.CreatePurchaseOrder(currentSession);
            //controller.ApprovalStages(currentSession, 30);
            //controller.GetExistent(currentSession, 31);
            //controller.ApprovalAndSaveDocument(currentSession, 32);
            //controller.ApprovalTeste();
            //ServiceLayerFluentMigrator.Connection.ServiceLayer sl = new ServiceLayerFluentMigrator.Connection.ServiceLayer(baseURI);
            //sl.Connect("SBODemo_BR", "manager", "sap@123");
            //var result = sl.SLConnection.Request("PurchaseOrders").GetAsync<List<PurchaseOrder>>().Result;
            //var stages = sl.SLConnection.Request("Requests").Filter($"WddCode eq {20}");

            controller.SaveDraftToDocument(currentSession,38);
            controller.Logout(currentSession);
            Console.ReadKey();
        }
    }
}
