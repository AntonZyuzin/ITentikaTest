using ITentikaTest.Domain.Data;
using ITentikaTest.Domain.IRepositories;
using ITentikaTest.Entities;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ITentikaTest.Domain.Repositories
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly AppDbContext _dbContext;
        public IncidentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateIncident(Incident newIncident)
        {
            _dbContext.Incidents.Add(newIncident);
            _dbContext.SaveChanges();
        }

        public List<Incident> GetIncidents(string? sortProperty = null, int? page = null, int? limit = null)
        {


            var query = _dbContext.Incidents.AsQueryable();

            if (sortProperty != null)
            {
                query = query.OrderBy(sortProperty, ListSortDirection.Descending);
            }

            if (page != null && limit != null)
            {
                if (page == 0)
                {
                    page = 1;

                }
                if (limit == 0)
                {
                    limit = int.MaxValue;
                }
                var skip = (page - 1) * limit;
                query = query.Skip(skip.Value).Take(limit.Value);
            }

            return query.ToList();
        }
    }

    public static class Extansions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortProperty, ListSortDirection sortOrder)
        {
            var type = typeof(T);
            var property = type.GetProperty(sortProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                return source;

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var methodName = sortOrder == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
