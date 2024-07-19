﻿using Hangfire.Dashboard.BasicAuthorization;

namespace TaskScheduler.Configuration
{
    public class HangFireDashBoard
    {
        public static BasicAuthAuthorizationFilter[] AuthAuthorizationFilters()
        {
            return
                [
                new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
            {
                SslRedirect = false,
                RequireSsl = false,
                LoginCaseSensitive = true,
                Users = new[] {
                    new BasicAuthAuthorizationUser
                    {
                        Login = "admin",
                        PasswordClear = "admin"
                    }
                }
            }
            )
            ];
        }
    }
}
