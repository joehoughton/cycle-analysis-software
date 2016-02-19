/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Web.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using cycle_analysis.Domain.Athlete;
    using cycle_analysis.Domain.Error;
    using cycle_analysis.Domain.Infrastructure;
    using cycle_analysis.Domain.Session;
    using cycle_analysis.Domain.Session.Dto;
    using cycle_analysis.Web.Infrastructure.Core;

    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/sessions")]
    public class SessionsController : ApiControllerBase
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IAthleteRepository _athleteRepository;

        public SessionsController(ISessionRepository sessionRepository, IAthleteRepository athleteRepository,
        IErrorRepository _errorsRepository, IUnitOfWork _unitOfWork)
        : base(_errorsRepository, _unitOfWork)
        {
            _sessionRepository = sessionRepository;
            _athleteRepository = athleteRepository;
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, HRMFileDto hrmFileDto)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                var athlete = _athleteRepository.GetSingle(hrmFileDto.AthleteId);

                if (athlete == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Athlete");
                }
                else
                {
                    try
                    {
                        _sessionRepository.Add(hrmFileDto);
                        response = request.CreateResponse(HttpStatusCode.Created);
                    }
                    catch (Exception)
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }

                return response;
            });
        }
    }
}