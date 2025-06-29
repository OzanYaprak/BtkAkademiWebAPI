﻿using Entities.DataTransferObjects.Manipulations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public record BookDTOForUpdate : BookDTOForManipulation
    {
        [Required]
        public int Id { get; init; }
    }
}