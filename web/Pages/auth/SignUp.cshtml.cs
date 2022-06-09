using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AuthenticationService = duelsys.ApplicationLayer.Services.AuthenticationService;

namespace web.Pages.auth
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public RegisterCredentials Credentials { get; set; }
        public duelsys.ApplicationLayer.Services.AuthenticationService aService { get; set; }

        public SignUpModel(AuthenticationService a)
        {
            aService = a;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid) return Page();
            try
            {
                aService.Register(Credentials.FirstName, Credentials.LastName, Credentials.Email, Credentials.Password);

                var user = aService.Login(Credentials.Email, Credentials.Password);
                var claims = new List<Claim>
                {
                new(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new(ClaimTypes.Role, user.IsAdmin ? "admin":"player"),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            }
            catch (Exception)
            {
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }

    public class RegisterCredentials
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
