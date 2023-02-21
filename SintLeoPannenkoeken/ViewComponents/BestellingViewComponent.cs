using Microsoft.AspNetCore.Mvc;
using SintLeoPannenkoeken.ViewModels.Bestellingen;

namespace SintLeoPannenkoeken.ViewComponents
{
    public class BestellingViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BestellingFormViewModel viewModel)
        {
            return View("Form", viewModel);
        }
    }
}
