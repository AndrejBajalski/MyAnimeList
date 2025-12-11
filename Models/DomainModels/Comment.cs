using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DomainModels
{
    public class Comment : BaseEntity
    {
        public Guid ReviewId { get; set; }
        public string Text { get; set; }

        public Comment(Guid ReviewId, string Text)
        {
            this.ReviewId = ReviewId;
            this.Text = Text;   
        }
    }
}
