﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Office.Utility.Extensions
{
    /// <summary>
    /// Extension methods which help a deterministic cleanup model
    /// </summary>
    public static class ComCleanupExtensions
    {
        /// <summary>
        /// Enables Linq on any COM collection. Releases each iterated item deterministically
        /// as the collection is enumerated
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comCollection">The COM collection.</param>
        /// <returns></returns>
        public static IEnumerable<T> ComLinq<T>(this IEnumerable comCollection)
            where T : class
        {
            return new ComCleanupWrapper<T>(comCollection, ReleaseComObject, ReleaseComObject);
        }

        /// <summary>
        /// Releases the COM object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The Com object to releases.</param>
        public static void ReleaseComObject<T>(this T resource) where T : class
        {
            if (resource != null && Marshal.IsComObject(resource))
                Marshal.ReleaseComObject(resource);
        }

        /// <summary>
        /// Wraps the Com resource in an IDisposable which releases 
        /// the Com object when Dispose is called.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The Com object to wrap.</param>
        /// <returns></returns>
        public static AutoDispose<T> WithComCleanup<T>(this T resource) where T : class
        {
            return new AutoDispose<T>(resource, ReleaseComObject);
        }

        /// <summary>
        /// Wraps the Com resource in an IDisposable which releases 
        /// the Com object when Dispose is called.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The Com object to wrap.</param>
        /// <returns></returns>
        public static IEnumerable<T> WithComCleanup<T>(this IEnumerable<T> source) where T : class
        {
            if (null == source) throw new ArgumentNullException("source");
            return new ComCleanupWrapper<T>(source, ReleaseComObject, ReleaseComObject);
        }
    }
}