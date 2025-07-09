using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SemantickKerenelDesktopApp.Model
{
    public enum Gender
    {
        Male,
        female,
    }

    public class FamilyPlugin
    {
        private readonly List<FamilyMemberModel> familyMembers =
        [
            new FamilyMemberModel { Id = 1, Name = "Habtie", Age = 30, Relation = "hasband" ,Gender =Gender.Male },
            new FamilyMemberModel { Id = 2, Name = "jo", Age = 35, Relation = "Child" , Gender=Gender.Male},
            new FamilyMemberModel { Id = 3, Name = "seble", Age = 45, Relation = "Friend", Gender=Gender.female },
            new FamilyMemberModel { Id = 4, Name = "Merisi", Age = 50, Relation = "Child" ,Gender=Gender.Male},
            new FamilyMemberModel { Id = 5, Name = "Mimi", Age = 55, Relation = "Sister", Gender=Gender.female },
            new FamilyMemberModel { Id = 6, Name = "wub", Age = 55, Relation = "Sister",Gender=Gender.female },
            new FamilyMemberModel { Id = 7, Name = "sis", Age = 60, Relation = "Father",Gender=Gender.Male },
            new FamilyMemberModel { Id = 8, Name = "Tsedy", Age = 65, Relation = "Frind" , Gender=Gender.female},
            new FamilyMemberModel { Id = 9, Name = "sara", Age = 70, Relation = "child" , Gender=Gender.female},
            ];

        [KernelFunction("show_details_for_a_family_member")]
        [Description("Get details of a family member by name")]
        public List<FamilyMemberModel> GetFamilyMembers()
        {
            return familyMembers;
        }

        [KernelFunction("Show_details")]
       [Description("show details of a family member by name")]
        public FamilyMemberModel GetFamilyMemberdetail(string familyName)
        {
            var member = familyMembers.FirstOrDefault(m => m.Name.Equals(familyName, StringComparison.OrdinalIgnoreCase));
            return member ?? null;
        }
    }

    public class FamilyMemberModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("relation")]
        public string Relation { get; set; }

        [JsonPropertyName("gender")]
        public Gender Gender { get; set; }

    }
}
