namespace Library.Tests.Services.Base;

public abstract class BaseTest
{
    private const string TestChatDb = "test_chatdb";
    private const string TestUser = "test_user";
    private const string TestAuthType = "test_auth_type";
    
    protected DataContext Context;
    
    protected Mock<IHttpContextAccessor> HttpContextAccessorMock;
    
    protected Mock<IMemoryCache> MemoryCacheMock;
    
    [SetUp]
    public void Setup()
    {
        InitializeMocks();
        Context = CreateInMemoryDbContext();
    }
    
    [TearDown]
    public void TearDown()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
    
    private void InitializeMocks()
    {
        HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
        MemoryCacheMock = new Mock<IMemoryCache>();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, TestUser)
        };
        var identity = new ClaimsIdentity(claims, TestAuthType);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext { User = claimsPrincipal };
        HttpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);
    }
    
    private DataContext CreateInMemoryDbContext()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(TestChatDb)
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        return new DataContext(options);
    }
    
    protected static ResponseDto<T>? GetResultData<T>(IActionResult result)
    {
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var responseDto = okResult?.Value as ResponseDto<T>;

        return responseDto;
    }
}