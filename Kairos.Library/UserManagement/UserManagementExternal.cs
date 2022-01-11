using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Kairos.Library.UserManagement
{
    public class UserManagementExternal
    {
        public bool AuthUser(string args_username, string args_password)
        {
            bool result = false;
            string errorMsg;
            try
            {
                if (Membership.ValidateUser(args_username, args_password))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.ToString();
                result = false;
            }
            return result;
        }

        #region MembershipMethod

      public bool ValidateUser(string username, string password)
        {
            //if (UsMan.ValidateUserServices(UsernameService, PasswordService))
            //{
            bool result = false;

            result = Membership.ValidateUser(username, password);
            if (result == true)
            {
                if (System.Web.HttpContext.Current.Session["UserExternalInfo"] == null)
                {
                    MembershipUser userInformation = Membership.GetUser(username);
                    System.Web.HttpContext.Current.Session["UserExternalInfo"] = userInformation;
                }
            }
            return result;
            //}
            //else
            //{ return false; }
        }

        public MembershipUser CreateUser(string username, string password, string email)
        {
            return Membership.CreateUser(username, password, email);
        }

        public MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, out MembershipCreateStatus status)
        {
            return Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, out status);
   
        }

        public MembershipUser CreateUser(string applicationName, string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, string ProviderUserKey, out MembershipCreateStatus status)
        {
            var _provider = Membership.Providers[applicationName];
            return _provider.CreateUser(username, password, email, null, null, isApproved, null, out status);
        }
        public MembershipUserCollection Getall()
        {
            return Membership.GetAllUsers();
        }

        


        public MembershipUser GetUser(string username)
        {
            return Membership.GetUser(username);
        }

        public MembershipUser GetUser(string username, bool userIsOnline)
        {
            return Membership.GetUser(username, userIsOnline);
        }

        public string GetUserNameByEmail(string email)
        {
            return Membership.GetUserNameByEmail(email);
        }

        public void DeleteUser(string username)
        {
            Membership.DeleteUser(username);
        }

        
        public void UpdateUser(MembershipUser user)
        {
            Membership.UpdateUser(user);
        }

        public int GetNumberOfUsersOnline()
        {
            return Membership.GetNumberOfUsersOnline();
        }

        public MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return Membership.GetAllUsers(pageIndex, pageSize, out totalRecords);
        }

        public MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return Membership.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
        }
        #endregion

        #region RoleMethod
        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            Roles.AddUsersToRoles(usernames, roleNames);
        }


        public void CreateRole(string roleName)
        {
            Roles.CreateRole(roleName);
        }
        
        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return Roles.DeleteRole(roleName, throwOnPopulatedRole);
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return Roles.FindUsersInRole(roleName, usernameToMatch);
        }

        public string[] GetAllRoles()
        {
            return Roles.GetAllRoles();
        }

        public string[] GetRolesForUser(string username)
        {
            return Roles.GetRolesForUser(username);
        }

        public string[] GetUsersInRole(string roleName)
        {
            return Roles.GetUsersInRole(roleName);
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return Roles.IsUserInRole(username, roleName);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            Roles.RemoveUsersFromRoles(usernames, roleNames);
        }

        public bool RoleExists(string roleName)
        {
            return Roles.RoleExists(roleName);
        }
        #endregion
    }
}
