using System.Text.Json;
using AutoMapper;
using ProductCatalogService.Dtos;

namespace ProductCatalogService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.Customer:
                    Console.WriteLine($"User sign up! Now do something else here, maybe send welcome email. 🔥🔥🔥");
                    break;

                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("Determining event...");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Role)
            {
                case "Customer":
                    Console.WriteLine("User created with Customer role...");
                    return EventType.Customer;
                case "Admin":
                    Console.WriteLine("User created with Admin role...");
                    return EventType.Admin;
                default:
                    Console.WriteLine("Could not determine User type...");
                    return EventType.Undetermined;

            }

        }

        enum EventType
        {
            Customer,
            Admin,
            Undetermined,
        }
    }
}
