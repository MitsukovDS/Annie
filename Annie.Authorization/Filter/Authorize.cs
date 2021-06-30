using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Annie.Model;


namespace Annie.Authorization.Filter
{
    /// <see cref="https://www.devtrends.co.uk/blog/dependency-injection-in-action-filters-in-asp.net-core"/>>
    public class Authorize : Attribute, IFilterFactory
    {
        private Roles[] Roles { get; set; }
        private RoleLevels[] RoleLevels { get; set; }

        public Authorize()
        {
        }

        public Authorize(params Roles[] roles)
        {
            Roles = roles;
        }

        public Authorize(params RoleLevels[] roleLevels)
        {
            RoleLevels = roleLevels;
        }

        public Authorize(RoleLevels[] roleLevels, params Roles[] roles)
        {
            Roles = roles;
            RoleLevels = roleLevels;
        }

        // свойство указывает, может ли несколько запросов обрабатываться с тем же экземпляром 
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            //var filter = serviceProvider.GetService<AuthorizeAsyncActionFilter>();
            return new AuthorizeAsyncActionFilter()
            {
                Roles = Roles,
                RoleLevels = RoleLevels
            };
        }
    }
}
