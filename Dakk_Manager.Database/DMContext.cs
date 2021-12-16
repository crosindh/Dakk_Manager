﻿using Dakk_Manager.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dakk_Manager.Database
{
    public class DMContext : DbContext
    {
        public DMContext() : base("DMContext")
        {

        }
        public DbSet<Dakk_Data> Dakk_Data { get; set; }
    }
}