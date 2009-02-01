//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a ConsitentCheckException in a Partial Order Plan
//
// Modification Notes:
// Date		Author        	Notes
// -------- ------          -----------------------------------------
// 01/26/09	SMcCarthy		Initial Implementation
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UnityAI.Core.Planning
{
    [Serializable]
    public class ConsistencyCheckException : Exception
    {
        public ConsistencyCheckException()
        {
        }

        public ConsistencyCheckException(string message)
            : base(message)
        {
        }

        public ConsistencyCheckException(string message,
                Exception innerException)
            : base(message, innerException)
        {
        }

        protected ConsistencyCheckException(SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info,
          StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
