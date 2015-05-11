﻿using Models.Models;

namespace Models.Interfaces
{
    public interface IClasssRepository : IRepository<Classs>
    {
        Classs FindByStudent(string studentId);

        Classs FindByMark(int markId);
    }
}
