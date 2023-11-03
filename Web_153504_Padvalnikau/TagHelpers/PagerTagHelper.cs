using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Web_153504_Padvalnikau.TagHelpers;

public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContext _context;

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool Admin { get; set; } = false;
        public string Category { get; set; }
        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _context = httpContextAccessor.HttpContext;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination justify-content-center");

            int prev = CurrentPage == 1 ? 1 : CurrentPage - 1;
            ul.InnerHtml.AppendHtml(GeneratePagerItem(prev, "&laquo;", CurrentPage > 1)); 
            for (int i = 1; i <= TotalPages; i++)
            {
                ul.InnerHtml.AppendHtml(GeneratePagerItem(i));
            }

            int next = CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;
            ul.InnerHtml.AppendHtml(GeneratePagerItem(next, "&raquo;", CurrentPage < TotalPages)); 

            output.Content.AppendHtml(ul);
        }

        private TagBuilder GeneratePagerItem(int i, string? value = null, bool isEnabled = true) 
        {
            var li = new TagBuilder("li");
            string liClass = i == CurrentPage ? "page-item active" : "page-item";
            li.AddCssClass(liClass);

            var a = new TagBuilder("a");
            a.AddCssClass("page-link async-request");

            if (isEnabled)
            {
                a.Attributes.Add("href", Admin
                    ? _linkGenerator.GetPathByPage(_context, values: new { pageno = i })
                    : _linkGenerator.GetPathByAction(_context, controller: "Product", action: "Index", values: new { category = Category, pageno = i }));
            }
            else
            {
                li.AddCssClass("disabled"); // Добавляем класс "disabled" для отключенных кнопок
            }

            a.InnerHtml.AppendHtml(value != null ? value : i.ToString());
            li.InnerHtml.AppendHtml(a);

            return li;
        }
    }