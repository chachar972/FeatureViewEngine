using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alloy.Business;
using Alloy.Models.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Web.Mvc;

namespace Alloy.Features.PageListBlock
{
    public class PageListBlockController : BlockController<Models.Blocks.PageListBlock>
    {
        private ContentLocator contentLocator;
        private IContentLoader contentLoader;
        public PageListBlockController(ContentLocator contentLocator, IContentLoader contentLoader)
        {
            this.contentLocator = contentLocator;
            this.contentLoader = contentLoader;
        }

        public override ActionResult Index(Models.Blocks.PageListBlock currentBlock)
        {
            var pages = FindPages(currentBlock);

            pages = Sort(pages, currentBlock.SortOrder);
            
            if(currentBlock.Count > 0)
            {
                pages = pages.Take(currentBlock.Count);
            }

            var model = new PageListModel(currentBlock)
                {
                    Pages = pages
                };

            ViewData.GetEditHints<PageListModel, Models.Blocks.PageListBlock>()
                .AddConnection(x => x.Heading, x => x.Heading);

            return PartialView(model);
        }

        private IEnumerable<PageData> FindPages(Models.Blocks.PageListBlock currentBlock)
        {
            IEnumerable<PageData> pages;
            var listRoot = currentBlock.Root;
            if (currentBlock.Recursive)
            {
                if (currentBlock.PageTypeFilter != null)
                {
                    pages = contentLocator.FindPagesByPageType(listRoot, true, currentBlock.PageTypeFilter.ID);
                }
                else
                {
                    pages = contentLocator.GetAll<PageData>(listRoot);
                }
            }
            else
            {
                if (currentBlock.PageTypeFilter != null)
                {
                    pages = contentLoader.GetChildren<PageData>(listRoot)
                        .Where(p => p.PageTypeID == currentBlock.PageTypeFilter.ID);
                }
                else
                {
                    pages = contentLoader.GetChildren<PageData>(listRoot);
                }
            }

            if (currentBlock.CategoryFilter != null && currentBlock.CategoryFilter.Any())
            {
                pages = pages.Where(x => x.Category.Intersect(currentBlock.CategoryFilter).Any());
            }
            return pages;
        }

        private IEnumerable<PageData> Sort(IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = new PageDataCollection(pages);
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);
            return asCollection;
        }
    }
}
