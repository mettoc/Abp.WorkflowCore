using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowDemo.Users.Dto
{
    public class SearchUserInput
    {
        public string  Keyword { get; set; }
    }

    public class SearchUserListInput: SearchUserInput
    {
        public int MaxCount { get; set; } = 10;
        public IEnumerable<long> UserIds { get; set; } = new List<long>();
    }
}
