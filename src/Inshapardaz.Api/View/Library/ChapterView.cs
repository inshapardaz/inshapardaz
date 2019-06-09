﻿using System.Collections.Generic;

namespace Inshapardaz.Api.View.Library
{
    public class ChapterView : LinkBasedView
    {
        public int Id { get; set; }

        public uint ChapterNumber { get; set; }

        public string Title { get; set; }

        public int BookId { get; set; }
    }
}
