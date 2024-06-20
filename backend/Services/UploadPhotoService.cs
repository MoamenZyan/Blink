using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;


// Service for uploading photos to s3 bucket
public class UploadPhotoService
{
    private Guid uuid = Guid.NewGuid();
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<Story> _storyRepository;

    public UploadPhotoService(IRepository<User> userRepository,
                                IRepository<Post> postRepository,
                                IRepository<Story> storyRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
        _storyRepository = storyRepository;
    }

    // Upload photo to s3 bucket for user
    public async Task<(bool, User?)> ProfilePhotoUpload(IFormFile photo, int userId)
    {
        // AWS S3 Configurations
        DotNetEnv.Env.Load();
        var accessKey = DotNetEnv.Env.GetString("AWS_ACCESS_KEY");
        var secretKey = DotNetEnv.Env.GetString("AWS_SECRET_KEY");
        var region = RegionEndpoint.EUNorth1;
        var bucketName = "blink-blog";
        using (var client = new AmazonS3Client(accessKey, secretKey, region))
        {
            try
            {
                var transfer = new TransferUtility(client);
                var photoName = uuid.ToString();
                await transfer.UploadAsync(photo.OpenReadStream(), bucketName, $"profilePhoto/{photoName}");
                var user = await _userRepository.GetByIdAsync(userId);
                if (user is null)
                    return (false, null);
                user.Photo = $"https://blink-blog.s3.eu-north-1.amazonaws.com/profilePhoto/{photoName}";
                await _userRepository.Update(user);
                return (true, user);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return (false, null);
            }
        }
    }


    // Upload photo to s3 bucket for a post
    public async Task<bool> PostPhotoUpload(IFormFile photo, string photoName)
    {
        // AWS S3 Configurations
        DotNetEnv.Env.Load();
        var accessKey = DotNetEnv.Env.GetString("AWS_ACCESS_KEY");
        var secretKey = DotNetEnv.Env.GetString("AWS_SECRET_KEY");
        var region = RegionEndpoint.EUNorth1;
        var bucketName = "blink-blog";
        using (var client = new AmazonS3Client(accessKey, secretKey, region))
        {
            try
            {
                var transfer = new TransferUtility(client);
                await transfer.UploadAsync(photo.OpenReadStream(), bucketName, $"postPhoto/{photoName}");
                return true;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return false;
            }
        }
    }


    // Upload photo to s3 bucket for a story
    public async Task<bool> StoryPhotoUpload(IFormFile photo, string photoName)
    {
        // AWS S3 Configurations
        DotNetEnv.Env.Load();
        var accessKey = DotNetEnv.Env.GetString("AWS_ACCESS_KEY");
        var secretKey = DotNetEnv.Env.GetString("AWS_SECRET_KEY");
        var region = RegionEndpoint.EUNorth1;
        var bucketName = "blink-blog";
        using (var client = new AmazonS3Client(accessKey, secretKey, region))
        {
            try
            {
                var transfer = new TransferUtility(client);
                await transfer.UploadAsync(photo.OpenReadStream(), bucketName, $"storyPhoto/{photoName}");
                return true;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return false;
            }
        }
    }
}