using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Alloy.Features.StartPage
{
    [ContentType(DisplayName = "HeadingBlock", GUID = "6544b87d-067b-4872-9b4a-00c97a4f908e", Description = "")]
    public class HeadingBlock : BlockData
    {

        [CultureSpecific]
        [Display(
            Name = "Name",
            Description = "Name field's description",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Heading { get; set; }

    }
}