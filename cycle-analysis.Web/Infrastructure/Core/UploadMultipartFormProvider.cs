﻿namespace cycle_analysis.Web.Infrastructure.Core
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class UploadMultipartFormProvider : MultipartFormDataStreamProvider
    {
        public UploadMultipartFormProvider(string rootPath) : base(rootPath) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
            {
            if (headers != null &&
                headers.ContentDisposition != null)
                {
                return headers
                    .ContentDisposition
                    .FileName.TrimEnd('"').TrimStart('"');
                }

            return base.GetLocalFileName(headers);
            }
    }
}