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
    public class SKUsDataAccess : ISKUsDataAccess
    {
        public List<SKUsDTO> GetSKUsList()
        {
            List<SKUsDTO> SKUs = new List<SKUsDTO>();
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                SKUs = SqlMapper.Query<SKUsDTO>(con, "GetSKUsList", commandType: CommandType.StoredProcedure).ToList();
            }

            return SKUs;
        }

        public string InsertSKUs(string Name, string Code, double UnitPrice, bool IsActive, string ImagePath, string[] auditChanges)
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
                        param.Add("@Name", Name);
                        param.Add("@Code", Code);
                        param.Add("@UnitPrice", UnitPrice);
                        param.Add("@IsActive", IsActive);
                        param.Add("@ImagePath", ImagePath);

                        int ID = con.ExecuteScalar<int>("InsertSKUs", param, tran, commandType: CommandType.StoredProcedure);
                        if(ID >= 1)
                        {
                            result = "Success";

                            param = new DynamicParameters();
                            param.Add("@TransactionType", "INSERT");
                            param.Add("@Description", "SKU successfully created. ID: " + ID.ToString());

                            int AuditLogsID = con.ExecuteScalar<int>("InsertAuditLogs", param, tran, commandType: CommandType.StoredProcedure);

                            param = new DynamicParameters();
                            foreach (string txt in auditChanges)
                            {
                                param.Add("@AuditLogsID", AuditLogsID);
                                param.Add("@Details", txt);

                                con.Execute("InsertAuditDetail", param, tran, commandType: CommandType.StoredProcedure);
                            }
                        }
                        else
                        {
                            result = "Exist";
                        }
                        tran.Commit();
                    }
                    catch(Exception ex)
                    {
                        result = "Error";
                        tran.Rollback();
                    }
                }
            }
            return result;
        }

        public string UpdateSKUs(int ID, string Name, string Code, double UnitPrice, bool IsActive, string ImagePath, string[] auditChanges)
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
                        param.Add("@ID", ID);
                        param.Add("@Name", Name);
                        param.Add("@Code", Code);
                        param.Add("@UnitPrice", UnitPrice);
                        param.Add("@IsActive", IsActive);
                        param.Add("@ImagePath", ImagePath);

                        int affectedRows = con.Execute("UpdateSKUs", param, tran, commandType: CommandType.StoredProcedure);
                        if (affectedRows >= 1)
                        {
                            result = "Success";

                            param = new DynamicParameters();
                            param.Add("@TransactionType", "INSERT");
                            param.Add("@Description", "SKU successfully updated. ID: " + ID.ToString());

                            int AuditLogsID = con.ExecuteScalar<int>("InsertAuditLogs", param, tran, commandType: CommandType.StoredProcedure);

                            param = new DynamicParameters();
                            foreach (string txt in auditChanges)
                            {
                                param.Add("@AuditLogsID", AuditLogsID);
                                param.Add("@Details", txt);

                                con.Execute("InsertAuditDetail", param, tran, commandType: CommandType.StoredProcedure);
                            }
                        }
                        else
                        {
                            result = "Exist";
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        result = "Error";
                        tran.Rollback();
                    }
                }
            }
            return result;
        }
    }
}
