﻿using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    [DataContract]
    public enum OperationResult
    {
        Failed,
        Success,
        NotFound,
    }
}
