﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.BL.Models
{
    public class Documents
    {
        public string Filename { get; set; }
        public string ContentId { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string Disposition { get; set; }
    }

    public enum Disposition
    {
        inline,
        attachment  
    }
}
