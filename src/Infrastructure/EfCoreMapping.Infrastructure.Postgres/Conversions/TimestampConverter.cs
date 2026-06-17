using EfCoreMapping.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EfCoreMapping.Infrastructure.Postgres.Conversions;

public class TimestampConverter() : ValueConverter<Timestamp, DateTime>(
    ts => ts.Value,
    dt => new Timestamp(DateTime.SpecifyKind(dt, DateTimeKind.Utc)));