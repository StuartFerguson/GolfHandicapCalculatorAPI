// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace ManagementAPI.IntegrationTests.GolfClub
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "golfclub")]
    public partial class AddTournamentDivisionToGolfClubFeature : Xunit.IClassFixture<AddTournamentDivisionToGolfClubFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "AddTournamentDivisionToGolfClub.feature"
#line hidden
        
        public AddTournamentDivisionToGolfClubFeature(AddTournamentDivisionToGolfClubFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "AddTournamentDivisionToGolfClub", null, ProgrammingLanguage.CSharp, new string[] {
                        "golfclub"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
#line 5
 testRunner.Given("The Golf Handicapping System Is Running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.And("I have registered as a golf club administrator", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.And("I am logged in as a golf club administrator", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.And("my golf club has been created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Add Tournament Divisions")]
        [Xunit.TraitAttribute("FeatureTitle", "AddTournamentDivisionToGolfClub")]
        [Xunit.TraitAttribute("Description", "Add Tournament Divisions")]
        public virtual void AddTournamentDivisions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add Tournament Divisions", null, ((string[])(null)));
#line 10
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 4
this.FeatureBackground();
#line 11
 testRunner.When("I add tournament division 1 with a start handicap of 0 and and end handicap of 5", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 12
 testRunner.Then("the divsion is addded successfully with an Http Status Code 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
 testRunner.When("I add tournament division 2 with a start handicap of 6 and and end handicap of 12" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
 testRunner.Then("the divsion is addded successfully with an Http Status Code 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 15
 testRunner.When("I add tournament division 3 with a start handicap of 13 and and end handicap of 2" +
                    "1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
 testRunner.Then("the divsion is addded successfully with an Http Status Code 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 17
 testRunner.When("I add tournament division 4 with a start handicap of 22 and and end handicap of 2" +
                    "8", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
 testRunner.Then("the divsion is addded successfully with an Http Status Code 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                AddTournamentDivisionToGolfClubFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                AddTournamentDivisionToGolfClubFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion