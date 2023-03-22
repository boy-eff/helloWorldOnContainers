using MediatR;
using Words.BusinessAccess.Dtos;

namespace Words.BusinessAccess.MediatR.Features.Dictionaries.Queries.GetDictionary;

public record GetDictionaryByUserIdQuery(int UserId) : IRequest<IEnumerable<WordDto>>;