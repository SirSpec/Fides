using SharedKernel.QueryObjects.Models;

namespace SharedKernel.Consumers;

public record ImportMessage(OrderedItem OrderedItem);
