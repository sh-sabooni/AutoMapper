using System;
using NUnit.Framework;
using Should;

namespace AutoMapper.UnitTests.Bug
{
    public class MissingMemberMessage : NonValidatingSpecBase
    {
        private Exception _exception;

        public class BeerResource
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public int BreweryId { get; set; }
            public string Brewery { get; set; }

            public int StyleId { get; set; }
            public string Style { get; set; }
        }

        public class Beer
        {
            protected Beer()
            {
            }

            public Beer(string name)
            {
                Name = name;
            }

            public int Id { get; protected set; }
            public string Name { get; set; }
            public BeerStyle Style { get; set; }
            public Brewery Brewery { get; set; }
        }

        public class BeerStyle
        {
            public int Id { get; set; }
        }

        public class Brewery
        {
            public int Id { get; set; }
        }

        protected override void Establish_context()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Beer, BeerResource>();
            });
        }

        protected override void Because_of()
        {
            try
            {
                Mapper.AssertConfigurationIsValid();
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        [Test]
        public void Should_not_suck()
        {
            _exception.ShouldNotEqual(null);

            Console.WriteLine(_exception.ToString());
        }
    }

}