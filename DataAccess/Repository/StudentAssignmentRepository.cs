﻿using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class StudentAssignmentRepository : Repository<StudentAssignment>, IStudentAssignmentRepository
    {
        private readonly ApplicationDbContext dbContext;
        public StudentAssignmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
