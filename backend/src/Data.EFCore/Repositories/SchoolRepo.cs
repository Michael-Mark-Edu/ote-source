using Amazon.Lambda.Core;
using OTE.Data.EFCore.Contexts;
using OTE.Data.EFCore.Entities;

namespace OTE.Data.EFCore.Repositories;

/// <summary>`SchoolEntity` implementation of `AbstractRepo`.</summary>
/// <param name="context">The `OteContext` the repository uses.</param>
/// <param name="logger">The `ILambdaLogger` used for logging.</param>
public class SchoolRepo(OteContext context, ILambdaLogger logger) : AbstractRepo<SchoolEntity>(context, logger)
{}
