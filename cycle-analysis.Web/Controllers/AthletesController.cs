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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using cycle_analysis.Domain.Athlete;
    using cycle_analysis.Domain.Athlete.Dto;
    using cycle_analysis.Domain.Error;
    using cycle_analysis.Domain.Infrastructure;
    using cycle_analysis.Web.Infrastructure.Core;

    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/athletes")]
    public class AthletesController : ApiControllerBase
    {
        private readonly IAthleteRepository _athletesRepository;

        public AthletesController(IAthleteRepository athletesRepository, IErrorRepository _errorsRepository, IUnitOfWork _unitOfWork)
        : base(_errorsRepository, _unitOfWork)
        {
            _athletesRepository = athletesRepository;
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, AthleteDto athleteDto)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newAthleteDto = _athletesRepository.Add(athleteDto);
                    response = request.CreateResponse(HttpStatusCode.Created, newAthleteDto);
                }

                return response;
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var athleteDto = _athletesRepository.GetSingle(id);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, athleteDto);

                return response;
            });
        }

        [MimeMultipart]
        [Route("images/upload")]
        public HttpResponseMessage Post(HttpRequestMessage request, int athleteId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                var athlete = _athletesRepository.GetSingle(athleteId);
                if (athlete == null)
                    response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid athlete.");
                else
                {
                    var uploadPath = HttpContext.Current.Server.MapPath("~/Content/images/athletes");

                    var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

                    // read the MIME multipart asynchronously 
                    Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

                    var localFileName = multipartFormDataStreamProvider
                        .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

                    // create response
                    FileUploadResult fileUploadResult = new FileUploadResult
                    {
                        LocalFilePath = localFileName,

                        FileName = Path.GetFileName(localFileName),

                        FileLength = new FileInfo(localFileName).Length
                    };

                    // update database
                    athlete.Image = fileUploadResult.FileName;
                    var newAthleteDto = _athletesRepository.Edit(athlete);

                    response = request.CreateResponse(HttpStatusCode.OK, newAthleteDto);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, AthleteDto athleteDto)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response;

                if (!ModelState.IsValid)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    _athletesRepository.Edit(athleteDto);
                    response = request.CreateResponse(HttpStatusCode.OK, athleteDto);
                }

                return response;
            });
        }

        [Route("{page:int=0}/{pageSize=3}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int? page, int? pageSize, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                List<AthleteDto> athletes;
                int totalAthletes;

                if (!string.IsNullOrEmpty(filter))
                {
                    athletes = _athletesRepository.GetAllByFilter(filter, currentPage, currentPageSize);
                    totalAthletes = _athletesRepository.Count(filter);
                }
                else
                {
                    athletes = _athletesRepository.GetAll(currentPage, currentPageSize);
                    totalAthletes = _athletesRepository.Count();
                }

                PaginationSet<AthleteDto> pagedSet = new PaginationSet<AthleteDto>()
                {
                    Page = currentPage,
                    TotalCount = totalAthletes,
                    TotalPages = (int)Math.Ceiling((decimal)totalAthletes / currentPageSize),
                    Items = athletes
                };

                HttpResponseMessage response = request.CreateResponse<PaginationSet<AthleteDto>>(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }
    }
}