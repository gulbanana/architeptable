namespace Architeptable.Models;

public abstract record OptionModel(string Name);

public sealed record OptionModel<T>(T ID, string Name) : OptionModel(Name);
