namespace ProductCatalogService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        // when user register, maybe send an email for welcome message and special offers
        // triggerred from ProductCatalogService
        void UserSignUp(string email);
    }
}
