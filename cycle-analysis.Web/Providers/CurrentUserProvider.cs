namespace cycle_analysis.Web.Providers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using cycle_analysis.Domain.User;
    using cycle_analysis.Domain.User.Models;

    public class CurrentUserProvider : ICurrentUserProvider
    {
        public CurrentUserProvider()
        {
            // get the current claims principal from GenericIdentity
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // get the claims values
            var userName = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                           .Select(c => c.Value).SingleOrDefault();

            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                         .Select(c => c.Value).SingleOrDefault();

            CurrentUserDetail = new CurrentUserDetail
            {
                UserId = userId,
                Username = userName
            };
        }

        public CurrentUserDetail Get()
        {
            // get the current claims principal from GenericIdentity
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // get the claims values
            var userName = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                           .Select(c => c.Value).SingleOrDefault();

            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                           .Select(c => c.Value).SingleOrDefault();

            CurrentUserDetail.UserId = userId;
            CurrentUserDetail.Username = userName;

            return CurrentUserDetail;
        }

        public CurrentUserDetail CurrentUserDetail { get; private set; }
    }
}
