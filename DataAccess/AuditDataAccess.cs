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
    public class AuditDataAccess : IAuditDataAccess
    {
        public List<AuditLogsDTO> GetAuditLogs()
        {
            List<AuditLogsDTO> auditLogs = new List<AuditLogsDTO>();
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                auditLogs = SqlMapper.Query<AuditLogsDTO>(con, "GetAuditLogs", commandType: CommandType.StoredProcedure).ToList();
            }
            return auditLogs;
        }

        public List<AuditDetailDTO> GetAuditDetail(int AuditLogsID)
        {
            List<AuditDetailDTO> auditDetail = new List<AuditDetailDTO>();
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString()))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AuditLogsID", AuditLogsID);

                auditDetail = SqlMapper.Query<AuditDetailDTO>(con, "GetAuditDetail", param, commandType: CommandType.StoredProcedure).ToList();
            }
            return auditDetail;
        }
    }
}
