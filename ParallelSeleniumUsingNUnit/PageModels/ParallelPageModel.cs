using System;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Threading.Tasks;
using ImpromptuInterface;

namespace ParallelSeleniumUsingNUnit.PageModels
{
    public class ParallelPageModel<T> : DynamicObject
    {
        private T[] Pages { get; set; }
        public ParallelPageModel(params T[] pages)
        {
            Pages = pages;
        }

        // have to cast this way because you can't use generics in operator overloading
        public T Cast()
        {
            T returnVar = this.ActLike();
            return returnVar;
        }


        public override bool TryInvokeMember(
           InvokeMemberBinder binder,
           object[] args,
           out object result)
        {
            result = null;
            var results = new ConcurrentBag<object>();
            Parallel.ForEach(Pages, page =>
            {

                var thisResult = typeof(T).InvokeMember(binder.Name,
                    BindingFlags.InvokeMethod |
                    BindingFlags.Public |
                    BindingFlags.Instance,
                    null, page, args);
                if (thisResult != null && !(thisResult is T) && !(thisResult is String) && !(thisResult is ValueType))
                    throw new Exception("You can't return a value that is a reference type other than Strings because anything you do with it will not be parallelized in future calls and there is no reason to use it otherwise.");
                results.Add(thisResult);
            });

            foreach (var thisResult in results)
            {
                if (thisResult is T)
                    result = this;
                else if (result != null)
                {
                    if (!result.Equals(thisResult)) // not the same value
                    {
                        throw new Exception("Call to method returns different values. [" + result + "] and [" + thisResult + "]");
                    }
                }
                else
                {
                    result = thisResult;
                }

            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Parallel.ForEach(Pages, page =>
                typeof(T).GetProperty(binder.Name).SetValue(page, value)
                );
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var results = new ConcurrentBag<object>();

            Parallel.ForEach(Pages, page =>
            {
                var thisResult = typeof(T).GetProperty(binder.Name).GetValue(page);
                if (thisResult != null && !(thisResult is T) && !(thisResult is String) && !(thisResult is ValueType))
                    throw new Exception("You can't return a value that is a reference type other than Strings because anything you do with it will not be parallelized in future calls and there is no reason to use it otherwise.");
                results.Add(thisResult);
            });

            foreach (var thisResult in results)
            {
                if (thisResult is T)
                    result = this;
                else if (result != null)
                {
                    if (!result.Equals(thisResult)) // not the same value
                    {
                        throw new Exception("Call to property returns different values. [" + result + "] and [" + thisResult + "]");
                    }
                }
                else
                {
                    result = thisResult;
                }
            }
            return true;
        }

    }
}
