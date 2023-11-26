using Xunit.Abstractions;
using Xunit.Sdk;

namespace Cynnamon.Tests.Stories;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestIndexAttribute(int index) : Attribute {
    public int Index => index;
};

/// <summary> Adapted from <see href="https://github.com/xunit/samples.xunit/blob/main/TestOrderExamples/TestCaseOrdering/PriorityOrderer.cs" /> </summary>
public class SequenceTestCaseOrderer : ITestCaseOrderer {
    public const string TypeName = "Cynnamon.Tests.Stories.SequenceTestCaseOrderer";
    public const string AssemblyName = "Cynnamon.Tests";


    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase => testCases
        .OrderBy(test => test.TestMethod.Method
                .GetCustomAttributes(typeof(TestIndexAttribute).AssemblyQualifiedName)
                .FirstOrDefault()
                ?.GetNamedArgument<int>("Index") ?? int.MaxValue
        );
}
