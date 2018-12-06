using System;

namespace cancel.Models
{
    public class SomeService : ITransisionService, IScopedService, ISingletonService
    {
        private readonly Guid _id;

        public SomeService()
        {
            _id = Guid.NewGuid();
        }

        public Guid GetGuid()
        {
            return _id;
        }
    }
}