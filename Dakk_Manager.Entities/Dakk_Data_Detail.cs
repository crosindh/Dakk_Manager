using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dakk_Manager.Entities
{
    public class Dakk_Data_Detail : BaseEntity
    {
        public int DakkDataDetailId { get; set; }
        public string FilePath { get; set; }

        public string comments { get; set; }

    }
}
