﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WordModel.cs" company="Inshapardaz">
//     Muhammad Umer Farooq
// </copyright>
// <summary>
// Defines the WordModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inshapardaz.Api.View
{
    public class WordView
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        public string TitleWithMovements { get; set; }

        public string Description { get; set; }

        public string Attributes { get; set; }

        public int AttributeValue { get; set; }

        public string Language { get; set; }

        public int LanguageId { get; set; }

        public string Pronunciation { get; set; }

        public IEnumerable<LinkView> Links { get; set; }
    }
}