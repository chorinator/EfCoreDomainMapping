using EfCoreMapping.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EfCoreMapping.Infrastructure.Postgres.Conversions;

public class TransferIdConverter() : ValueConverter<TransferId, Guid>(
    id => id.Id,
    value => new TransferId(value));