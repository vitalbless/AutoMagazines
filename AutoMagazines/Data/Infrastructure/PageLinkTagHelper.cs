using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using AutoMagazines.Data.ViewModels;

namespace AutoMagazines.Data.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory factory)
        {
            urlHelperFactory = factory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();
        public string PageAction { get; set; }
        public PagingInfo PageModel { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string Class { get; set; }
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //Интерфейс для работы с параметрами тэгов
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder aTag = new TagBuilder("a");
                PageUrlValues["page"] = i;
                aTag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues); //   Index?page=1
                if (PageClassesEnabled)
                {
                    aTag.AddCssClass(PageClass);
                    //Class = (i == PageModel.CurrentPage) ? PageClassSelected : PageClassNormal;
                    //aTag.AddCssClass(Class);
                    aTag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                }
                aTag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(aTag);
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
