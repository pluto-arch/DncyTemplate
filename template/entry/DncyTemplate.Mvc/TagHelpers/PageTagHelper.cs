using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DncyTemplate.Mvc.TagHelpers
{

    [HtmlTargetElement(Attributes = "pager")]
    [HtmlTargetElement(Attributes = "pager,page-no")]
    [HtmlTargetElement(Attributes = "pager,page-size")]
    [HtmlTargetElement(Attributes = "pager,page-totalPage")]
    [HtmlTargetElement(Attributes = "pager,page-url")]
    public class PageTagHelper : TagHelper
    {

        private IUrlHelperFactory _urlHelperFactory;

        public PageTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("page-no")]
        public int PageNo { get; set; }

        [HtmlAttributeName("page-size")]
        public int PageSize { get; set; }

        [HtmlAttributeName("page-total-page")]
        public int TotalPage { get; set; }

        [HtmlAttributeName("page-url")]
        public int Url { get; set; }


        /// <inheritdoc />
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {




            await base.ProcessAsync(context, output);
        }
    }
}