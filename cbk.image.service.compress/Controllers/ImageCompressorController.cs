using cbk.image.service.compress.Service;
using Microsoft.AspNetCore.Mvc;

namespace cbk.image.service.compress.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageCompressorController : ControllerBase
    {
        private IImageCompressorService _imageCompressorService;

        public ImageCompressorController(IImageCompressorService imageCompressorService)
        {
            _imageCompressorService = imageCompressorService;
        }



    }
}
