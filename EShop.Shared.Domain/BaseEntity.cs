using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Shared.Domain
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreationDate { get; private set; }
        public bool IsDelete { get; private set; }
        public BaseEntity()
        {
            CreationDate = DateTime.Now;
        }

        public void SoftDelete()
        {
            if (IsDelete)
                return;

            IsDelete = true;
        }

        public void Restore()
        {
            if (!IsDelete)
                return;

            IsDelete = false;
        }
    }
}
