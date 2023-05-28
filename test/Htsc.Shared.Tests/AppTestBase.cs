using System;
using System.Threading.Tasks;
using Abp.TestBase;

namespace Htsc.Shared.Tests
{

    public class AppTestBase : AbpIntegratedTestBase<SharedTestModule>
    {
        public AppTestBase()
        {
            UsingDbContext(context => new TestDataBuilder(context).Build());
        }

        protected virtual void UsingDbContext(Action<MposDbContext> action)
        {
            using (var context = LocalIocManager.Resolve<MposDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        protected virtual T UsingDbContext<T>(Func<MposDbContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<MposDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        protected virtual async Task UsingDbContextAsync(Func<MposDbContext, Task> action)
        {
            using (var context = LocalIocManager.Resolve<MposDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        protected virtual async Task<T> UsingDbContextAsync<T>(Func<MposDbContext, Task<T>> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<MposDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}