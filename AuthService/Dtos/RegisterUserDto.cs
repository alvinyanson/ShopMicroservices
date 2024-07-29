namespace AuthService.Dtos
{
    // This will be used as the object to be passed on Product Catalog Service for asynchronous communication
    public class RegisterUserDto
    {
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
