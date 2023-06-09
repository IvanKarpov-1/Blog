﻿using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Core;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        AddRange(items);
    }

    public PagedList(IEnumerable<T> items, int currentPage, int totalPages, int pageSize, int totalCount)
    {
        CurrentPage = currentPage;
        TotalPages = totalPages;
        PageSize = pageSize;
        TotalCount = totalCount;
        AddRange(items);
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}

public static class PagedListMapper
{
    public static PagedList<TMapTo> Map<TMapFrom, TMapTo>(this PagedList<TMapFrom> source, Func<TMapFrom, TMapTo> mapperConfiguration)
    {
        return new PagedList<TMapTo>(source.Select(mapperConfiguration), source.CurrentPage, source.TotalPages,
            source.PageSize, source.TotalCount);
    }
}