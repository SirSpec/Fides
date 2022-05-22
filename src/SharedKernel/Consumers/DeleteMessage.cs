using SharedKernel.QueryObjects.Models;

namespace SharedKernel.Consumers;

public record DeleteMessage(OrderedItem OrderedItem);
