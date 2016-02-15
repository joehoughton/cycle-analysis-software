namespace cycle_analysis.Web.Controllers
{
    using cycle_analysis.Domain.Error;
    using cycle_analysis.Domain.Infrastructure;
    using cycle_analysis.Web.Infrastructure.Core;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using cycle_analysis.Domain.User;
    using cycle_analysis.Services.Abstract;

    // [Authorize(Roles = "Admin")]
    [RoutePrefix("api/sessions")]
    public class SessionController : ApiControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly ICurrentUserProvider _currentUserProvider;

        public SessionController(IErrorRepository _errorsRepository, IUnitOfWork _unitOfWork, IMembershipService membershipService, ICurrentUserProvider currentUserProvider)
        : base(_errorsRepository, _unitOfWork)
        {
            _membershipService = membershipService;
            _currentUserProvider = currentUserProvider;
        }

        [HttpGet]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request)
        {
            var currentUser = _currentUserProvider.Get(); // empty :( 

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.Created);
                return response;
            });
        }
    }
}
