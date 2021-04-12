using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = AspNetCoreCodeFix.Test.CSharpCodeFixVerifier<
    AspNetCoreCodeFix.AspNetCoreCodeFixAnalyzer,
    AspNetCoreCodeFix.AspNetCoreCodeFixCodeFixProvider>;

namespace AspNetCoreCodeFix.Test
{
    [TestClass]
    public class UseHttpContextAsMember
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task NoDiagnosticExcpectedToShowUp()
        {
            await VerifyCS.VerifyAnalyzerAsync("");
        }

        [TestMethod]
        public async Task LocalIntCouldBeConstant_Diagnostic()
        {
            await VerifyCS.VerifyCodeFixAsync(@"
using Microsoft.AspNetCore.Http;

namespace ErabliereApi
{
    public class UseOfHttpContextAccessor
    {
        private readonly [|HttpContext|]? _context;

        public UseOfHttpContextAccessor(IHttpContextAccessor accessor)
        {
            [|_context = accessor.HttpContext;|]
        }

        public void DoSomthing()
        {
            if ([|_context|] != null) [|_context|].Response.Headers[""accept""] = ""*/*"";
        }
    }
}
", @"
using Microsoft.AspNetCore.Http;

namespace ErabliereApi
{
    public class UseOfHttpContextAccessor
    {
        private readonly IHttpContextAccessor _context;

        public UseOfHttpContextAccessor(IHttpContextAccessor accessor)
        {
            _context = accessor;
        }

        public void DoSomthing()
        {
            if (_context.HttpContext != null) _context.HttpContext.Response.Headers[""accept""] = ""*/*"";
        }
    }
}
");
        }
    }
}
