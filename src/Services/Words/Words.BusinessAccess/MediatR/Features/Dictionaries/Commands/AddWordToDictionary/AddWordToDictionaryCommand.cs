using MediatR;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.AddWordToDictionary;

public record AddWordToDictionaryCommand(int WordId) : IRequest<int>;