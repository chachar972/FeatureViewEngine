using System.Web.Mvc;
using EPiServer.Web.Mvc;

namespace Alloy.Features.StartPage
{
    public class HeadingBlockController : BlockController<HeadingBlock>
    {
        public override ActionResult Index(HeadingBlock currentBlock)
        {
            return PartialView("HeadingBlock", currentBlock);
        }
    }
}
