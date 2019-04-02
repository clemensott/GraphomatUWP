using System.Xml.Serialization;

namespace MathFunction
{
    enum PriorityType { Same = 0, LeftHigher = -1, RightHigher = 1 }

    enum RelativeTo { Left, Right }

    interface IResult
    {
        [XmlIgnore]
        double this[double x] { get; }

        bool Used { get; set; }

        double GetResult(double x);

        PartResultPriority GetPriority();

        int GetPriorityValue();

        double GetRelativePriority(RelativeTo relativeTo);

        PriorityType GetPriorityType();

        void SetValues(Parts parts);
    }
}
