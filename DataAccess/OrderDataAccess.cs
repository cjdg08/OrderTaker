using DataAccess.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace DataAccess
{
    public class OrderDataAccess : IOrderDataAccess
    {
        public List<PurchaseOrderDTO> GetOrderList()
        {
            List<PurchaseOrderDTO> purchaseOrder = new List<PurchaseOrderDTO>();
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                purchaseOrder = SqlMapper.Query<PurchaseOrderDTO>(con, "GetOrderList", commandType: CommandType.StoredProcedure).ToList();
            }
            return purchaseOrder;
        }

        public string InsertPurchseOrder(PurchaseOrderDTO purchaseOrder, List<string> auditChanges)
        {
            string result = "";
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@CustomerID", purchaseOrder.CustomerID);
                        param.Add("@DateOfDeliver", purchaseOrder.DateOfDelivery);
                        param.Add("@Status", purchaseOrder.Status);
                        param.Add("@AmountDue", purchaseOrder.AmountDue);

                        int ID = con.ExecuteScalar<int>("InsertPurchseOrder", param, tran, commandType: CommandType.StoredProcedure);

                        purchaseOrder.PurchaseItems.ForEach(x => x.PurchaseOrderID = ID);

                        param = new DynamicParameters();
                        foreach(var item in purchaseOrder.PurchaseItems)
                        {
                            param.Add("@PurchaseOrderID", item.PurchaseOrderID);
                            param.Add("@SKUID", item.SKUID);
                            param.Add("@Quantity", item.Quantity);
                            param.Add("@UnitPrice", item.UnitPrice);

                            con.Execute("InsertPurchaseItems", param, tran, commandType: CommandType.StoredProcedure);
                        }

                        param = new DynamicParameters();
                        param.Add("@TransactionType", "INSERT");
                        param.Add("@Description", "Order successfully created. Purchase Order ID: " + ID.ToString());

                        int AuditLogsID = con.ExecuteScalar<int>("InsertAuditLogs", param, tran, commandType: CommandType.StoredProcedure);

                        param = new DynamicParameters();
                        foreach (string txt in auditChanges)
                        {
                            param.Add("@AuditLogsID", AuditLogsID);
                            param.Add("@Details", txt);

                            con.Execute("InsertAuditDetail", param, tran, commandType: CommandType.StoredProcedure);
                        }

                        tran.Commit();
                        result = "Success";
                    }
                    catch(Exception ex)
                    {
                        result = "Error";
                        tran.Rollback();
                    }
                }
                con.Close();
            }
            return result;
        }

        public List<PurchaseItemsDTO> GetPurchaseItems(int PurchaseOrderID)
        {
            List<PurchaseItemsDTO> purchaseItem = new List<PurchaseItemsDTO>();
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@PurchaseOrderID", PurchaseOrderID);

                purchaseItem = SqlMapper.Query<PurchaseItemsDTO>(con, "GetPurchaseItems", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return purchaseItem;
        }

        public string UpdatePurchseOrder(PurchaseOrderDTO purchaseOrder, List<AuditDetailDTO> auditChanges)
        {
            string result = "";
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@ID", purchaseOrder.ID);
                        param.Add("@CustomerID", purchaseOrder.CustomerID);
                        param.Add("@DateOfDeliver", purchaseOrder.DateOfDelivery);
                        param.Add("@Status", purchaseOrder.Status);
                        param.Add("@AmountDue", purchaseOrder.AmountDue);

                        con.Execute("UpdatePurchseOrder", param, tran, commandType: CommandType.StoredProcedure);

                        param = new DynamicParameters();
                        foreach (var item in purchaseOrder.PurchaseItems)
                        {
                            param.Add("@ID", item.ID);
                            param.Add("@PurchaseOrderID", item.PurchaseOrderID);
                            param.Add("@SKUID", item.SKUID);
                            param.Add("@Quantity", item.Quantity);
                            param.Add("@UnitPrice", item.UnitPrice);

                            con.Execute("UpdatePurchaseItems", param, tran, commandType: CommandType.StoredProcedure);
                        }

                        param = new DynamicParameters();
                        param.Add("@TransactionType", "UPDATE");
                        param.Add("@Description", "Order successfully updated. Purchase Order ID: " + purchaseOrder.ID.ToString());

                        int AuditLogsID = con.ExecuteScalar<int>("InsertAuditLogs", param, tran, commandType: CommandType.StoredProcedure);

                        param = new DynamicParameters();
                        foreach (var txt in auditChanges)
                        {
                            param.Add("@AuditLogsID", AuditLogsID);
                            param.Add("@Details", txt.Details);

                            con.Execute("InsertAuditDetail", param, tran, commandType: CommandType.StoredProcedure);
                        }

                        tran.Commit();
                        result = "Success";
                    }
                    catch (Exception ex)
                    {
                        result = "Error";
                        tran.Rollback();
                    }
                }
                con.Close();
            }
            return result;
        }
    }
}
