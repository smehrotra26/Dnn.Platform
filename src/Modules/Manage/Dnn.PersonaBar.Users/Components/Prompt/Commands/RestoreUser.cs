﻿using System.Collections.Generic;
using System.Text;
using Dnn.PersonaBar.Library.Prompt;
using Dnn.PersonaBar.Library.Prompt.Attributes;
using Dnn.PersonaBar.Library.Prompt.Models;
using Dnn.PersonaBar.Users.Components.Prompt.Models;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace Dnn.PersonaBar.Users.Components.Prompt.Commands
{
    [ConsoleCommand("restore-user", "Recovers a user that has previously been deleted or 'unregistered'", new[] { "id" })]
    public class RestoreUser : ConsoleCommandBase
    {

        private const string FlagId = "id";
        public int? UserId { get; private set; }

        public override void Init(string[] args, PortalSettings portalSettings, UserInfo userInfo, int activeTabId)
        {
            base.Init(args, portalSettings, userInfo, activeTabId);
            var sbErrors = new StringBuilder();

            if (HasFlag(FlagId))
            {
                var tmpId = 0;
                if (int.TryParse(Flag(FlagId), out tmpId))
                    UserId = tmpId;
            }
            else
            {
                var tmpId = 0;
                if (args.Length == 2 && int.TryParse(args[1], out tmpId))
                {
                    UserId = tmpId;
                }
            }

            if (!UserId.HasValue)
            {
                sbErrors.AppendFormat("You must specify a valid numeric User ID using the --{0} flag or by passing it as the first argument; ", FlagId);
            }

            ValidationMessage = sbErrors.ToString();
        }

        public override ConsoleResultModel Run()
        {
            var lst = new List<UserModel>();
            ConsoleErrorResultModel errorResultModel;
            UserInfo userInfo;
            if ((errorResultModel = Utilities.ValidateUser(UserId, PortalSettings, User, out userInfo)) != null) return errorResultModel;

            if (!userInfo.IsDeleted)
                return new ConsoleResultModel("This user has not been deleted. Nothing to restore.");

            if (!UserController.RestoreUser(ref userInfo))
                return new ConsoleErrorResultModel("The system was unable to restore the user");

            var restoredUser = UserController.GetUserById(PortalId, userInfo.UserID);
            lst.Add(new UserModel(restoredUser));
            return new ConsoleResultModel("Successfully recovered the user.") { Data = lst };
        }
    }
}