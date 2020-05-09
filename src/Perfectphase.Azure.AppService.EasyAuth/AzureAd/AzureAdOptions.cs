using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Perfectphase.Azure.AppService.EasyAuth.AzureAd
{
    public class AzureAdOptions : AuthenticationSchemeOptions
    {
        public AzureAdOptions()
        {
            LoginReturnUrlParameter= AzureAdDefaults.LoginReturnUrlParameter;
            LogoutReturnUriParameter= AzureAdDefaults.LogoutReturnUriParameter;
        }

        /// <summary>
        /// The LoginPath property is used by the handler for the redirection target when handling ChallengeAsync.
        /// The current url which is added to the LoginPath as a query string parameter named by the ReturnUrlParameter. 
        /// Once a request to the LoginPath grants a new SignIn identity, the ReturnUrlParameter value is used to redirect 
        /// the browser back to the original url.
        /// </summary>
        public PathString LoginPath { get; set; }

        /// <summary>
        /// If the LogoutPath is provided the handler then a request to that path will redirect based on the ReturnUrlParameter.
        /// </summary>
        public PathString LogoutPath { get; set; }

        /// <summary>
        /// The AccessDeniedPath property is used by the handler for the redirection target when handling ForbidAsync.
        /// </summary>
        public PathString AccessDeniedPath { get; set; }

        /// <summary>
        /// The ReturnUrlParameter determines the name of the query string parameter which is appended by the handler
        /// when during a Challenge. This is also the query string parameter looked for when a request arrives on the 
        /// login path or logout path, in order to return to the original url after the action is performed.
        /// </summary>
        public string LoginReturnUrlParameter { get; set; }
        public string LogoutReturnUriParameter { get; set; }
        public IList<string> IgnoreClaimTypes { get; set; } = new List<string>();
        public string RoleClaimType { get; set; }
        public string NameClaimType { get; set; }
    }
}
