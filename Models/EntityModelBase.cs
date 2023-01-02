using Architeptable.Data;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Architeptable.Models;

public class EntityModelBase : ModelBase
{
    public TabModelBase? Owner { get; init; }
    public int ID { get; init; }

    private IBrush highlight = Brushes.Transparent;
    public IBrush Highlight
    {
        get => highlight;
        set => RaiseAndSetIfChanged(ref highlight, value);
    }

    protected bool SaveIfChanged<T>(ref T field, T value, Action<EntityContext> write, bool reload = false, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            Owner?.Save(c => write(c), reload);
            RaisePropertyChanged(propertyName);
            return true;
        }
        return false;
    }

    protected void Save(Action<EntityContext> write, bool reload = false)
    {
        Owner?.Save(c => write(c), reload);
    }
}

public class EntityModelBase<E> : EntityModelBase where E : class
{
    protected bool SaveIfChanged<T>(ref T field, T value, Action<E> write, bool reload = false, [CallerMemberName] string? propertyName = null)
    {
        return SaveIfChanged(ref field, value, c =>
        {
            write(c.Set<E>().Find(ID)!);
        }, reload, propertyName);
    }

    protected bool SaveIfChanged<T>(ref T field, T value, bool reload = false, [CallerMemberName] string? propertyName = null)
    {
        return SaveIfChanged(ref field, value, c =>
        {
            var entity = c.Set<E>().Find(ID)!;
            var entry = c.Entry(entity);
            entry.Property<T>(propertyName!).CurrentValue = value;
        }, reload, propertyName);
    }

    protected void Save(Action<E> write, bool reload = false)
    {
        Save(c =>
        {
            write(c.Set<E>().Find(ID)!);
        }, reload);
    }
}
