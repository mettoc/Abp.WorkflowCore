using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Abp.BaseDto
{
    public class PagedInputDto : IPagedResultRequest
    {
        [Range(1, AppLtmConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }


		
		//// custom codes 
		
        //// custom codes end


        public PagedInputDto()
        {
            MaxResultCount = AppLtmConsts.DefaultPageSize;
        }
    }
}