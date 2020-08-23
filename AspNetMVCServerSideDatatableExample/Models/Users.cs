using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspNetMVCServerSideDatatableExample.Models
{
    public class Users
    {
        public int id { get; set; }
        public Guid RowGuid { get; set; }
        [StringLength(50)]
        public string first_name { get; set; }
        [StringLength(50)]
        public string last_name { get; set; }
        [StringLength(50)]
        public string email { get; set; }
        [StringLength(50)]
        public string gender { get; set; }
        [StringLength(20)]
        public string ip_address { get; set; }
        public bool is_active { get; set; }
        [Column(TypeName = "Date")]
        public DateTime date_time { get; set; }
    }
}