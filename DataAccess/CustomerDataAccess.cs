using DataAccess.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DataAccess
{
    public class CustomerDataAccess : ICustomerDataAccess
    {
        public List<CustomerDTO> GetCustomerList()
        {
            List<CustomerDTO> customer = new List<CustomerDTO>();
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                customer = SqlMapper.Query<CustomerDTO>(con, "GetCustomerList", commandType: CommandType.StoredProcedure).ToList();
            }

            return customer;
        }

        public string InsertCustomer(string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges)
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
                        param.Add("@FirstName", FirstName);
                        param.Add("@LastName", LastName);
                        param.Add("@MobileNumber", MobileNumber);
                        param.Add("@City", City);
                        param.Add("@IsActive", IsActive);

                        int ID = con.ExecuteScalar<int>("InsertCustomer", param, tran, commandType: CommandType.StoredProcedure);

                        if(ID >= 1)
                        {
                            result = "Success";

                            param = new DynamicParameters();
                            param.Add("@TransactionType", "INSERT");
                            param.Add("@Description", "Customer successfully created. Customer ID: " + ID.ToString());

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
                            result = "Exists";
                        }
                        tran.Commit();
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

        public string UpdateCustomer(int ID, string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges)
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
                        param.Add("@FirstName", FirstName);
                        param.Add("@LastName", LastName);
                        param.Add("@MobileNumber", MobileNumber);
                        param.Add("@City", City);
                        param.Add("@IsActive", IsActive);

                        int affectedRow = con.Execute("UpdateCustomer", param, tran, commandType: CommandType.StoredProcedure);
                        
                        if (affectedRow >= 1)
                        {
                            result = "Success";

                            param = new DynamicParameters();
                            param.Add("@TransactionType", "UPDATE");
                            param.Add("@Description", "Customer successfully updated. Customer ID: " + ID.ToString());

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
                            result = "Exists";
                        }
                        tran.Commit();
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

        public List<CustomerDTO> SearchCustomer(string Keyword)
        {
            List<CustomerDTO> customer = new List<CustomerDTO>();
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", Keyword);
                customer = SqlMapper.Query<CustomerDTO>(con, "SearchCustomer", param, commandType: CommandType.StoredProcedure).ToList();
            }

            return customer;
        }
    }
}
