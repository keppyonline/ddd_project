using MediatR;
using Simple.Domain.SeedWork;
using Simple.Infrastructure.DomainContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Infrastructure
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, UserContext ctx)
        {
            List<Entity> domainEntities = new List<Entity>();
            if (ctx.user != null)
            {
                domainEntities.Add(ctx.user);
            }

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
