using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benefits.BusinessLogic;
using Benefits.BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Benefits.DataAccess;
using FakeItEasy;
using System.Linq;

namespace Benefits.BusinessLogic.Tests
{
    [TestClass]
    public class BenefitCalculatorTests
    {
        #region ValidateInputTests

        [TestMethod]
        public async Task ValidateInput_ValidRequest_ReturnsTrue()
        {
            //Arrange
            BenefitCalculator calculator = new BenefitCalculator();
            List<BeneficiaryRequestModel> testRequest = new List<BeneficiaryRequestModel>
            {
                new BeneficiaryRequestModel
                {
                    Name = "name1",
                    Type = BenefitsData.Employee
                },
                new BeneficiaryRequestModel
                {
                    Name = "name2",
                    Type = BenefitsData.Dependent
                }
            };

            //Act
            bool result = await calculator.ValidateRequest(testRequest);

            //Assert
            Assert.IsTrue(result, "Expected validation to pass as inputs were valid");
        }

        [TestMethod]
        public async Task ValidateInput_BadRequest_EmptyName_ReturnsFalse()
        {
            //Arrange
            BenefitCalculator calculator = new BenefitCalculator();
            List<BeneficiaryRequestModel> testRequest= new List<BeneficiaryRequestModel>
            {
                new BeneficiaryRequestModel
                {
                    Name = " ",
                    Type = BenefitsData.Employee
                },
                new BeneficiaryRequestModel
                {
                    Name = "name2",
                    Type = BenefitsData.Dependent
                }
            };

            //Act
            bool result = await calculator.ValidateRequest(testRequest);

            //Assert
            Assert.IsFalse(result, "Expected validation to fail as input contained an empty name");
        }

        [TestMethod]
        public async Task ValidateInput_BadRequest_InvalidType_ReturnsFalse()
        {
            //Arrange
            
            BenefitCalculator calculator = new BenefitCalculator();
            List<BeneficiaryRequestModel> testRequest = new List<BeneficiaryRequestModel>
            {
                new BeneficiaryRequestModel
                {
                    Name = "Name1",
                    Type = BenefitsData.Employee
                },
                new BeneficiaryRequestModel
                {
                    Name = "name2",
                    Type = "badType"
                }
            };

            //Act
            bool result = await calculator.ValidateRequest(testRequest);

            //Assert
            Assert.IsFalse(result, "Expected validation to fail as input contained an invalid type.");
        }
        #endregion

        #region CalculateBenefitTests

        [TestMethod]
        public async Task CalculateBenefits_GivenInput_ReturnsExpectedCosts()
        {

            //Arrange
            string name1 = "Name1";
            string name2 = "Name2";
            decimal discount = 0.5M;
            int payPeriods = 10;

            decimal employeeYearCost = 400M;
            decimal dependentYearCost = 200M;

            decimal employeeDiscountYearCost = (1 - discount) * employeeYearCost;
            decimal dependentDiscountYearCost = (1 - discount) * dependentYearCost;

            List<BeneficiaryRequestModel> testRequest = new List<BeneficiaryRequestModel>
            {
                new BeneficiaryRequestModel
                {
                    Name = name1,
                    Type = BenefitsData.Employee
                },
                new BeneficiaryRequestModel
                {
                    Name = name2,
                    Type = BenefitsData.Dependent
                }
            };

            BenefitCalculator calculator = new BenefitCalculator();
            IDiscountHandler fakeDiscountHandler = A.Fake<IDiscountHandler>();
            IDataAccessor fakeDataAccess = A.Fake<IDataAccessor>();
            calculator.DiscountHandler = fakeDiscountHandler;
            calculator.DataAccess = fakeDataAccess;
            A.CallTo(() => fakeDiscountHandler.EligibleForDiscount(A<string>.Ignored)).Returns(true);
            A.CallTo(() => fakeDataAccess.GetBenefitCostsAsync(BenefitsData.Employee)).Returns(Task.FromResult(employeeYearCost));
            A.CallTo(() => fakeDataAccess.GetBenefitCostsAsync(BenefitsData.Dependent)).Returns(Task.FromResult(dependentYearCost));
            A.CallTo(() => fakeDataAccess.GetDiscountAsync()).Returns(Task.FromResult(discount));
            A.CallTo(() => fakeDataAccess.GetPayPeriodsAsync()).Returns(Task.FromResult(payPeriods));

            //Act
            var response = await calculator.CalculateBenefits(testRequest);

            //Assert
            A.CallTo(() => fakeDiscountHandler.EligibleForDiscount(name1)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDiscountHandler.EligibleForDiscount(name2)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDataAccess.GetDiscountAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDataAccess.GetPayPeriodsAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDataAccess.GetBenefitCostsAsync(BenefitsData.Employee)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeDataAccess.GetBenefitCostsAsync(BenefitsData.Dependent)).MustHaveHappenedOnceExactly();

            BeneficiaryResponseModel expected1 = new BeneficiaryResponseModel
            {
                Name = name1,
                Type = BenefitsData.Employee,
                YearCost = employeeDiscountYearCost,
                PeriodCost = decimal.Round((employeeDiscountYearCost / payPeriods), 2)
            };

            BeneficiaryResponseModel expected2 = new BeneficiaryResponseModel
            {
                Name = name2,
                Type = BenefitsData.Dependent,
                YearCost = dependentDiscountYearCost,
                PeriodCost = decimal.Round((dependentDiscountYearCost / payPeriods), 2)
            };

            Assert.AreEqual(2, response.Count, "Expected the response to contain the same number of entries as input");

            var bene1 = response.Where(b =>
            {
                return (b.Name.Equals(expected1.Name) && b.Type.Equals(expected1.Type) && b.YearCost == expected1.YearCost && b.PeriodCost == expected1.PeriodCost);
            });

            var bene2 = response.Where(b =>
            {
                return (b.Name.Equals(expected2.Name) && b.Type.Equals(expected2.Type) && b.YearCost == expected2.YearCost && b.PeriodCost == expected2.PeriodCost);
            });

            Assert.AreEqual(1, bene1.Count(), "Expected the response to contain expected1 model");
            Assert.AreEqual(1, bene2.Count(), "Expected the response to contain expected2 model");
        }

        #endregion


        #region DiscountHandlerTests
        [TestMethod]
        public void EligibleForDiscount_NameStartsWithA_ReturnsTrue()
        {
            DiscountHandler handler = new DiscountHandler();

            string name1 = "Adam Smith";
            string name2 = " alan Curtis ";

            bool discounted1 = handler.EligibleForDiscount(name1);
            bool discounted2 = handler.EligibleForDiscount(name2);

            Assert.IsTrue(discounted1, $"Expected name: {name1} to be eligible for a discount");
            Assert.IsTrue(discounted2, $"Expected name: {name2} to be eligible for a discount");
        }

        [TestMethod]
        public void EligibleForDiscount_NameNotStartsWithA_ReturnsFalse()
        {
            DiscountHandler handler = new DiscountHandler();

            string name1 = "Bob Anthony";
            string name2 = " curt Alanson";

            bool discounted1 = handler.EligibleForDiscount(name1);
            bool discounted2 = handler.EligibleForDiscount(name2);

            Assert.IsFalse(discounted1, $"Expected name: {name1} to not be eligible for a discount");
            Assert.IsFalse(discounted2, $"Expected name: {name2} to not be eligible for a discount");
        }
        #endregion
    }
}
