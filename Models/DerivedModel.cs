using Avalonia.Media;

namespace Architeptable.Models;

public sealed record DerivedModel<T>(T Content, IBrush Highlight);
