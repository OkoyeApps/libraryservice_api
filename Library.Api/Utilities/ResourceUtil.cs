using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Api.Utilities
{
    //couldn't find a better way to name this guy
    //so i decided to create a util folder and place it here
    //
    public class ResourceUtil : IResourceUtil
    {
        private readonly IUrlHelper _urlHelper;
        public ResourceUtil(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
        public string CreatePaginationResourceUri(string actionName, ResourceParameters ResourceParameters, ResourceUriType type)
        {

            //var UrlHelper = new UrlHelper()
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link(actionName, new
                    {
                        PageNumber = ResourceParameters.PageNumber - 1,
                        pageSize = ResourceParameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link(actionName, new
                    {
                        PageNumber = ResourceParameters.PageNumber + 1,
                        pageSize = ResourceParameters.PageSize,
                    });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link(actionName, new
                    {
                        PageNumber = ResourceParameters.PageNumber,
                        pageSize = ResourceParameters.PageSize,
                    });
            }
        }

        public IEnumerable<LinkDto> CreateLinksFoPaginations(string actionName, ResourceParameters ResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();
            //self
            links.Add(new LinkDto(CreatePaginationResourceUri(actionName, ResourceParameters, ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                links.Add(new LinkDto(CreatePaginationResourceUri( actionName, ResourceParameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }
            if (hasPrevious)
            {
                links.Add(new LinkDto(CreatePaginationResourceUri( actionName, ResourceParameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }
            return links;
        }


        public IEnumerable<LinkDto> CreateLinksForContoller(IList<ControllerLink> ActionLinks)
        {
            var links = new List<LinkDto>();

            foreach (var item in ActionLinks)
            {
                links.Add(new LinkDto(_urlHelper.Link(item.ActionDescription, item.ActionParams), item.ActionDescription, item.ActionMethod));
            }
            return links;
        }

    }
}
