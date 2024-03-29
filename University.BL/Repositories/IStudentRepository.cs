﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Models;

namespace University.BL.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<bool> DeleteCheckOnEntity(int id);
    }
}
