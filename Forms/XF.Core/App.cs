using Xamarin.Forms;
using XF.Core.Bootstrap;

namespace XF.Core
{
    public class AppCore : Application
    {
        public AppCore()
        {
            var bootstrapper = new Bootstrapper(this);
        }

    }
}
