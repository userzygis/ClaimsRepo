﻿namespace Claims.Domain.ActionModels.Requests.ClaimsRequests
{
    public class DeleteClaimRequest
    {
        public string Id { get; set; }
        public string? Reason { get; set; }
    }
}
