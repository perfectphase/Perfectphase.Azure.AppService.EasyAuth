using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Sample.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public void OnGet()
        {
        }

        public IActionResult OnGetLogin()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" });
        }

        public IActionResult OnGetLogout()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/"});
        }
    }
}
