using Abp.Runtime.Validation;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.UI.Inputs
{

    [Serializable]
    [InputType("SELECT_ROLES")]
    public class SelectRoleInputType : InputTypeBase
    {
        public SelectRoleInputType()
        {

        }

        public SelectRoleInputType(IValueValidator validator)
            : base(validator)
        {
        }
    }
}
