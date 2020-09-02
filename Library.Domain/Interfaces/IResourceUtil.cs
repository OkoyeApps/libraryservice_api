using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IResourceUtil
    {
        string CreatePaginationResourceUri(IUrlHelper Url,string actionName, ResourceParameters ResourceParameters, ResourceUriType type);
        IEnumerable<LinkDto> CreateLinksFoPaginations(IUrlHelper Url, string actionName, ResourceParameters ResourceParameters, bool hasNext, bool hasPrevious);
        IEnumerable<LinkDto> CreateLinksForContoller(IUrlHelper Url, IList<ControllerLink> ActionLinks);
    }
}
