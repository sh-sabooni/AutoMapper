namespace AutoMapper.UnitTests.Bug
{
    namespace NamingInconsistencyBug
    {
        using AutoMapper;
        using NUnit.Framework;

        namespace AutomapperTest
        {
            class From
            {
                public string S1 { get; set; }
                public string S2 { get; set; }
            }

            class To
            {
                public Wrapper S1 { get; set; }
                public Wrapper S2 { get; set; }
            }

            class Wrapper
            {
                public Wrapper(string value)
                {
                    Value = value;
                }

                public string Value { get; private set; }
            }

            [TestFixture]
            public class MappingTests
            {
                [SetUp]
                public void Init()
                {
                    Mapper.Reset();
                    Mapper
                        .CreateMap<string, Wrapper>()
                        .ForMember(d => d.Value, c => c.Ignore())
                        .ConstructUsing(s => new Wrapper(s));

                    Mapper.CreateMap<From, To>();
                    Mapper.AssertConfigurationIsValid();
                }

                [Test]
                public void Mapping_WithSameStringValues_ResultsInEqualCreatedInstances()
                {
                    var from = new From { S1 = "a", S2 = "a" };
                    var to = Mapper.Map<From, To>(from);

                    Assert.IsTrue(from.S1 == to.S1.Value);
                    Assert.IsTrue(from.S2 == to.S2.Value);
                    Assert.IsTrue(to.S1.Value == to.S2.Value);

                    Assert.AreNotSame(to.S1, to.S2);
                }
            }
        }    
    }
}