using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;
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
        public string CreatePaginationResourceUri(IUrlHelper Url, string actionName, ResourceParameters ResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(actionName, new
                    {
                        PageNumber = ResourceParameters.PageNumber - 1,
                        pageSize = ResourceParameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(actionName, new
                    {
                        PageNumber = ResourceParameters.PageNumber + 1,
                        pageSize = ResourceParameters.PageSize,
                    });
                case ResourceUriType.Current:
                default:
                    return Url.Link(actionName, new
                    {
                        PageNumber = ResourceParameters.PageNumber,
                        pageSize = ResourceParameters.PageSize,
                    });
            }
        }

        public IEnumerable<LinkDto> CreateLinksFoPaginations(IUrlHelper Url,string actionName, ResourceParameters ResourceParameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<LinkDto>();
            //self
            links.Add(new LinkDto(CreatePaginationResourceUri(Url, actionName, ResourceParameters, ResourceUriType.Current), "self", "GET"));

            if (hasNext)
            {
                links.Add(new LinkDto(CreatePaginationResourceUri(Url, actionName, ResourceParameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }
            if (hasPrevious)
            {
                links.Add(new LinkDto(CreatePaginationResourceUri(Url, actionName, ResourceParameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }
            return links;
        }


        public IEnumerable<LinkDto> CreateLinksForContoller(IUrlHelper Url, IList<ControllerLink> ActionLinks)
        {
            var links = new List<LinkDto>();

            foreach (var item in ActionLinks)
            {
                links.Add(new LinkDto(Url.Link(item.ActionDescription, item.ActionParams), item.ActionDescription, item.ActionMethod));
            }
            return links;
        }

    }
}
