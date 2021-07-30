using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectTeamNET.Models;
using ProjectTeamNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Npgsql;
using System.Threading.Tasks;

namespace ProjectTeamNET.Repository.Implement
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public DbSet<T> dbset;
        public ProjectDbContext context;
        public BaseRepository(ProjectDbContext context)
        {
            dbset = context.Set<T>();
            this.context = context;
        }

        public async Task<T> Get(object id)
        {
            var result = await dbset.FindAsync(id);
            return result;
        }

        public async Task<int> Delete(object id)
        {
            var result = -1;
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var tmp = await Get(id);
                    dbset.Remove(tmp);
                    result = await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return result;
        }

        public int Create(T obj)
        {
            int result = 0;
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    dbset.Add(obj);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    _ = e.Message;
                    transaction.Rollback();
                    result = -1;
                }
                transaction.Dispose();
            }
            
            result = context.SaveChanges();
            return result;
        }
        public async Task<int> Update(T obj)
        {
            var result = -1;
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    dbset.Update(obj);
                    result = await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    result = -1;
                }
                transaction.Dispose();
            }

            return result;
        }

        public async Task<List<T>> Gets()
        {
            var result = await dbset.ToListAsync();
            return result;
        }

        public List<object> Search(object obj)
        {
            var result = context.Database.GetDbConnection().Query<object>("", obj).ToList();
            return result;
        }
        public async Task<List<M>> Select<M>(string sql)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<M>(sql);
            return query.ToList();
        }
        public async Task<List<M>> Search<M>(string sql, object param)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<M>(sql, param);
            return query.ToList();
        }
        public async Task<M> MenuSearch<M>(string sql, object param)
        {
            var query = await context.Database.GetDbConnection().QueryAsync<M>(sql, param);
            return query.FirstOrDefault();
        }
        public void Delete(T obj)
        {
            dbset.Remove(obj);
        }
        public async Task<List<T1>> Select<T1>(string sql, object param)
        {
            List<T1> rs = new List<T1>();
            try
            {
                rs =(List<T1>) await context.Database.GetDbConnection().QueryAsync<T1>(sql, param);
            }
            catch (Exception e)
            {
                _ = e.Message;
            }
          
            return rs;
        }
        public int Update(string sql, object param)
        {
            var result = 0;
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    result = context.Database.GetDbConnection().Execute(sql, param);
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    _ = e.Message;
                    transaction.Rollback();
                    result = -1;
                }
                transaction.Dispose();
            }
            result = context.SaveChanges();
            return result;
        }      
        public int Update<M>(string sql, object param)
        {
            try
            {
                context.Database.GetDbConnection().Query<M>(sql, param);
            }
            catch(Exception e)
            {
                _ = e.Message; 
                return 0;
            }
            return 1;
        }
        public void Insert<M>(string sql, object param)
        {
            context.Database.GetDbConnection().QueryAsync<M>(sql, param);
        }
        public int Create(List<T> objs)
        {
            var result = -1;
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    dbset.AddRange(objs);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            result = context.SaveChanges();
            return result;
        }
    }
}
