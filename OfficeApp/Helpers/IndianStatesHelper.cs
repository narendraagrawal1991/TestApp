using Microsoft.AspNetCore.Mvc.Rendering;

namespace OfficeApp.Helpers
{
    public static class IndianStatesHelper
    {
        private static readonly List<string> States = new()
        {
            "Andhra Pradesh", "Arunachal Pradesh", "Assam", "Bihar", "Chhattisgarh", "Goa",
            "Gujarat", "Haryana", "Himachal Pradesh", "Jharkhand", "Karnataka", "Kerala",
            "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram", "Nagaland",
            "Odisha", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu", "Telangana", "Tripura",
            "Uttar Pradesh", "Uttarakhand", "West Bengal", "Delhi", "Jammu and Kashmir",
            "Ladakh", "Puducherry", "Chandigarh", "Andaman and Nicobar Islands",
            "Dadra and Nagar Haveli and Daman and Diu", "Lakshadweep"
        };

        public static List<SelectListItem> GetIndianStates()
        {
            return States.Select(s => new SelectListItem { Text = s, Value = s }).ToList();
        }
    }
}
