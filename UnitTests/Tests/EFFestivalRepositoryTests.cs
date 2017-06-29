using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Concrete;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Tests
{
    [TestClass()]
    public class EFFestivalRepositoryTests

    {
        EFFestivalRepository repo = new EFFestivalRepository();

        [TestMethod()]
        public void AddFestivalTest()
        {
            repo.DropDatabase();
            Festival festival = new Festival();
            festival.Name = "Test";
            festival.StartTime = new DateTime(2017, 06, 28, 11, 00, 00);
            festival.EndTime = new DateTime(2017, 06, 28, 23, 00, 00); ;
            festival.Location = "Oisterwijk";
            repo.AddFestival(festival);

            Festival repoFestival = repo.Festivals.FirstOrDefault(f => f.Id == festival.Id);

            Assert.AreEqual(festival, repoFestival);
        }

        [TestMethod()]
        public void AddStageTest()
        {
            repo.DropDatabase();
            Festival festival = new Festival();
            festival.Name = "Test";
            festival.StartTime = new DateTime(2017, 06, 28, 11, 00, 00);
            festival.EndTime = new DateTime(2017, 06, 28, 23, 00, 00); ;
            festival.Location = "Oisterwijk";
            repo.AddFestival(festival);

            Festival repoFestival = repo.Festivals.FirstOrDefault();
            Stage stage = new Stage();
            stage.Festival = repoFestival;
            stage.Name = "Stage";
            repo.AddStage(stage);

            Stage repoStage = repo.Stages.FirstOrDefault(s => s.Id == stage.Id);

            Assert.AreEqual(stage, repoStage);
        }

        [TestMethod()]
        public void AddPerformancesValidTest()
        {
            repo.DropDatabase();
            //Add festival
            Festival festival = new Festival();
            festival.Name = "Test";
            festival.StartTime = new DateTime(2017, 06, 28, 11, 00, 00);
            festival.EndTime = new DateTime(2017, 06, 28, 23, 00, 00); ;
            festival.Location = "Oisterwijk";
            repo.AddFestival(festival);
            Festival repoFestival = repo.Festivals.FirstOrDefault();

            //Add stage
            Stage stage = new Stage();
            stage.Festival = repoFestival;
            stage.Name = "Stage";
            repo.AddStage(stage);
            Stage repoStage = repo.Stages.FirstOrDefault();

            //Add valid performance
            Performance performance1 = new Performance();
            performance1.Artist = "B-Front";
            performance1.StartTime = new DateTime(2017, 06, 28, 11, 00, 00); ;
            performance1.EndTime = new DateTime(2017, 06, 28, 12, 00, 00); ;
            performance1.Stage = repoStage;
            repo.AddPerformance(performance1);           
            Performance repoPerformance1 = repo.Performances.FirstOrDefault(p => p.Id == performance1.Id);
            Assert.AreEqual(performance1, repoPerformance1);
        }

        [TestMethod()]
        public void AddPerformancesOutSideFestivalTimeTest()
        {
            repo.DropDatabase();
            //Add festival
            Festival festival = new Festival();
            festival.Name = "Test";
            festival.StartTime = new DateTime(2017, 06, 28, 11, 00, 00);
            festival.EndTime = new DateTime(2017, 06, 28, 23, 00, 00); ;
            festival.Location = "Oisterwijk";
            repo.AddFestival(festival);
            Festival repoFestival = repo.Festivals.FirstOrDefault();

            //Add stage
            Stage stage = new Stage();
            stage.Festival = repoFestival;
            stage.Name = "Stage";
            repo.AddStage(stage);
            Stage repoStage = repo.Stages.FirstOrDefault();

            //Add valid performance
            Performance performance1 = new Performance();
            performance1.Artist = "B-Front";
            performance1.StartTime = new DateTime(2017, 06, 28, 11, 00, 00); ;
            performance1.EndTime = new DateTime(2017, 06, 28, 12, 00, 00); ;
            performance1.Stage = repoStage;
            repo.AddPerformance(performance1);

            //Add performance outside of festival time
            Performance performance2 = new Performance();
            performance2.Artist = "B-Front";
            performance2.StartTime = new DateTime(2017, 06, 28, 10, 30, 00); ;
            performance2.EndTime = new DateTime(2017, 06, 28, 11, 30, 00); ;
            performance2.Stage = repoStage;
            int beforeCount = repo.StagePerformanceCount(repoStage.Id);
            repo.AddPerformance(performance2);
            int afterCount = repo.StagePerformanceCount(repoStage.Id);
            Assert.AreEqual(beforeCount, afterCount);
        }

        [TestMethod()]
        public void AddPerformancesOverlapTest()
        {
            repo.DropDatabase();
            //Add festival
            Festival festival = new Festival();
            festival.Name = "Test";
            festival.StartTime = new DateTime(2017, 06, 28, 11, 00, 00);
            festival.EndTime = new DateTime(2017, 06, 28, 23, 00, 00); ;
            festival.Location = "Oisterwijk";
            repo.AddFestival(festival);
            Festival repoFestival = repo.Festivals.FirstOrDefault();

            //Add stage
            Stage stage = new Stage();
            stage.Festival = repoFestival;
            stage.Name = "Stage";
            repo.AddStage(stage);
            Stage repoStage = repo.Stages.FirstOrDefault();

            //Add valid performance
            Performance performance1 = new Performance();
            performance1.Artist = "B-Front";
            performance1.StartTime = new DateTime(2017, 06, 28, 11, 00, 00); ;
            performance1.EndTime = new DateTime(2017, 06, 28, 12, 00, 00); ;
            performance1.Stage = repoStage;
            repo.AddPerformance(performance1);

            //Add performance outside of festival time
            Performance performance2 = new Performance();
            performance2.Artist = "B-Front";
            performance2.StartTime = new DateTime(2017, 06, 28, 11, 30, 00); ;
            performance2.EndTime = new DateTime(2017, 06, 28, 13, 00, 00); ;
            performance2.Stage = repoStage;
            int beforeCount = repo.StagePerformanceCount(repoStage.Id);
            repo.AddPerformance(performance2);
            int afterCount = repo.StagePerformanceCount(repoStage.Id);
            Assert.AreEqual(beforeCount, afterCount);
        }

        [TestMethod()]
        public void AddPerformancesBetween()
        {
            repo.DropDatabase();
            //Add festival
            Festival festival = new Festival();
            festival.Name = "Test";
            festival.StartTime = new DateTime(2017, 06, 28, 11, 00, 00);
            festival.EndTime = new DateTime(2017, 06, 28, 23, 00, 00); ;
            festival.Location = "Oisterwijk";
            repo.AddFestival(festival);
            Festival repoFestival = repo.Festivals.FirstOrDefault();

            //Add stage
            Stage stage = new Stage();
            stage.Festival = repoFestival;
            stage.Name = "Stage";
            repo.AddStage(stage);
            Stage repoStage = repo.Stages.FirstOrDefault();

            //Add valid performance
            Performance performance1 = new Performance();
            performance1.Artist = "B-Front";
            performance1.StartTime = new DateTime(2017, 06, 28, 11, 00, 00); ;
            performance1.EndTime = new DateTime(2017, 06, 28, 12, 00, 00); ;
            performance1.Stage = repoStage;
            repo.AddPerformance(performance1);

            Performance performance2 = new Performance();
            performance2.Artist = "B-Front";
            performance2.StartTime = new DateTime(2017, 06, 28, 13, 00, 00); ;
            performance2.EndTime = new DateTime(2017, 06, 28, 14, 00, 00); ;
            performance2.Stage = repoStage;
            repo.AddPerformance(performance2);

            //Add performance between performance1 and performance2
            Performance performance3 = new Performance();
            performance3.Artist = "B-Front";
            performance3.StartTime = new DateTime(2017, 06, 28, 12, 00, 00); ;
            performance3.EndTime = new DateTime(2017, 06, 28, 13, 00, 00); ;
            performance3.Stage = repoStage;
            int beforeCount = repo.StagePerformanceCount(repoStage.Id);
            repo.AddPerformance(performance3);
            int afterCount = repo.StagePerformanceCount(repoStage.Id);
            Assert.AreEqual(beforeCount+1, afterCount);
        }

        [TestMethod()]
        public void EditPerformancesBetween()
        {
            repo.DropDatabase();
            //Add festival
            Festival festival = new Festival();
            festival.Name = "Test";
            festival.StartTime = new DateTime(2017, 06, 28, 11, 00, 00);
            festival.EndTime = new DateTime(2017, 06, 28, 23, 00, 00); ;
            festival.Location = "Oisterwijk";
            repo.AddFestival(festival);
            Festival repoFestival = repo.Festivals.FirstOrDefault();

            //Add stage
            Stage stage = new Stage();
            stage.Festival = repoFestival;
            stage.Name = "Stage";
            repo.AddStage(stage);
            Stage repoStage = repo.Stages.FirstOrDefault();

            //Add valid performance
            Performance performance1 = new Performance();
            performance1.Artist = "B-Front";
            performance1.StartTime = new DateTime(2017, 06, 28, 11, 00, 00); ;
            performance1.EndTime = new DateTime(2017, 06, 28, 12, 00, 00); ;
            performance1.Stage = repoStage;
            repo.AddPerformance(performance1);

            Performance performance2 = new Performance();
            performance2.Artist = "B-Front";
            performance2.StartTime = new DateTime(2017, 06, 28, 13, 00, 00); ;
            performance2.EndTime = new DateTime(2017, 06, 28, 14, 00, 00); ;
            performance2.Stage = repoStage;
            repo.AddPerformance(performance2);

            //Add performance between performance1 and performance2
            Performance performance3 = new Performance();
            performance3.Artist = "B-Front";
            performance3.StartTime = new DateTime(2017, 06, 28, 12, 00, 00); ;
            performance3.EndTime = new DateTime(2017, 06, 28, 13, 00, 00); ;
            performance3.Stage = repoStage;
            repo.AddPerformance(performance3);

            //Edit performance between performance1 and performance2
            Performance editperformance3 = repo.PerformanceById(performance3.Id);
            editperformance3.Artist = "Headhunterz";
            repo.EditPerformance(editperformance3);

            //Test if edit saved with succes
            Performance testPerformance = repo.PerformanceById(performance3.Id);
            Assert.AreEqual("Headhunterz", testPerformance.Artist);
        }
    }
}