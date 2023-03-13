using MediatR;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Commands.RemoveWordFromDictionary;

public record RemoveWordFromDictionaryCommand(int WordId) : IRequest<int>;