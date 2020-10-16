using Abp.Application.Services.Dto;
using Abp.BaseDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Abp.Extensions
{
    public static partial class Ext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public async static Task<PagedResultDto<TResult>> ToPagedResultAsync<TSource, TResult>(this IQueryable<TSource> query, int skip, int take, Expression<Func<TSource, TResult>> selector) where TResult : class
        {
            var count = await query.CountAsync();
            var list = await query.Select(selector).Skip(skip).Take(take).AsNoTracking().ToListAsync();
            return new PagedResultDto<TResult>(count, list);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public async static Task<PagedResultDto<TResult>> ToPagedResultAsync<TSource, TResult>(this IQueryable<TSource> query, IPagedResultRequest page, Expression<Func<TSource, TResult>> selector) where TResult : class
        {
            var count = await query.CountAsync();
            var list = await query.Select(selector).Skip(page.SkipCount).Take(page.MaxResultCount).AsNoTracking().ToListAsync();
            return new PagedResultDto<TResult>(count, list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="selector"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async static Task<TResult> ToPagedResultAsync<TSource, TSelect, TResult>(this IQueryable<TSource> query, PagedInputDto page, Expression<Func<TSource, TSelect>> selector, Func<int, List<TSelect>, TResult> func)
          where TSelect : class
          where TResult : PagedResultDto<TSelect>
        {
            var count = await query.CountAsync();
            var list = await query.Select(selector).Skip(page.SkipCount).Take(page.MaxResultCount).AsNoTracking().ToListAsync();
            return func(count, list);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="selector"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async static Task<TResult> ToPagedResultAsync<TSource, TSelect, TResult>(this IQueryable<TSource> query, int skip, int take, Expression<Func<TSource, TSelect>> selector, Func<int, List<TSelect>, TResult> func)
            where TSelect : class
            where TResult : PagedResultDto<TSelect>
        {
            var count = await query.CountAsync();
            var list = await query.Select(selector).Skip(skip).Take(take).AsNoTracking().ToListAsync();
            return func(count, list);

        }
        /// <summary>
        /// 条件正序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, object>> predicate)
        {

            return condition
               ? query.OrderBy(predicate)
               : query;
        }
        /// <summary>
        ///条件倒序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByDescendingIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, object>> predicate)
        {

            return condition
               ? query.OrderByDescending(predicate)
               : query;
        }
    }
}
