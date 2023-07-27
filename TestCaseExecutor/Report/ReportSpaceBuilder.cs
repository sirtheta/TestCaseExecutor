using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;

namespace TestCaseExecutor.Report
{
    internal static class ReportSpaceBuilder
    {
        internal static void Build(SectionBuilder section)
        {
            section.AddLine()
                    .SetMarginTop(5)
                    .SetMarginBottom(5)
                    .SetLength(XUnit.FromPercent(100))
                    .SetWidth(3);

        }
    }
}
