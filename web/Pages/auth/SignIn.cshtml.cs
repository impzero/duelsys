using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace web.Pages.auth
{
    public class SignInModel : PageModel
    {
        [BindProperty]
        public Credentials Credentials { get; set; }

        public duelsys.ApplicationLayer.Services.AuthenticationService aService { get; set; }
        public SignInModel(duelsys.ApplicationLayer.Services.AuthenticationService a)
        {
            aService = a;
        }

        public IActionResult OnGet()
        {
            if (User.Identity is null)
                return Page();

            if (User.Identity.IsAuthenticated)
                return RedirectToPage("/");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = aService.Login(Credentials.Email, Credentials.Password);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new(ClaimTypes.Role, user.IsAdmin ? "admin":"player"),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            return RedirectToPage("/Index");
        }
    }

    public class Credentials
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
