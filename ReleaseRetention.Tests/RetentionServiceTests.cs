using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ReleaseRetention.Data.Services;
using ReleaseRetention.Extensions;
using ReleaseRetention.Models;
using ReleaseRetention.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReleaseRetention.Tests
{
    public class RetentionService
    {
        private ReleaseRetentionService _releaseRetentionService;
        private IEnumerable<ProjectModel> _projects;
        private IEnumerable<EnvironmentModel> _environments;
        private IEnumerable<ReleaseModel> _releases;
        private IEnumerable<DeploymentModel> _deployments;


        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            SetupMockedData(services);
            services.AddReleaseRetention();
            var serviceProvider = services.BuildServiceProvider();
            _releaseRetentionService = serviceProvider.GetRequiredService<ReleaseRetentionService>();
        }

        [Test]
        public void Should_Return_Expected_Projects()
        {
            var releasesKept = _releaseRetentionService.GetRecentReleases();

            // Assert 2 project/environment combinations
            releasesKept.Should().HaveCount(4);
        }

        [Test]
        public void Should_Return_All_Releases()
        {
            var releasesKept = _releaseRetentionService.GetRecentReleases();

            // Assert the exact number of releases returned per combination
            // TODO: We should just compare the result against an expected object state instead of using this syntax..
            releasesKept.Should().SatisfyRespectively(
                r =>
                {
                    r.ProjectName.Should().Be("Project 2");
                    r.EnvironmentName.Should().Be("Development");
                    r.Releases.Should().HaveCount(3);
                    r.Releases.Select(r => r.ReleaseId).Should().BeEquivalentTo(new[] { "r7", "r4", "r5" });
                    r.Releases.Select(r => r.DeploymentId).Should().BeEquivalentTo(new[] { "d9", "d4", "d6" });
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 1");
                    r.EnvironmentName.Should().Be("Development");
                    r.Releases.Should().HaveCount(2);
                    r.Releases.Select(r => r.ReleaseId).Should().BeEquivalentTo(new[] { "r3", "r1" });
                    r.Releases.Select(r => r.DeploymentId).Should().BeEquivalentTo(new[] { "d3", "d1" });
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 1");
                    r.EnvironmentName.Should().Be("Production");
                    r.Releases.Should().HaveCount(1);
                    r.Releases.First().ReleaseId.Should().Be("r2");
                    r.Releases.First().DeploymentId.Should().Be("d2");
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 2");
                    r.EnvironmentName.Should().Be("Production");
                    r.Releases.Should().HaveCount(1);
                    r.Releases.First().ReleaseId.Should().Be("r4");
                    r.Releases.First().DeploymentId.Should().Be("d5");
                });
        }

        [Test]
        public void Should_Return_Up_To_1_Release()
        {
            var releasesKept = _releaseRetentionService.GetRecentReleases(1);

            // Assert 2 project/environment combinations
            releasesKept.Should().HaveCount(4);

            // Assert 1 release returned per combination
            // TODO: We should just compare the result against an expected object state instead of using this syntax..
            releasesKept.Should().SatisfyRespectively(
                r =>
                {
                    r.ProjectName.Should().Be("Project 2");
                    r.EnvironmentName.Should().Be("Development");
                    r.Releases.Should().HaveCount(1);
                    r.Releases.First().ReleaseId.Should().Be("r7");
                    r.Releases.First().DeploymentId.Should().Be("d9");
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 1");
                    r.EnvironmentName.Should().Be("Development");
                    r.Releases.Should().HaveCount(1);
                    r.Releases.First().ReleaseId.Should().Be("r3");
                    r.Releases.First().DeploymentId.Should().Be("d3");
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 1");
                    r.EnvironmentName.Should().Be("Production");
                    r.Releases.Should().HaveCount(1);
                    r.Releases.First().ReleaseId.Should().Be("r2");
                    r.Releases.First().DeploymentId.Should().Be("d2");
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 2");
                    r.EnvironmentName.Should().Be("Production");
                    r.Releases.Should().HaveCount(1);
                    r.Releases.First().ReleaseId.Should().Be("r4");
                    r.Releases.First().DeploymentId.Should().Be("d5");
                });
        }

        [Test]
        public void Should_Return_Up_To_2_Release()
        {
            var releasesKept = _releaseRetentionService.GetRecentReleases(2);

            // Assert 2 project/environment combinations
            releasesKept.Should().HaveCount(4);

            // Assert up to 2 release returned per combination
            // TODO: We should just compare the result against an expected object state instead of using this syntax..
            releasesKept.Should().SatisfyRespectively(
                r =>
                {
                    r.ProjectName.Should().Be("Project 2");
                    r.EnvironmentName.Should().Be("Development");
                    r.Releases.Should().HaveCount(2);
                    r.Releases.Select(r => r.ReleaseId).Should().BeEquivalentTo(new[] { "r7", "r4" });
                    r.Releases.Select(r => r.DeploymentId).Should().BeEquivalentTo(new[] { "d9", "d4" });
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 1");
                    r.EnvironmentName.Should().Be("Development");
                    r.Releases.Should().HaveCount(2);
                    r.Releases.Select(r => r.ReleaseId).Should().BeEquivalentTo(new[] { "r3", "r1" });
                    r.Releases.Select(r => r.DeploymentId).Should().BeEquivalentTo(new[] { "d3", "d1" });
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 1");
                    r.EnvironmentName.Should().Be("Production");
                    r.Releases.Should().HaveCount(1);
                    r.Releases.First().ReleaseId.Should().Be("r2");
                    r.Releases.First().DeploymentId.Should().Be("d2");
                },
                r =>
                {
                    r.ProjectName.Should().Be("Project 2");
                    r.EnvironmentName.Should().Be("Production");
                    r.Releases.Should().HaveCount(1);
                    r.Releases.First().ReleaseId.Should().Be("r4");
                    r.Releases.First().DeploymentId.Should().Be("d5");
                });
        }

        private void SetupMockedData(IServiceCollection services)
        {
            var mockedProjectService = new Moq.Mock<ProjectDataService>();
            mockedProjectService.Setup(m => m.Query()).Returns(() => _projects.AsQueryable());
            services.AddScoped(sp => mockedProjectService.Object);

            var mockedEnvironmentService = new Moq.Mock<EnvironmentDataService>();
            mockedEnvironmentService.Setup(m => m.Query()).Returns(() => _environments.AsQueryable());
            services.AddScoped(sp => mockedEnvironmentService.Object);

            var mockedReleaseService = new Moq.Mock<ReleaseDataService>();
            mockedReleaseService.Setup(m => m.Query()).Returns(() => _releases.AsQueryable());
            services.AddScoped(sp => mockedReleaseService.Object);

            var mockedDeploymentService = new Moq.Mock<DeploymentDataService>();
            mockedDeploymentService.Setup(m => m.Query()).Returns(() => _deployments.AsQueryable());
            services.AddScoped(sp => mockedDeploymentService.Object);

            _projects = new[]
            {
                new ProjectModel
                {
                    Id = "p1",
                    Name = "Project 1"
                },
                new ProjectModel
                { 
                    Id = "p2",
                    Name = "Project 2"
                }
            };

            _environments = new[]
            {
                new EnvironmentModel
                {
                    Id = "dev",
                    Name = "Development"
                },
                new EnvironmentModel
                {
                    Id = "prod",
                    Name = "Production"
                }
            };

            _releases = new[]
            {
                new ReleaseModel
                {
                    Id = "r1",
                    Created = DateTime.Now.AddDays(-10),
                    ProjectId = "p1",
                    Version = "1.0"
                },
                new ReleaseModel
                {
                    Id = "r2",
                    Created = DateTime.Now.AddDays(-9),
                    ProjectId = "p1",
                    Version = "1.0"
                },
                new ReleaseModel
                {
                    Id = "r3",
                    Created = DateTime.Now.AddDays(-8),
                    ProjectId = "p1",
                    Version = "1.1"
                },
                new ReleaseModel
                {
                    Id = "r4",
                    Created = DateTime.Now.AddDays(-10),
                    ProjectId = "p2",
                    Version = "1.0"
                },
                new ReleaseModel
                {
                    Id = "r5",
                    Created = DateTime.Now.AddDays(-9),
                    ProjectId = "p2",
                    Version = "1.1"
                },
                new ReleaseModel
                {
                    Id = "r6",
                    Created = DateTime.Now.AddDays(-8),
                    ProjectId = "p4",
                    Version = "1.2"
                },
                new ReleaseModel
                {
                    Id = "r7",
                    Created = DateTime.Now.AddDays(-6),
                    ProjectId = "p2",
                    Version = "1.3"
                }
            };

            _deployments = new[]
            {
                new DeploymentModel
                {
                    Id = "d1",
                    EnvironmentId = "dev",
                    ReleaseId = "r1",
                    DeployedAt = DateTime.Now.AddDays(-6)
                },
                new DeploymentModel
                {
                    Id = "d2",
                    EnvironmentId = "prod",
                    ReleaseId = "r2",
                    DeployedAt = DateTime.Now.AddDays(-5)
                },
                new DeploymentModel
                {
                    Id = "d3",
                    EnvironmentId = "dev",
                    ReleaseId = "r3",
                    DeployedAt = DateTime.Now.AddDays(-4)
                },
                new DeploymentModel
                {
                    Id = "d4",
                    EnvironmentId = "dev",
                    ReleaseId = "r4",
                    DeployedAt = DateTime.Now.AddDays(-3)
                },
                new DeploymentModel
                {
                    Id = "d5",
                    EnvironmentId = "prod",
                    ReleaseId = "r4",
                    DeployedAt = DateTime.Now.AddDays(-7)
                },
                new DeploymentModel
                {
                    Id = "d6",
                    EnvironmentId = "dev",
                    ReleaseId = "r5",
                    DeployedAt = DateTime.Now.AddDays(-6)
                },
                new DeploymentModel
                {
                    Id = "d7",
                    EnvironmentId = "invalid",
                    ReleaseId = "r5",
                    DeployedAt = DateTime.Now.AddDays(-5)
                },
                new DeploymentModel
                {
                    Id = "d8",
                    EnvironmentId = "prod",
                    ReleaseId = "r6",
                    DeployedAt = DateTime.Now.AddDays(-4)
                },
                new DeploymentModel
                {
                    Id = "d9",
                    EnvironmentId = "dev",
                    ReleaseId = "r7",
                    DeployedAt = DateTime.Now.AddDays(-3)
                }
            };
        }
    }
}