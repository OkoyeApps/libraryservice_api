using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Interfaces
{
    public interface IResourceUtil
    {
        string CreatePaginationResourceUri(string actionName, ResourceParameters ResourceParameters, ResourceUriType type);
        IEnumerable<LinkDto> CreateLinksFoPaginations( string actionName, ResourceParameters ResourceParameters, bool hasNext, bool hasPrevious);
        IEnumerable<LinkDto> CreateLinksForContoller( IList<ControllerLink> ActionLinks);
    }
}
