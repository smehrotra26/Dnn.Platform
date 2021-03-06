﻿// 
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 
using System;
using System.Runtime.Serialization;

using DotNetNuke.Services.Exceptions;

namespace DotNetNuke.Services.FileSystem
{
    [Serializable]
    public class InvalidFileContentException : Exception
    {
        public InvalidFileContentException()
        {
        }

        public InvalidFileContentException(string message)
            : base(message)
        {
        }

        public InvalidFileContentException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public InvalidFileContentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
