using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EndPoint.Site.TagHelpers
{
    [HtmlTargetElement("help", Attributes = "key")]
    public class HelpTagHelper : TagHelper
    {
        public string Key { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "i";

            output.Attributes.SetAttribute("class",
                "fa fa-question-circle help-icon");

            output.Attributes.SetAttribute("onclick",
                $"openHelp('{Key}')");

            output.Attributes.SetAttribute("title",
                "راهنما");

            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
