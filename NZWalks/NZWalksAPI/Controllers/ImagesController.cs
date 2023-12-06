using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;

        public ImagesController(IMapper mapper, IImageRepository imageRepository)
        {
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        //api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if(ModelState.IsValid)
            {
                //convert the dto to a domain model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };
                //use repository
                await _imageRepository.Upload(imageDomainModel);
                return Ok(_mapper.Map<ImageUploadRequestDto>(imageDomainModel));
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
           
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName),StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("file", "Unsupported File Extension");
            }

            if(request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size More Than 10MB");
            }
        }
    }
}
