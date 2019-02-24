
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 398108 $
 * $Date: 2006-04-29 11:39:42 +0200 (sam., 29 avr. 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

#region Using

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Castle.DynamicProxy;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Logging;

#endregion

namespace IBatisNet.DynamicProxy
{
    /// <summary>
    /// An ProxyGenerator with cache that uses the Castle.DynamicProxy library.
    /// </summary>
    [CLSCompliant(false)]
    public class CachedProxyGenerator : ProxyGenerator
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // key = mapped type
        // value = proxy type
        private IDictionary _cachedProxyTypes = null;

        /// <summary>
        /// Cosntructor
        /// </summary>
        public CachedProxyGenerator()
        {
            _cachedProxyTypes = new HybridDictionary();
        }

        public override object CreateInterfaceProxyWithTarget(Type interfaceToProxy, Type[] additionalInterfacesToProxy, object target,
            ProxyGenerationOptions options, params IInterceptor[] interceptors)
        {
            return base.CreateInterfaceProxyWithTarget(interfaceToProxy, additionalInterfacesToProxy, target, options, interceptors);
        }

        public override object CreateClassProxy(Type classToProxy, Type[] additionalInterfacesToProxy, ProxyGenerationOptions options,
            object[] constructorArguments, params IInterceptor[] interceptors)
        {
            return base.CreateClassProxy(classToProxy, additionalInterfacesToProxy, options, constructorArguments, interceptors);
        }

        public override object CreateClassProxyWithTarget(Type classToProxy, Type[] additionalInterfacesToProxy, object target,
            ProxyGenerationOptions options, object[] constructorArguments, params IInterceptor[] interceptors)
        {
            return base.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, options, constructorArguments, interceptors);
        }

        public override object CreateInterfaceProxyWithTargetInterface(Type interfaceToProxy, Type[] additionalInterfacesToProxy, object target,
            ProxyGenerationOptions options, params IInterceptor[] interceptors)
        {
            return base.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, target, options, interceptors);
        }

        public override object CreateInterfaceProxyWithoutTarget(Type interfaceToProxy, Type[] additionalInterfacesToProxy,
            ProxyGenerationOptions options, params IInterceptor[] interceptors)
        {
            return base.CreateInterfaceProxyWithoutTarget(interfaceToProxy, additionalInterfacesToProxy, options, interceptors);
        }

        private object CreateInterfaceProxy(Type[] interfaces, IInterceptor interceptor, object target)
        {
            try
            {
                System.Type proxyType = null;
                System.Type targetType = target.GetType();

                lock (_cachedProxyTypes.SyncRoot)
                {
                    proxyType = _cachedProxyTypes[targetType] as System.Type;

                    if (proxyType == null)
                    {
                        proxyType = ProxyBuilder.CreateInterfaceProxyTypeWithTarget(interfaces.First(),
                            interfaces.Skip(1).ToArray(), targetType, ProxyGenerationOptions.Default);
                        _cachedProxyTypes[targetType] = proxyType;
                    }
                }

                return base.CreateInterfaceProxyWithTarget(targetType, target, ProxyGenerationOptions.Default,
                    interceptor);
            }
            catch (Exception e)
            {
                _log.Error("Castle Dynamic Proxy Generator failed", e);
                throw new IBatisNetException("Castle Proxy Generator failed", e);
            }
        }

        private object CreateCachedInterfaceProxy(Type[] interfaces, IInterceptor interceptor, object target)
        {
            try
            {
                System.Type proxyType = null;
                System.Type targetType = target.GetType();

                lock (_cachedProxyTypes.SyncRoot)
                {
                    proxyType = _cachedProxyTypes[targetType] as System.Type;

                    if (proxyType == null)
                    {
                        proxyType = ProxyBuilder.CreateInterfaceProxyTypeWithTarget(interfaces.First(),
                            interfaces.Skip(1).ToArray(), targetType, ProxyGenerationOptions.Default);
                        _cachedProxyTypes[targetType] = proxyType;
                    }
                }

                return base.CreateInterfaceProxyWithTarget(targetType, target, ProxyGenerationOptions.Default,
                    interceptor);
            }
            catch (Exception e)
            {
                _log.Error("Castle Dynamic Proxy Generator failed", e);
                throw new IBatisNetException("Castle Proxy Generator failed", e);
            }
        }

        private object CreateCachedClassProxy(Type targetType, IInterceptor interceptor, params object[] argumentsForConstructor)
        {
            try
            {
                System.Type proxyType = null;

                lock (_cachedProxyTypes.SyncRoot)
                {
                    proxyType = _cachedProxyTypes[targetType] as System.Type;

                    if (proxyType == null)
                    {
                        proxyType = ProxyBuilder.CreateClassProxyType(targetType, new Type[0], ProxyGenerationOptions.Default);
                        _cachedProxyTypes[targetType] = proxyType;
                    }
                }

                return base.CreateClassProxy(proxyType, ProxyGenerationOptions.Default, argumentsForConstructor);
            }
            catch (Exception e)
            {
                _log.Error("Castle Dynamic Class-Proxy Generator failed", e);
                throw new IBatisNetException("Castle Proxy Generator failed", e);
            }

        }

        /// <summary>
        /// Generates a proxy implementing all the specified interfaces and
        /// redirecting method invocations to the specifed interceptor.
        /// </summary>
        /// <param name="interfaces">Array of interfaces to be implemented</param>
        /// <param name="interceptor">instance of <see cref="IInterceptor"/></param>
        /// <param name="target">The target object.</param>
        /// <returns>Proxy instance</returns>
        //public override object CreateProxy(Type[] interfaces, IInterceptor interceptor, object target)
        //{
        //    try
        //    {
        //        System.Type proxyType = null;
        //        System.Type targetType = target.GetType();

        //        lock (_cachedProxyTypes.SyncRoot)
        //        {
        //            proxyType = _cachedProxyTypes[targetType] as System.Type;

        //            if (proxyType == null)
        //            {
        //                proxyType = ProxyBuilder.CreateInterfaceProxy(interfaces, targetType);
        //                _cachedProxyTypes[targetType] = proxyType;
        //            }
        //        }
        //        return base.CreateProxyInstance(proxyType, interceptor, target);
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Error("Castle Dynamic Proxy Generator failed", e);
        //        throw new IBatisNetException("Castle Proxy Generator failed", e);
        //    }
        //}



        /// <summary>
        /// Generates a proxy implementing all the specified interfaces and
        /// redirecting method invocations to the specifed interceptor.
        /// This proxy is for object different from IList or ICollection
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="argumentsForConstructor">The arguments for constructor.</param>
        /// <returns></returns>
        //public override object CreateClassProxy(Type targetType, IInterceptor interceptor, params object[] argumentsForConstructor)
        //{
        //    try
        //    {
        //        System.Type proxyType = null;

        //        lock (_cachedProxyTypes.SyncRoot)
        //        {
        //            proxyType = _cachedProxyTypes[targetType] as System.Type;

        //            if (proxyType == null)
        //            {
        //                proxyType = ProxyBuilder.CreateClassProxy(targetType);
        //                _cachedProxyTypes[targetType] = proxyType;
        //            }
        //        }
        //        return CreateClassProxyInstance(proxyType, interceptor, argumentsForConstructor);
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Error("Castle Dynamic Class-Proxy Generator failed", e);
        //        throw new IBatisNetException("Castle Proxy Generator failed", e);
        //    }

        //}
    }
}
