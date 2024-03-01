using Microsoft.Azure.Cosmos;

namespace Claims.ActionModels.Responses.CoverResponses
{
    public class DeleteCoverResponse : ResponseBase
    {
        public ItemResponse<Cover> ItemResponseCover { get; set; }
    }
}
