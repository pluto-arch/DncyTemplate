using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dncy.Specifications.Evaluators;
using EntityFrameworkCore.Specifications.Test.Models;
using NUnit.Framework;

namespace EntityFrameworkCore.Specifications.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void InMemorySpecificationEvaluator_Test()
        {
            var query = GetQuerySource();
            var res = InMemorySpecificationEvaluator.Default.Evaluate(query, new IdBetweenOneAndTenSpecification());
            Assert.IsTrue(res.Count()==10);
        }



        private IEnumerable<User> GetQuerySource()
        {
            foreach (var item in Enumerable.Range(1,200))
            {
                yield return new User
                {
                    Id = item,
                    Name = $"{item}_{DateTime.Now.Ticks}",
                    Avatar = "123123",
                    Age = Random.Shared.Next(1,100)
                };
            }
        }
    }
}