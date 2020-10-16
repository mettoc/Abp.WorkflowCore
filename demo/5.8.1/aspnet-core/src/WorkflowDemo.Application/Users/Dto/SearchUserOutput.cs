using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowDemo.Users.Dto
{
   public class SearchUserOutput:EntityDto<long>
    {
        public string  UserName { get; set; }
        public string FullName { get; set; }
        public string  HeadImage { get; set; }

    }
}
