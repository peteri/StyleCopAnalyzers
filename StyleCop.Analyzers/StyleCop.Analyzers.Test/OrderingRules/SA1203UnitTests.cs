﻿namespace StyleCop.Analyzers.Test.OrderingRules
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.OrderingRules;
    using TestHelper;
    using Xunit;

    public class SA1203UnitTests : DiagnosticVerifier
    {
        [Fact]
        public async Task TestNoDiagnosticAsync()
        {
            var testCode = @"public static class TestClass1 { }

public class TestClass2
{
    public const int TestField1 = 1;
    public static readonly int TestField2 = 1;
    public static int TestField3 = 1;
    public readonly int TestField4 = 1;
    public int TestField5 = 1;
    internal const int TestField6 = 1;
    internal static readonly int TestField7 = 1;
    internal static int TestField8 = 1;
    internal readonly int TestField9 = 1;
    internal int TestField10 = 1;
    protected internal const int TestField11 = 1;
    protected internal static readonly int TestField12 = 1;
    protected internal static int TestField13 = 1;
    protected internal readonly int TestField14 = 1;
    protected internal int TestField15 = 1;
    protected const int TestField16 = 1;
    protected static readonly int TestField17 = 1;
    protected static int TestField18 = 1;
    protected readonly int TestField19 = 1;
    protected int TestField20 = 1;
    private const int TestField21 = 1;
    private static readonly int TestField22 = 1;
    private static int TestField23 = 1;
    private readonly int TestField24 = 1;
    private int TestField25 = 1;

    public TestClass2()
    {
    }

    private TestClass2(string a)
    {
    }

    ~TestClass2() { }
    
    public static int TestProperty1 { get; set; }
    public int TestProperty2 { get; set; }
    internal static int TestProperty3 { get; set; }
    internal int TestProperty4 { get; set; }
    protected internal static int TestProperty5 { get; set; }
    protected internal int TestProperty6 { get; set; }
    protected static int TestProperty7 { get; set; }
    protected int TestProperty8 { get; set; }
    private static int TestProperty9 { get; set; }
    private int TestProperty10 { get; set; }
    
    public static void TestMethod1() { }
    public void TestMethod2() { }
    internal static void TestMethod3() { }
    internal void TestMethod4() { }
    protected internal static void TestMethod5() { }
    protected internal void TestMethod6() { }
    protected static void TestMethod7() { }
    protected void TestMethod8() { }
    private static void TestMethod9() { }
    private void TestMethod10() { }

    public static class TestClass1 { }
    public class TestClass2a { }
    internal static class TestClass3 { }
    internal class TestClass4 { }
    protected internal static class TestClass5 { }
    protected internal class TestClass6 { }
    protected static class TestClass7 { }
    protected class TestClass8 { }
    private static class TestClass9 { }
    private class TestClass10 { }
}
";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestClassViolationAsync()
        {
            var testCode = @"
public class Foo
{
    private int Baz = 1;
    private const int Bar = 2;
}";
            var firstDiagnostic = this.CSharpDiagnostic().WithLocation(5, 23).WithArguments("private");
            await this.VerifyCSharpDiagnosticAsync(testCode, firstDiagnostic, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestStructViolationAsync()
        {
            var testCode = @"
public struct Foo
{
    private int baz;
    private const int Bar = 2;
}";
            var firstDiagnostic = this.CSharpDiagnostic().WithLocation(5, 23).WithArguments("private");
            await this.VerifyCSharpDiagnosticAsync(testCode, firstDiagnostic, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSecondConstAfterNonConstAsync()
        {
            var testCode = @"
public class Foo
{
    private const int Bar = 2;
    private int Baz = 1;
    private const int FooBar = 2;
}";
            var firstDiagnostic = this.CSharpDiagnostic().WithLocation(6, 23).WithArguments("private");
            await this.VerifyCSharpDiagnosticAsync(testCode, firstDiagnostic, CancellationToken.None).ConfigureAwait(false);
        }

        protected override IEnumerable<DiagnosticAnalyzer> GetCSharpDiagnosticAnalyzers()
        {
            yield return new SA1203ConstantsMustAppearBeforeFields();
        }
    }
}