using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.Helpers
{
    public enum TransformationSourceSchemaValidation
    {
        ValidateSourceSchema = 0,
        ValidateSourceSchemaIfKnown = 1,
        DoNotValidateSourceSchema = 2
    }
}
