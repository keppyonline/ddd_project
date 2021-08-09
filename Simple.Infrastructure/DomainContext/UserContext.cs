using MediatR;
using Npgsql;
using NpgsqlTypes;
using Simple.Application.Configuration.Data;
using Simple.Domain.SeedWork;
using Simple.Domain.Users;
using Simple.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Infrastructure.DomainContext
{
    public class UserContext : IDBContext, IUnitOfWork
    {
        private readonly IMediator mediator;
        private readonly ISqlConnectionFactory sqlConnectionFactory;
        private int stackCounter = 0;
        private int returnVal = 0;
        private bool disposedValue;

        public UserContext(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            this.mediator = mediator;
            this.sqlConnectionFactory = sqlConnectionFactory;
        }
        public User user { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await this.mediator.DispatchDomainEventsAsync(this).ConfigureAwait(false);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            //change using store procedure
            var conn = this.sqlConnectionFactory.GetOpenConnection();
            var result = 0;
            using (var tr = conn.BeginTransaction())
            {
                try
                {
                    // store product
                    using (var cmd = new NpgsqlCommand("INSERT INTO public.web_user(id, first_name, last_name, email) VALUES(@id, @first_name, @last_name, @email)", (NpgsqlConnection)conn))
                    {
                        cmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlDbType.Uuid) { Value = this.user.Id.Value });
                        cmd.Parameters.Add(new NpgsqlParameter("@first_name", NpgsqlDbType.Varchar) { Value = user.FirstName });
                        cmd.Parameters.Add(new NpgsqlParameter("@last_name", NpgsqlDbType.Varchar) { Value = user.LastName });
                        cmd.Parameters.Add(new NpgsqlParameter("@email", NpgsqlDbType.Varchar) { Value = user.Email });

                        result = await cmd.ExecuteNonQueryAsync();
                    }

                    //commit
                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    throw ex;
                }
                return result;
            }

        }
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    //TBD
                }
            }
        }

    }
}
