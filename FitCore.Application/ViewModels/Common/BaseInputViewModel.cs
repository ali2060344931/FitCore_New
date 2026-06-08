using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace FitCore.Application.ViewModels.Common
{
    public class BaseInputViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        public string LabelName { get; set; }
        public string Placeholder { get; set; }
        public string Value { get; set; }
        public int Col { get; set; } = 12;

        public string ValidationMessage { get; set; }

        // اضافه برای آینده
        public bool Disabled { get; set; }
        public bool ReadOnly { get; set; }

        public bool ValueBool { get; set; }

        public List<SelectListItem> Items { get; set; }
        public string SelectedValue { get; set; }
    }
}

