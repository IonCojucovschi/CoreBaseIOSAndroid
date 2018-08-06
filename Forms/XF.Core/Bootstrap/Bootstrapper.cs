
using XF.Core.Factories;

namespace XF.Core.Bootstrap
{
    public class Bootstrapper : AutoBootstrapper
    {
        private readonly AppCore _application;

        public Bootstrapper(AppCore application)
        {
            _application = application;
        }

        protected override void RegisterPages(IPageFactory pageFactory)
        {
        }
    }
}
