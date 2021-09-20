using RestSharp;
using System;
using Treinamento03.Models;
using Newtonsoft.Json;

namespace Treinamento03
{
    public class SessionController
    {
        public string BaseURI { get; }
        public RestClient ClientRest { get;}
        /// <summary>
        /// Construtor para o objeto passando host
        /// </summary>
        /// <param name="baseURI">Caminho principal para o host</param>
        public SessionController(string baseURI)
        {
            ClientRest = new RestClient(baseURI);
            BaseURI = baseURI;
        }
        /// <summary>
        /// Realiza pedido de compra
        /// </summary>
        /// <typeparam name="t">Tipo da sessão</typeparam>
        /// <param name="currentSession">Sessão atual</param>
        /// /// <summary>
        /// Aprovação de estapas
        /// </summary>
        /// <typeparam name="t">Tipo da sessão</typeparam>
        /// <param name="currentSession">Sessão atual</param>
        /// <param name="code">código do pedido</param>
        public void ApprovalStages<t>(t currentSession, int code) where t : Session
        {
            var request = new RestRequest($"ApprovalRequests({code})", DataFormat.Json)
            {
                Method = Method.PATCH
            };
            request.AddCookie("B1SESSION", currentSession.SessionId);
            request.AddJsonBody(new ApprovalRequests());
            var result = ClientRest.ExecuteAsPost<ApprovalRequests>(request, "PATCH");
            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"Etapa {code} aprovada!");
                Console.WriteLine(result.Content);
            }
            else
            {
                Console.WriteLine(result.Content);
                Console.WriteLine("Status code: " + result.StatusCode);
            }
        }
        /// <summary>
        /// Aprova a etapa e, se todas as etapas de aprovação foram cumpridas, gera documento
        /// </summary>
        /// <typeparam name="t">Tipo da sessão</typeparam>
        /// <param name="currentSession">Sessão atual</param>
        /// <param name="wddCode"></param>
        public void ApprovalAndSaveDocument<t>(t currentSession, int wddCode) where t : Session
        {
            ApprovalStages(currentSession, wddCode);
            var status = GetExistent(currentSession, wddCode);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(4));
            if (status.Status.Equals("arsApproved"))
                SaveDraftToDocument(currentSession, status.DraftEntry);
        }

        public void ApprovalTeste()
        {
            ServiceLayerFluentMigrator.Connection.ServiceLayer sl =
                new ServiceLayerFluentMigrator.Connection.ServiceLayer("https://desktop-vm39m7u:50000/b1s/v1");
            sl.Connect("SBODemo_BR", "manager2", "sap@123");
            var result = sl.SLConnection.Request("ApprovalRequests", 33).GetAsync<DraftWDD>().Result;
            Console.WriteLine(result.Status);
            var result2 = sl.SLConnection.Request("ApprovalRequests", 33).PatchAsync(new ApprovalRequests());
            Console.WriteLine(result2.Status);
            Console.WriteLine(result.Status);
        }

        /// <summary>
        /// Realiza novo pedido de compra
        /// </summary>
        /// <typeparam name="t">Tipo da sessão</typeparam>
        /// <param name="currentSession">Sessão atual</param>
        public void CreatePurchaseOrder<t>(t currentSession) where t:Session
        {
            var request = new RestRequest("PurchaseOrders", DataFormat.Json)
            {
                Method = Method.POST
            };
            request.AddCookie("B1SESSION", currentSession.SessionId);

            request.AddJsonBody(new PurchaseOrder());
            var result = ClientRest.ExecuteAsPost<PurchaseOrder>(request, "POST");
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine(result.Content);
            }
            else
            {
                Console.WriteLine(result.Content);
                Console.WriteLine("Status code: " + result.StatusCode);
            }
        }
        /// <summary>
        /// Realiza o Login da sessão
        /// </summary>
        /// <typeparam name="t">Tipo da sessão</typeparam>
        /// <param name="currentSession">Sessão atual</param>
        /// <returns>Sessão atual</returns>
        public t Login<t>(string user) where t : Session
        {
            var request = new RestRequest("Login", DataFormat.Json)
            {
                Method = Method.POST
            };
            request.AddJsonBody(
                new
                {
                    CompanyDB = "SBODemo_BR",
                    UserName = user,
                    Password = "sap@123"
                });
            t currentSession = null;
            var result = ClientRest.ExecuteAsPost<t>(request, "POST");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Login aceito! " + result.StatusCode);
                currentSession = result.Data;
                Console.WriteLine(result.Content);
                Console.WriteLine(currentSession.OdataMetadata);
            }
            else
            {
                Console.WriteLine(result.Content);
            }
            return currentSession;
        }
        /// <summary>
        /// Realiza o Logout da sessão
        /// </summary>
        /// <typeparam name="t">Tipo da sessão</typeparam>
        /// <param name="currentSession">Sessão atual</param>
        public void Logout<t>(t currentSession) where t : Session
        {
            var request = new RestRequest("Logout", Method.POST);
            var result = ClientRest.ExecuteAsPost<Session>(request, "POST");
            request.AddCookie("B1SESSION", currentSession.SessionId);
            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine(result.Content);
                Console.WriteLine("Fim da Sessão!");
            }
            else
            {
                Console.WriteLine(result.Content);
            }
        }
        /// <summary>
        /// Salva rascunho aprovado como documento
        /// </summary>
        /// <typeparam name="t">Tipo da sessão</typeparam>
        /// <param name="currentSession">Sessão atual</param>
        /// <param name="docEntry">Codigo do DocEntry</param>
        public void SaveDraftToDocument<t>(t currentSession, int docEntry) where t:Session
        {
            var request = new RestRequest("DraftsService_SaveDraftToDocument", DataFormat.Json)
            {
                Method = Method.POST
            };
            request.AddJsonBody(new DraftService(docEntry));
            request.AddCookie("B1SESSION", currentSession.SessionId);
            var result = ClientRest.ExecuteAsPost<DraftService>(request, "POST");
            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"Rascunho {docEntry} salvo em documento!");
                Console.WriteLine(result.Content);
            }
            else
            {
                Console.WriteLine(result.Content);
                Console.WriteLine("Documento não salvo!Status code: " + result.StatusCode);
            }
        }
        /// <summary>
        /// Busca por um pedido em etapa de aprovação e retorna o objeto do Draft
        /// </summary>
        /// <typeparam name="t">Tipo da sessão</typeparam>
        /// <param name="currentSession">Sessão atual</param>
        /// <param name="wddCode">Código WDD</param>
        /// <returns></returns>
        public DraftWDD GetExistent<t>(t currentSession, int wddCode) where t :Session
        {
            //var request = new RestRequest($"ApprovalRequests({wddCode})", Method.GET);
            var request = new RestRequest($"ApprovalRequests({wddCode})?$select=Status, Code, DraftEntry", Method.GET);
            request.AddCookie("B1SESSION", currentSession.SessionId);
            var result = ClientRest.ExecuteAsGet<ApprovalRequests>(request, "GET");
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Etapa {wddCode} Existe!");
                var resultJ = JsonConvert.DeserializeObject<DraftWDD>(result.Content);
                Console.WriteLine($"Status:{resultJ.Status}\nWddCode:{resultJ.Code}\nDraftEntry:{resultJ.DraftEntry}");
                return resultJ;
            }
            else
            {
                Console.WriteLine(result.Content);
                Console.WriteLine("Status code: " + result.StatusCode);
                return null;
            }
        }
    }
}
