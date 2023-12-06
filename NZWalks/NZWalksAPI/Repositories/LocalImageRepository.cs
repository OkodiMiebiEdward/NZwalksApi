using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IHttpContextAccessor _httpContext;
        private readonly NzWalksDbContext _dbContext;

        public LocalImageRepository(IWebHostEnvironment webHost, IHttpContextAccessor httpContext,NzWalksDbContext dbContext)
        {
            _webHost = webHost;
            _httpContext = httpContext;
            _dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path
                .Combine(_webHost.ContentRootPath, "Images",$"{image.FileName}{image.FileExtension}");

            //Upload image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);
            var urlFilePath = $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}{_httpContext.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            //Add image to the image table
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }
    }
}
