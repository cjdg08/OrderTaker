using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SKUsDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double UnitPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserID { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
    }
}
