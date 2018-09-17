using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using static ECommerce.Models.Helper.MailHelper;
namespace ECommerce.Models.Helper
{
    public class UserHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static ECommerceContext db = new ECommerceContext();

        public static bool DeleteUser(string userName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser userASP = userManager.FindByEmail(userName);
            if (userASP==null)
            {
                return false;
            }
            return userManager.Delete(userASP).Succeeded;
        }

        public static bool UpdateUserName(string currentUserName,string newUserName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            ApplicationUser userASP = userManager.FindByEmail(currentUserName);
            if (userASP == null)
            {
                return false;
            }
            userASP.UserName = userASP.Email = newUserName;
            return userManager.Update(userASP).Succeeded;
        }
        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

        public static void CheckSupperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassword"];
            var UserASP = userManager.FindByEmail(email);
            if (UserASP == null)
            {
                CreateUserASP(email, "Admin", password);
                return;
            }
            userManager.AddToRole(UserASP.Id, "Admin");
        }
        public static void CreateUserASP(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var UserASP = new ApplicationUser()
            {
                Email = email,
                UserName = email
            };
            userManager.Create(UserASP);
            userManager.AddToRole(UserASP.Id, roleName);
        }
        public static void CreateUserASP(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var UserASP = new ApplicationUser()
            {
                Email = email,
                UserName = email
            };
            userManager.Create(UserASP, password);
            userManager.AddToRole(UserASP.Id, roleName);
        }

        public static async Task PasswordRecovery(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var UserASP = userManager.FindByEmail(email);
            if (UserASP == null)
            {
                return;
            }
            var user = db.Users.Where(tp => tp.UserName == email).FirstOrDefault();
            if (user == null)
            {
                return;
            }
            var random = new Random();
            var newPassword = $"{user.FirstName.Trim().Substring(0, 1).ToUpper()}{user.LastName.Trim().ToLower()}{random.Next(10000):04}*";

            userManager.RemovePassword(UserASP.Id);
            userManager.AddPassword(UserASP.Id, newPassword);

            var subject = "Taxes Password Recovery";
            var body = $"<h1>{subject}</h1><p>Your new Password: <strong>{newPassword}</strong></p>"
                + "<p>Please change it for one,that you remeber easyly</p>";
            await SendMail(email, subject, body);
        }
        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }
}