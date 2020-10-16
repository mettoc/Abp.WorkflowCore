using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.UI.Inputs
{
    [Serializable]
    [InputType("SELECT_USERS")]
    public class SelectUserInputType : InputTypeBase
    {
        public SelectUserInputType()
        {

        }

        public SelectUserInputType(IValueValidator validator)
            : base(validator)
        {
        }
    }
}
